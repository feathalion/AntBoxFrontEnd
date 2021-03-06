﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AntBoxFrontEnd.Infrastructure;
using AntBoxFrontEnd.Models;
using AntBoxFrontEnd.Services.Customer;
using AntBoxFrontEnd.Services.Login;
using AntBoxFrontEnd.Services;

namespace AntBoxFrontEnd.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AntBoxUpdateAccountAjax(string name, string lastname, string lastname2, string mail, string phone, string cellphone, string password)
        {
            var cus = new AntBoxFrontEnd.Services.Customer.CustomerUpdateOptions
            {
                Name=name,
                LastName=lastname,
                Lastname2=lastname2,
                Email = mail,
                Phone = phone,
                Mobile_phone = cellphone,
                Password = password
            };

            var ser = new CustomerServices(ServiceConfiguration.GetApiKey());

            string tempid = ((CustomerResponse)Session["customer"]).Id;
            var res = ser.UpdateCustomer(cus, tempid);
            if (res)
            {

                CustomerServices cs = new CustomerServices(ServiceConfiguration.GetApiKey());

                CustomerResponse customer = cs.SearchCustomer(tempid);

                Session["customer"] = customer;

                return RedirectToAction("Cuenta", "Customer");

            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //AddErrors(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AntBoxRegisterAjax(string email, string name, string lastname, string password, string username, string verifoption, string phone)
        {
            var cus = new AntBoxFrontEnd.Services.Customer.CustomerRequestOptions
            {
                Email = email,
                Name = name,
                LastName = lastname,
                //Lastname2 = "Prueba Apellido materno4",
                Mobilephone = "",
                Password = password,
                Username=username,
                Status=true,
                Phone = phone
            };

            var ser = new CustomerServices(ServiceConfiguration.GetApiKey());

            var res = ser.CreateCustomer(cus);
            if (res)
            {
                var usr = new AntBoxFrontEnd.Services.Login.LoginCreateOptions
                {
                    Email = email,
                    Password = password
                };

                LoginService ls = new LoginService(ServiceConfiguration.GetApiKey());

                string id = ls.HovaLogin(usr);

                CustomerServices cs = new CustomerServices(ServiceConfiguration.GetApiKey());

                CustomerResponse customer = cs.SearchCustomer(id);

                Session["customer"] = customer;

                return PartialView("_LoginPartial");
            }

            

            return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            //AddErrors(result);
        }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult AntBoxLoginAjax(string username, string password)
    {
        //if (ModelState.IsValid)w
        //{
        var usr = new AntBoxFrontEnd.Services.Login.LoginCreateOptions
        {
            Email = username,
            Password = password
        };
            
        LoginService ls = new LoginService(ServiceConfiguration.GetApiKey());

        string id = ls.HovaLogin(usr);

        CustomerServices cs = new CustomerServices(ServiceConfiguration.GetApiKey());

        CustomerResponse customer = cs.SearchCustomer(id);

        if (customer != null)
        {
            Session["customer"] = customer;
            return PartialView("_LoginPartial");
            //return Json(new { success = true, user = customer }, JsonRequestBehavior.AllowGet);
        }
        else
        {
            return Json(new { success = false, user = customer }, JsonRequestBehavior.AllowGet);
        }
        //}
        //return RedirectToAction("Index", "Home");
    }
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AntBoxLogin(AntBoxRegisterViewModel model)
    {
        //if (ModelState.IsValid)
        //{
        var usr = new AntBoxFrontEnd.Services.Login.LoginCreateOptions
        {
            Email = model.Email,
            Password = model.Password
        };

        LoginService ls = new LoginService(ServiceConfiguration.GetApiKey());

        string id = ls.HovaLogin(usr);

        CustomerServices cs = new CustomerServices(ServiceConfiguration.GetApiKey());

        CustomerResponse customer = cs.SearchCustomer(id);

        Session["customer"] = customer;

        return RedirectToAction("Precios", "Home");
        //}
        //return RedirectToAction("Index", "Home");
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AntBoxLogout()
    {
        Session["customer"] = null;

        return RedirectToAction("Index", "Home");
    }
    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> AntBoxRegister(AntBoxRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //var result = await UserManager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
            //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //    // Send an email with this link
            //    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            var cus = new AntBoxFrontEnd.Services.Customer.CustomerRequestOptions
            {
                Email = model.Email,
                Name = model.Name,
                LastName = model.LastName,
                //Lastname2 = "Prueba Apellido materno4",
                Mobilephone = "",
                Password = model.Password
            };

            var ser = new CustomerServices(ServiceConfiguration.GetApiKey());

            var res = ser.CreateCustomer(cus);
            if (res)
            {
                var usr = new AntBoxFrontEnd.Services.Login.LoginCreateOptions
                {
                    Email = model.Email,
                    Password = model.Password
                };

                LoginService ls = new LoginService(ServiceConfiguration.GetApiKey());

                string id = ls.HovaLogin(usr);

                CustomerServices cs = new CustomerServices(ServiceConfiguration.GetApiKey());

                CustomerResponse customer = cs.SearchCustomer(id);

                Session["customer"] = customer;

                return RedirectToAction("Precios", "Home");

            }


            //AddErrors(result);
        }
        return RedirectToAction("Index", "Home");
        // If we got this far, something failed, redisplay form
        //return View(model);
    }

    public AccountController()
    {
    }

    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    public ApplicationSignInManager SignInManager
    {
        get
        {
            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }
        private set
        {
            _signInManager = value;
        }
    }

    public ApplicationUserManager UserManager
    {
        get
        {
            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
        private set
        {
            _userManager = value;
        }
    }

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string email, string password, bool isPersistent)
    {
        string id = string.Empty;
        if (new UserManager().IsValid(email, password, ref id))
        {
            var customer = new UserManager().GetCustomer(id);


            var ident = new ClaimsIdentity(
                            new[] { 
                                    // adding following 2 claim just for supporting default antiforgery provider
                                    new Claim(ClaimTypes.NameIdentifier, customer.Name),
                                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                                    "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                                    new Claim(ClaimTypes.Name,customer.Name),
                            }, DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties { IsPersistent = isPersistent }, ident);

            return RedirectToAction("Index"); // auth succeed 
        }

        ModelState.AddModelError("", "invalid username or password");
        return View();
    }


    ////
    //// POST: /Account/Login
    //[HttpPost]
    //[AllowAnonymous]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(model);
    //    }

    //    // This doesn't count login failures towards account lockout
    //    // To enable password failures to trigger account lockout, change to shouldLockout: true
    //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
    //    switch (result)
    //    {
    //        case SignInStatus.Success:
    //            return RedirectToLocal(returnUrl);
    //        case SignInStatus.LockedOut:
    //            return View("Lockout");
    //        case SignInStatus.RequiresVerification:
    //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
    //        case SignInStatus.Failure:
    //        default:
    //            ModelState.AddModelError("", "Invalid login attempt.");
    //            return View(model);
    //    }
    //}

    //
    // GET: /Account/VerifyCode
    [AllowAnonymous]
    public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
    {
        // Require that the user has already logged in via username/password or external login
        if (!await SignInManager.HasBeenVerifiedAsync())
        {
            return View("Error");
        }
        return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /Account/VerifyCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // The following code protects for brute force attacks against the two factor codes. 
        // If a user enters incorrect codes for a specified amount of time then the user account 
        // will be locked out for a specified amount of time. 
        // You can configure the account lockout settings in IdentityConfig
        var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
        switch (result)
        {
            case SignInStatus.Success:
                return RedirectToLocal(model.ReturnUrl);
            case SignInStatus.LockedOut:
                return View("Lockout");
            case SignInStatus.Failure:
            default:
                ModelState.AddModelError("", "Invalid code.");
                return View(model);
        }
    }

    //
    // GET: /Account/Register
    [AllowAnonymous]
    public ActionResult Register()
    {
        return View();
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }



    //
    // GET: /Account/ConfirmEmail
    [AllowAnonymous]
    public async Task<ActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return View("Error");
        }
        var result = await UserManager.ConfirmEmailAsync(userId, code);
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    //
    // GET: /Account/ForgotPassword
    [AllowAnonymous]
    public ActionResult ForgotPassword()
    {
        return View();
    }

    //
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    //
    // GET: /Account/ForgotPasswordConfirmation
    [AllowAnonymous]
    public ActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    //
    // GET: /Account/ResetPassword
    [AllowAnonymous]
    public ActionResult ResetPassword(string code)
    {
        return code == null ? View("Error") : View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await UserManager.FindByNameAsync(model.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }
        var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }
        AddErrors(result);
        return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [AllowAnonymous]
    public ActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    //
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult ExternalLogin(string provider, string returnUrl)
    {
        // Request a redirect to the external login provider
        return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    }

    //
    // GET: /Account/SendCode
    [AllowAnonymous]
    public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
    {
        var userId = await SignInManager.GetVerifiedUserIdAsync();
        if (userId == null)
        {
            return View("Error");
        }
        var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /Account/SendCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SendCode(SendCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        // Generate the token and send it
        if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        {
            return View("Error");
        }
        return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
    }

    //
    // GET: /Account/ExternalLoginCallback
    [AllowAnonymous]
    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    {
        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            return RedirectToAction("Login");
        }

        // Sign in the user with this external login provider if the user already has a login
        var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        switch (result)
        {
            case SignInStatus.Success:
                return RedirectToLocal(returnUrl);
            case SignInStatus.LockedOut:
                return View("Lockout");
            case SignInStatus.RequiresVerification:
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            case SignInStatus.Failure:
            default:
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        }
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Manage");
        }

        if (ModelState.IsValid)
        {
            // Get the information about the user from the external login provider
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl);
                }
            }
            AddErrors(result);
        }

        ViewBag.ReturnUrl = returnUrl;
        return View(model);
    }

    //
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        return RedirectToAction("Index", "Home");
    }

    //
    // GET: /Account/ExternalLoginFailure
    [AllowAnonymous]
    public ActionResult ExternalLoginFailure()
    {
        return View();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            if (_signInManager != null)
            {
                _signInManager.Dispose();
                _signInManager = null;
            }
        }

        base.Dispose(disposing);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult ResetPasswordV(string email)
    {
        try
        {
            var rs = new ResetCreateOptions
            {
                Email = email,
            };

            Session["email"] = email;

            var ser = new LoginService(ServiceConfiguration.GetApiKey());
            var res = ser.ResetPassword(rs);
            return Json(new { success = res }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            LogManager.Write(ex.Message, LogManager.Error);
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult RestorePassword(string code, string newpassword)
    {
        try
        {
            var rs = new RestoreCreateOptions
            {
                Email = Session["email"].ToString(),
                Code = code,
                Newpassword = newpassword
            };

            var ser = new LoginService(ServiceConfiguration.GetApiKey());
            var res = ser.RestorePassword(rs);
            return Json(new { success = res }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            LogManager.Write(ex.Message, LogManager.Error);
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }

    #region Helpers
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

    private IAuthenticationManager AuthenticationManager
    {
        get
        {
            return HttpContext.GetOwinContext().Authentication;
        }
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }

    internal class ChallengeResult : HttpUnauthorizedResult
    {
        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
            {
                properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }
    }
    #endregion
}
}