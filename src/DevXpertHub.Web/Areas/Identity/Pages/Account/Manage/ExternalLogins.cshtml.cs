// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevXpertHub.Web.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;

        public ExternalLoginsModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
        }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     Esta API oferece suporte à infraestrutura padrão da interface do usuário do ASP.NET Core Identity e não se destina a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

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

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
            }

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                StatusMessage = "O login externo não foi removido.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "O login externo foi removido.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Limpa o cookie externo existente para garantir um processo de login limpo
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Solicita um redirecionamento para o provedor de login externo para vincular um login ao usuário atual
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                throw new InvalidOperationException($"Ocorreu um erro inesperado ao carregar as informações de login externo.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage = "O login externo não foi adicionado. Logins externos só podem ser associados a uma conta.";
                return RedirectToPage();
            }

            // Limpa o cookie externo existente para garantir um processo de login limpo
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "O login externo foi adicionado.";
            return RedirectToPage();
        }
    }
}
