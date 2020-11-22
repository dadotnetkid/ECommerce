
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using Helpers;
using Microsoft.AspNet.Identity;
using Models;
using Newtonsoft.Json;

namespace Models.Repository
{
    public class SearchProperty
    {
        public string Operand { get; set; }
        public string FieldName { get; set; }
        public string Search { get; set; }
    }
    public partial class GenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;


        public GenericRepository(DbContext context)
        {
            this.context = context;
#if DEBUG
            context.Database.Log = (s) => { Debug.WriteLine(s); };
#endif
            this.dbSet = context.Set<TEntity>();

        }

        public virtual void InsertIdentity(string tableName, string query)
        {
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[" + tableName + "] ON; " + query + ";SET IDENTITY_INSERT [dbo].[" + tableName + "] OFF");
        }

        public virtual void ExecuteQuery(string query, params string[] parameters)
        {
            context.Database.ExecuteSqlCommand(query, parameters);
        }

        public virtual IEnumerable<TEntity> Paginate(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int take = 10,
            int skip = 0,
            Expression<Func<TEntity, bool>> filter = null,

            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).Take(take).Skip(skip).AsNoTracking().ToList();
                //return orderBy(query);
            }

            return query;
        }
        public virtual List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
                //return orderBy(query);
            }
            else
            {
                return query.ToList();
                //return query;
            }
        }
        public virtual IEnumerable<object> Get(
             Expression<Func<TEntity, object>> selector)
        {
            IQueryable<TEntity> query = dbSet;


            return query.Select(selector).ToList();
        }
        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public virtual IQueryable<TEntity> Search(string query, params string[] parameters)
        {
            return context.Database.SqlQuery<TEntity>(query, parameters).AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> ApplyFilter(Action<SearchProperty> expression)
        {
            SearchProperty searchProperty = new SearchProperty();
            IQueryable query = dbSet;
            expression(searchProperty);
            CriteriaOperator criteria = CriteriaOperator.Parse(searchProperty.FieldName + " " + searchProperty.Operand + " " + searchProperty.Search);
            if (CriteriaValidator.IsCriteriaOperatorValid(criteria))
                query = query.MakeSelect(Converter, criteria).AppendWhere(Converter, criteria);
            return query as IQueryable<TEntity>;
        }
        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query.Where(filter).FirstOrDefault();
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, bool proxyCreationEnable, bool asNoTracking = false, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return await query.Where(filter).FirstOrDefaultAsync();
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> filter, bool proxyCreationEnable, bool asNoTracking = false, string includeProperties = "")
        {
            this.context.Configuration.ProxyCreationEnabled = proxyCreationEnable;
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            if (asNoTracking)
                return query.AsNoTracking().Where(filter).FirstOrDefault();
            return query.Where(filter).FirstOrDefault();
        }
        public virtual TEntity Find(string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query.FirstOrDefault();
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
        {

            return await dbSet.Where(filter).FirstOrDefaultAsync();
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void InsertRange(IEnumerable<TEntity> entity)
        {
            dbSet.AddRange(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            Delete(this.Find(filter));
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            dbSet.RemoveRange(dbSet.Where(filter));

        }
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var res = await this.FindAsync(filter);
            dbSet.Attach(res);
            dbSet.Remove(res);
            return await context.SaveChangesAsync();
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            //           context.Entry(entityToUpdate).State = EntityState.Detached;
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual void UpdateRange(List<TEntity> entityToUpdate)
        {
            //           context.Entry(entityToUpdate).State = EntityState.Detached;
            foreach (var item in entityToUpdate)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            var entity = await dbSet.Where(filter).ToListAsync();
            return entity;
        }
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);
            return await context.SaveChangesAsync();
        }
        public virtual async Task<TEntity> CloneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await dbSet.Where(filter).AsNoTracking().FirstOrDefaultAsync();
        }
        public virtual TEntity Clone(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (includeProperties != "")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            return query.Where(filter).AsNoTracking().FirstOrDefault();
        }
        public void Detach(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        public void DetachRange(IEnumerable<TEntity> entity)
        {
            dbSet.RemoveRange(entity);
        }

     
        static ICriteriaToExpressionConverter Converter { get { return new CriteriaToExpressionConverter(); } }
        //public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, string filterExpression)
        //{
        //    CriteriaOperator criteria = CriteriaOperator.Parse(filterExpression);
        //    if (CriteriaValidator.IsCriteriaOperatorValid(criteria))
        //        //    IQueryable sql = query.AppendWhere(Converter, criteria).AsQueryable();
        //        query = CriteriaToQueryableExtender.AppendWhere(query, new CriteriaToEFExpressionConverter(), criteria);
        //    return query;
        //}
    }


}