using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace BackEnd.Models
{
    public class Speaker : ConferenceDTO.Speaker
    {
        
    }
    
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args) =>
            Program.CreateWebHostBuilder(args).Build().Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}