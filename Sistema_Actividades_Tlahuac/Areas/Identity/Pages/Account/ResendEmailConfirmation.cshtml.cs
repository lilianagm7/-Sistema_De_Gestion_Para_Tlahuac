using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_Actividades_Tlahuac.Models.Actores;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Sistema_Actividades_Tlahuac.Areas.Identity.Pages.Account
{
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return Page();
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { area = "Identity", userId, code },
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                Email,
                "Confirma tu cuenta",
                $"Confirma aquí: <a href='{callbackUrl}'>Confirmar</a>");

            TempData["Mensaje"] = "Si el correo existe, se ha enviado un nuevo enlace de confirmación.";

            return Page();
        }
    }
}
