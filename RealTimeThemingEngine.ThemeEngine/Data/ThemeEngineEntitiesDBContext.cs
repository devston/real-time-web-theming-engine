namespace RealTimeThemingEngine.ThemeEngine.Data
{
    using RealTimeThemingEngine.ThemeEngine.Data.Entities;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public class ThemeEngineEntitiesDBContext : DbContext
    {
        // Your context has been configured to use a 'ThemeEngineEntitiesDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'RealTimeThemingEngine.ThemeEngine.Data.ThemeEngineEntitiesDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ThemeEngineEntitiesDBContext' 
        // connection string in the application configuration file.
        public ThemeEngineEntitiesDBContext()
            : base("name=ThemeEngineEntitiesDBContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<ActiveTheme> ActiveThemes { get; set; }
    }
}