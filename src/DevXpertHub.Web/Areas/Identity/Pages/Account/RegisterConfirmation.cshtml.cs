// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DevXpertHub.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
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
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o e-mail '{email}'.");
            }

            Email = email;
            // Assim que você adicionar um remetente de e-mail real, remova este código que permite confirmar a conta
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
