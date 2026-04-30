// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Sistema_Actividades_Tlahuac.Models.Actores;

namespace Sistema_Actividades_Tlahuac.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Nombres")]
            public string Nombre { get; set; }
            [Required]
            [Display(Name = "Apellido paterno")]
            public string ApellidoPaterno { get; set; }
            [Required]
            [Display(Name = "Apellido materno")]
            public string ApellidoMaterno { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                /* Modificacion de la variable
                 * var user = CreateUser();
                */
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nombre = Input.Nombre,
                    ApellidoPaterno = Input.ApellidoPaterno,
                    ApellidoMaterno = Input.ApellidoMaterno,
                    FechaCreacion = DateTime.Now
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("La nueva cuenta ha sido creada exitosamente.");

                    //Generar token
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    //Codificar token (IMPORTANTE)
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    //Crear link de confirmación
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);
                    // Correo de confirmación profesional
                    var mensaje = $@"
                        <div style='
                            font-family: Arial, sans-serif;
                            background-color: #800020;
                            padding: 40px 20px;
                            margin: 0;
                        '>

                        <div style='
                            max-width: 600px;
                            margin: auto;
                            background: #ffffff;
                            border-radius: 12px;
                            overflow: hidden;
                            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
                        '>

                            <!-- Header -->
                            <div style='
                                background-color: #7B3F61;
                                padding: 30px;
                                text-align: center;
                                color: white;
                            '>
                                <h1 style='margin:0; font-size: 26px;'>
                                    Actividades y eventos Tláhuac
                                </h1>
                            </div>

                            <!-- Body -->
                            <div style='padding: 40px 30px;'>

                                <h2 style='
                                    margin-top: 0;
                                    color: #333;
                                    font-size: 22px;
                                '>
                                    Hola, {Input.Nombre}
                                </h2>

                                <p style='
                                    color: #555;
                                    line-height: 1.7;
                                    font-size: 15px;
                                '>
                                    Gracias por registrarte en nuestra plataforma.
                                    Para activar tu cuenta y comenzar a utilizar el sistema,
                                    es necesario confirmar tu correo electrónico.
                                </p>

                                <p style='
                                    color: #555;
                                    line-height: 1.7;
                                    font-size: 15px;
                                '>
                                    Haz clic en el siguiente botón para completar el proceso:
                                </p>

                                <!-- Botón -->
                                <div style='text-align: center; margin: 35px 0;'>
                                    <a href='{callbackUrl}' style='
                                        display: inline-block;
                                        padding: 14px 28px;
                                        background-color: #D4AF37;
                                        color: white;
                                        text-decoration: none;
                                        border-radius: 8px;
                                        font-weight: bold;
                                        font-size: 15px;
                                    '>
                                        Confirmar mi cuenta
                                    </a>
                                </div>

                                <p style='
                                    color: #777;
                                    font-size: 14px;
                                    line-height: 1.6;
                                '>
                                    Si no realizaste este registro, puedes ignorar este mensaje.
                                    No será necesario realizar ninguna acción adicional.
                                </p>

                                <p style='
                                    color: #777;
                                    font-size: 14px;
                                    margin-top: 30px;
                                '>
                                    Atentamente,<br>
                                    <strong>Administración del Sistema</strong><br>
                                    Gobierno de la Alcaldía Tláhuac
                                </p>

                            </div>

                            <!-- Footer -->
                            <div style='
                                background-color: #f5f5f5;
                                padding: 20px;
                                text-align: center;
                                font-size: 12px;
                                color: #888;
                            '>
                                Este es un correo automático, por favor no respondas a este mensaje.
                            </div>

                        </div>

                    </div>";


                    try
                    {
                        await _emailSender.SendEmailAsync(
                            Input.Email,
                            "Confirma tu cuenta",
                            mensaje);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error enviando correo");

                        ModelState.AddModelError(string.Empty,
                            "Usuario creado, pero no se pudo enviar el correo. Intenta reenviar la confirmación.");
                    }


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"No se puede crear una instancia de'{nameof(ApplicationUser)}'. " +
                    $"Asegúrese de que '{nameof(ApplicationUser)}' no sea una clase abstracta y tenga un constructor sin parámetros, o bien" +
                    $"sobrescriba la página de registro en Register");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("La interfaz de usuario predeterminada requiere una tienda de usuarios con soporte por correo electrónico..");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
