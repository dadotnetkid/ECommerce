using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Models.Repository
{
    public partial class UnitOfWork : IDisposable
    {
        public DbContext context;

        public UnitOfWork()
        {
            context = ModelDb.Create();//(/*DataSource.ConnectionString ?? context.Database.Connection.ConnectionString*/);
        }

        public UnitOfWork(DbContext dbContext)
        {
            this.context = dbContext;
        }
        public UnitOfWork(UnitOfWorkSettings settings)
        {

        }

        private GenericRepository<Reviews> _reviewsRepo;
        public GenericRepository<Reviews> ReviewsRepo
        {
            get
            {
                if (this._reviewsRepo == null)
                    this._reviewsRepo = new GenericRepository<Reviews>(context);
                return _reviewsRepo;
            }
            set { _reviewsRepo = value; }
        }
        private GenericRepository<OrderStatus> _OrderStatusRepo;
        public GenericRepository<OrderStatus> OrderStatusRepo
        {
            get
            {
                if (this._OrderStatusRepo == null)
                    this._OrderStatusRepo = new GenericRepository<OrderStatus>(context);
                return _OrderStatusRepo;
            }
            set { _OrderStatusRepo = value; }
        }

        private GenericRepository<Orders> _OrdersRepo;
        public GenericRepository<Orders> OrdersRepo
        {
            get
            {
                if (this._OrdersRepo == null)
                    this._OrdersRepo = new GenericRepository<Orders>(context);
                return _OrdersRepo;
            }
            set { _OrdersRepo = value; }
        }
        private GenericRepository<Carts> _CartsRepo;
        public GenericRepository<Carts> CartsRepo
        {
            get
            {
                if (this._CartsRepo == null)
                    this._CartsRepo = new GenericRepository<Carts>(context);
                return _CartsRepo;
            }
            set { _CartsRepo = value; }
        }
        private GenericRepository<Products> _ProductsRepo;
        public GenericRepository<Products> ProductsRepo
        {
            get
            {
                if (this._ProductsRepo == null)
                    this._ProductsRepo = new GenericRepository<Products>(context);
                return _ProductsRepo;
            }
            set { _ProductsRepo = value; }
        }
        private GenericRepository<Functions> _FunctionsRepo;
        public GenericRepository<Functions> FunctionsRepo
        {
            get
            {
                if (this._FunctionsRepo == null)
                    this._FunctionsRepo = new GenericRepository<Functions>(context);
                return _FunctionsRepo;
            }
            set { _FunctionsRepo = value; }
        }

        public UnitOfWork(bool lazyLoadingEnabled, bool proxyCreationEnabled)
        {
            context = ModelDb.Create(DataSource.ConnectionString);//(/*DataSource.ConnectionString ?? context.Database.Connection.ConnectionString*/);
            this.context.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
            this.context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }

        

        private GenericRepository<Users> usersRepo;
        public GenericRepository<Users> UsersRepo
        {
            get
            {
                if (this.usersRepo == null)
                    this.usersRepo = new GenericRepository<Users>(context);
                return usersRepo;
            }
            set { usersRepo = value; }
        }

        private GenericRepository<UserRoles> userRolesRepo;
        public GenericRepository<UserRoles> UserRolesRepo
        {
            get
            {
                if (this.userRolesRepo == null)
                    this.userRolesRepo = new GenericRepository<UserRoles>(context);
                return userRolesRepo;
            }
            set { userRolesRepo = value; }
        }




        public void Save()
        {
            context.SaveChanges();

        }


        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    public class UnitOfWorkSettings
    {
        private bool _proxyCreationEnabled;
        private bool _lazyLoading;

        public bool LazyLoading
        {
            get
            {
                if (_lazyLoading == null)
                    _lazyLoading = true;
                return _lazyLoading;
            }
            set => _lazyLoading = value;
        }

        public bool AsNoTracking { get; set; }

        public bool ProxyCreationEnabled
        {
            get
            {
                if (_proxyCreationEnabled == null)
                    _proxyCreationEnabled = true;
                return _proxyCreationEnabled;
            }
            set => _proxyCreationEnabled = value;
        }
    }
}