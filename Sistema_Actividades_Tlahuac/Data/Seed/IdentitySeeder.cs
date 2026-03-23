using Microsoft.AspNetCore.Identity;
using Sistema_Actividades_Tlahuac.Models.Actores;
namespace Sistema_Actividades_Tlahuac.Data.Seed
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Definicion de roles
            string[] roles = { "Administrador", "Usuario" , "Instructor", "Coordinador" };

            //Crear roles si no existen
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Crear usuario administrador
            string adminEmail = "admin@tlahuac.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nombre = "Administrador",
                    ApellidoPaterno = "del",
                    ApellidoMaterno = "Sistema"

                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }
        }
    }
}