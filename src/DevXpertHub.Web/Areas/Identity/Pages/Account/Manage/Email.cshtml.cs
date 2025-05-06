// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace DevXpertHub.Web.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
            ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Novo e-mail")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Confirme seu e-mail",
                    $"Por favor, confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                StatusMessage = "Link de confirmação para alterar o e-mail enviado. Por favor, verifique seu e-mail.";
                return RedirectToPage();
            }

            StatusMessage = "Seu e-mail não foi alterado.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirme seu e-mail",
                $"Por favor, confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

            StatusMessage = "E-mail de verificação enviado. Por favor, verifique seu e-mail.";
            return RedirectToPage();
        }
    }
}
