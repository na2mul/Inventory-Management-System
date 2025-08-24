using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Infrastructure.Identity.Requirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
        }
        public static void AddPolicy(this IServiceCollection services)
        {            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomAdminAccess", policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireRole("Support");
                    policy.RequireRole("HR");
                });

                options.AddPolicy("CustomAccess", policy =>
                {
                    policy.RequireRole("HR");
                    policy.RequireRole("Support");
                });
            });

            services.AddAuthorization(options =>
            {               
                options.AddPolicy("UserAddPermission", policy =>
                {
                    policy.RequireClaim("create_user", "allowed");
                });

                options.AddPolicy("AddPermission", policy =>
                {
                    policy.RequireClaim("Add", "allowed");
                });

                options.AddPolicy("UpdatePermission", policy =>
                {
                    policy.RequireClaim("Update", "allowed");
                });

                options.AddPolicy("DeletePermission", policy =>
                {
                    policy.RequireClaim("Delete", "allowed");
                });
                options.AddPolicy("AgeRestriction", policy =>
                {
                    policy.Requirements.Add(new AgeRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, AgeRequirementHandler>();
        }

        // Extension method for seeding admin roles and user
        public static async Task SeedAdminUserAndRolesAsync(this IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] rolesName = { "Admin", "Support", "HR" };
            string adminEmail = "nazmul@gmail.com";
            string adminPassword = "123456";

            // Retrieve claims from the database dynamically (or set them manually)
            var userClaims = new List<Claim>
            {
                new Claim("create_user", "allowed"),
                new Claim("Add", "allowed"),
                new Claim("Update", "allowed"),
                new Claim("Delete", "allowed")
            };

            // Ensure roles exist
            foreach (var roleName in rolesName)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            // Create admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    // Assign roles to the admin user
                    foreach (var roleName in rolesName)
                    {
                        await userManager.AddToRoleAsync(adminUser, roleName);
                    }

                    // Add claims to the user
                    foreach (var claim in userClaims)
                    {
                        var existingClaim = (await userManager.GetClaimsAsync(adminUser))
                                            .FirstOrDefault(c => c.Type == claim.Type);

                        if (existingClaim == null)
                        {
                            await userManager.AddClaimAsync(adminUser, claim);
                        }
                    }
                }
                else
                {
                    // Log errors (optional)
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else if (!adminUser.EmailConfirmed)
            {
                adminUser.EmailConfirmed = true;
                await userManager.UpdateAsync(adminUser);
            }
        }

    }
}
