using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public partial class ModelDb : DbContext
    {
        public ModelDb(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }
        public static ModelDb Create()
        {

            return new ModelDb();
        }
        public static ModelDb Create(string providerConnectionString)
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            // use your ADO.NET connection string
            entityBuilder.ProviderConnectionString = providerConnectionString;

            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the Metadata location. metadata=res://*/ModelDb.csdl|res://*/ModelDb.ssdl|res://*/ModelDb.msl
            entityBuilder.Metadata = @"res://*/ModelDb.csdl|res://*/ModelDb.ssdl|res://*/ModelDb.msl";

            return new ModelDb(entityBuilder.ConnectionString);
        }
        public static void SetConnection(string connection, string userName, string password)
        {

        }
    }
    public static class DataSource
    {
        public static string ConnectionString { get; set; }
        public static string OFMISConnectionString { get; set; }
        public static string DMSConnectionString { get; set; }
    }
    //public class MyDbConfiguration : DbConfiguration
    //{
    //    public MyDbConfiguration() : base()
    //    {
    //        var path = Path.GetDirectoryName(this.GetType().Assembly.Location);
    //        SetModelStore(new DefaultDbModelStore(path));
    //    }
    //}
}
