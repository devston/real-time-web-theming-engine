namespace RealTimeThemingEngine.ThemeManagement.Data
{
    using RealTimeThemingEngine.ThemeManagement.Data.Entities;
    using System.Data.Entity;

    public class ThemeDbContext : DbContext
    {
        // Your context has been configured to use a 'ThemeDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'RealTimeThemingEngine.ThemeManagement.Data.ThemeDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ThemeDbContext' 
        // connection string in the application configuration file.
        public ThemeDbContext()
            : base("name=ThemeDbContext")
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