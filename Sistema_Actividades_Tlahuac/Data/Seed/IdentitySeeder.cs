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

            //Crear usuario normal
            string userEmail = "liliana@gmail.com";
            string userPassword = "Mipagina123*";

            var User_1 = await userManager.FindByEmailAsync(userEmail);


            if (User_1 == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    Nombre = "Liliana",
                    ApellidoPaterno = "Gerardo",
                    ApellidoMaterno = "Mendez"

                };

                var result = await userManager.CreateAsync(user, userPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Usuario");
                }
            }


            //Crear usuario coordinador
            string coorEmail = "coordinador@tlahuac.com";
            string coorPassword = "Mipagina123*";

            var coordinador1 = await userManager.FindByEmailAsync(coorEmail);


            if (coordinador1 == null)
            {
                var user = new ApplicationUser
                {
                    UserName = coorEmail,
                    Email = coorEmail,
                    EmailConfirmed = true,
                    Nombre = "Coordinador",
                    ApellidoPaterno = "de",
                    ApellidoMaterno = "Tlahuac"

                };

                var result = await userManager.CreateAsync(user, coorPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Coordinador");
                }
            }
        }
    }
}