using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class UNCCredentials
    {
        public string UNCPath { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
    }
    public class UNCAccess
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal String ui2_local;
            internal String ui2_remote;
            internal String ui2_password;
            internal UInt32 ui2_status;
            internal UInt32 ui2_asg_type;
            internal UInt32 ui2_refcount;
            internal UInt32 ui2_usecount;
            internal String ui2_username;
            internal String ui2_domainname;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseAdd(
            String UncServerName,
            UInt32 Level,
            ref USE_INFO_2 Buf,
            out UInt32 ParmError);

        private string sUNCPath;
        private string sUser;
        private string sPassword;
        private string sDomain;
        private int iLastError;
        public UNCAccess()
        {
        }

        public static UNCAccess Access(Action<UNCCredentials> uNCCredentials)
        {
            return new UNCAccess(uNCCredentials);
        }
        public UNCAccess(Action<UNCCredentials> uNCCredentials)
        {
            var credential = new UNCCredentials();
            uNCCredentials(credential);
            login(credential.UNCPath, credential.UserName, credential.Domain, credential.Password);
        }
        public int LastError
        {
            get { return iLastError; }
        }

        /// <summary>
        /// Connects to a UNC share folder with credentials
        /// </summary>
        /// <param name="UNCPath">UNC share path</param>
        /// <param name="User">Username</param>
        /// <param name="Domain">Domain</param>
        /// <param name="Password">Password</param>
        /// <returns>True if login was successful</returns>
        public bool login(string UNCPath, string User, string Domain, string Password)
        {
            sUNCPath = UNCPath;
            sUser = User;
            sPassword = Password;
            sDomain = Domain;
            return NetUseWithCredentials();
        }
        private bool NetUseWithCredentials()
        {
            uint returncode;
            try
            {
                USE_INFO_2 useinfo = new USE_INFO_2();

                useinfo.ui2_remote = sUNCPath;
                useinfo.ui2_username = sUser;
                useinfo.ui2_domainname = sDomain;
                useinfo.ui2_password = sPassword;
                useinfo.ui2_asg_type = 0;
                useinfo.ui2_usecount = 1;
                uint paramErrorIndex;
                returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                iLastError = (int)returncode;
                return returncode == 0;
            }
            catch
            {
                iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }
    }
}
