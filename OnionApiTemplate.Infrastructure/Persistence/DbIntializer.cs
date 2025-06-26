using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionApiTemplate.Domain.Entities;
using OnionApiTemplate.Domain.IRepositoty;
using System.Text.Json;

namespace OnionApiTemplate.Infrastructure.Persistence
{
    public class DbInitializer(ApplicationDbContext _context, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        : IDbInitializer
    {

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();

                if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _context.Database.MigrateAsync();
                }
                //Seed any Data here
                //if (!await _context.Set<Entity>().AnyAsync())
                //{
                //    var data = await ReadFileAsync("FileName.json");
                //    var entities = JsonSerializer.Deserialize<List<Entity>>(data);

                //    if (entities != null && entities.Any())
                //    {
                //        await _context.Set<Entity>().AddRangeAsync(entities);
                //        await _context.SaveChangesAsync();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }

        public async Task InitializeIdentityAsync()
        {
            //if ((await _identityContext.Database.GetPendingMigrationsAsync()).Any())
            //    await _identityContext.Database.MigrateAsync();

            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!await _userManager.Users.AnyAsync())
            {
                var superAdminUser = new ApplicationUser
                {
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890",
                };
                var adminUser = new ApplicationUser
                {
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "1234567890",
                };

                await _userManager.CreateAsync(superAdminUser, "P@ssW0rd123");
                await _userManager.CreateAsync(adminUser, "P@ssW0rd123");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        private static async Task<string> ReadFileAsync(string relativePath)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Seeding");
            var fullPath = Path.Combine(basePath, relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Seed file not found: {fullPath}");

            return await File.ReadAllTextAsync(fullPath);
        }
    }
}
