using Microsoft.EntityFrameworkCore;

namespace passwordManagent2.Models
{
    public class PasswordManagerContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public PasswordManagerContext(DbContextOptions<PasswordManagerContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        // tabla passwordEntry que se encuentra en la clase passwordEntry
        public DbSet<PasswordEntry> passwordEntry { get; set; } = null;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // obtiene la cadena de configuración del archivo appsettings.json
        //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
