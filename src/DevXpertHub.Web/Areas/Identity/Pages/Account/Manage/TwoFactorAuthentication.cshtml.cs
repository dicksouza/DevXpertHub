// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevXpertHub.Web.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        public TwoFactorAuthenticationModel(
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<TwoFactorAuthenticationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public bool HasAuthenticator { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [BindProperty]
        public bool Is2faEnabled { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public bool IsMachineRemembered { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage = "O navegador atual foi esquecido. Quando você fizer login novamente neste navegador, será solicitado o código 2FA.";
            return RedirectToPage();
        }
    }
}
