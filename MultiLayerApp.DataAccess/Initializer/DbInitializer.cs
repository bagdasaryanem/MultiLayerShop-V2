using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiLayerApp.DataAccess.Data;
using MultiLayerApp.Utility;
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly ShopDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ILogger<DbInitializer> logger,ShopDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Initial migrations failed, unable to start application");
            }

            if (_db.Roles.Any(r => r.Name == StaticDetails.RoleAdmin)) return;

            _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleAdmin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleEmployee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new User
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Artur Bagdasaryan"
            }, "@Admin12").GetAwaiter().GetResult();

            User user = _db.ShopUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");

            _userManager.AddToRoleAsync(user, StaticDetails.RoleAdmin).GetAwaiter().GetResult();
        }
    }
}
