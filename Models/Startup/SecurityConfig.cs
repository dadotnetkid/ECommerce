using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Models.Startup
{
   
        public class ApplicationUserManager : UserManager<Users, string>
        {
            public ApplicationUserManager(IUserStore<Users, string> store)
                : base(store)
            {
            }

            public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            {
                var userStores = new UserStores(context.Get<ModelDb>());
                var userManager = new UserManager<Users, string>(userStores);
                var manager = new ApplicationUserManager(userStores);
                // Configure validation logic for usernames
                manager.UserValidator = new UserValidator<Users>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = false
                    
                };

                // Configure validation logic for passwords
                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 0,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                // Configure user lockout defaults
                manager.UserLockoutEnabledByDefault = true;
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                manager.MaxFailedAccessAttemptsBeforeLockout = 5;

                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug it in here.
                manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<Users>
                {
                    MessageFormat = "Your security code is {0}"
                });
                manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Users>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                });
                manager.EmailService = new EmailService();
                manager.SmsService = new SmsService();
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider =
                        new DataProtectorTokenProvider<Users>(dataProtectionProvider.Create("ASP.NET Identity"));
                }
                return manager;
            }
            public async Task<IdentityResult> ChangePasswordAsync(Users userId, string newPassword)
            {
                var store = this.Store as IUserPasswordStore<Users, string>;
                //if (store == null) {
                //    var errors = new string[]
                //    {
                //    "Current UserStore doesn't implement IUserPasswordStore"
                //    };

                //    return Task.FromResult<IdentityResult>(new IdentityResult(errors) {  Succeeded = false });
                //}

                var newPasswordHash = this.PasswordHasher.HashPassword(newPassword);

                await store.SetPasswordHashAsync(userId, newPasswordHash);
                return await Task.FromResult<IdentityResult>(IdentityResult.Success);
            }
    }

        // Configure the application sign-in manager which is used in this application.
        public class ApplicationSignInManager : SignInManager<Users, string>
        {
            public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
            {
            }

            public override Task<ClaimsIdentity> CreateUserIdentityAsync(Users user)
            {
                return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
            }

            public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
            {
                return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            }
        }

        public class ApplicationRoleManager : RoleManager<UserRoles>
        {
            public ApplicationRoleManager(IRoleStore<UserRoles, string> roleStore)
                : base(roleStore)
            {
            }
            public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
            {
                var roleStore = new RoleStores(context.Get<ModelDb>());
                return new ApplicationRoleManager(roleStore);
            }
        }


        public class EmailService : IIdentityMessageService
    {
        private string Email = "careers@northops.asia";
        public Task SendAsync(IdentityMessage message)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

            //initialization
            var mailMessage = new System.Net.Mail.MailMessage();
            //from
            mailMessage.From = new System.Net.Mail.MailAddress(Email, "NorthOps");
            //to
            mailMessage.To.Add(new System.Net.Mail.MailAddress(message.Destination));
            mailMessage.Body = message.Body;
            mailMessage.Subject = message.Subject;
            mailMessage.IsBodyHtml = true;

            //client.SendAsync(mailMessage, null);
            Task task = Task.Run(new Action(async () =>
            {
                await client.SendMailAsync(mailMessage);

            }));
            return task;
        }

        public Task SendAsync(MailMessage mailMessage)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

            Task task = Task.Run(new Action(async () =>
            {
                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }


            }));

            return task;
        }
    }

        public class SmsService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your SMS service here to send a text message.
                return Task.FromResult(0);
            }
        }
    }
