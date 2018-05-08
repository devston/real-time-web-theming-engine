namespace RealTimeThemingEngine.ThemeManagement.Data
{
    using RealTimeThemingEngine.ThemeManagement.Data.Entities;
    using System.Data.Entity;

    public class ThemeEntitiesDBContext : DbContext
    {
        // Your context has been configured to use a 'ThemeEntitiesDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'RealTimeThemingEngine.ThemeManagement.Data.ThemeEntitiesDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ThemeEntitiesDBContext' 
        // connection string in the application configuration file.
        public ThemeEntitiesDBContext()
            : base("name=ThemeEntitiesDBContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<ThemeVariable> ThemeVariables { get; set; }
        public virtual DbSet<ThemeVariableCategory> ThemeVariableCategories { get; set; }
        public virtual DbSet<ThemeVariableType> ThemeVariableTypes { get; set; }
        public virtual DbSet<ThemeVariableValue> ThemeVariableValues { get; set; }
    }
}