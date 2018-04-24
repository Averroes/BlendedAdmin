using System;
using Microsoft.AspNetCore.Mvc;
using BlendedAdmin.DomainModel;
using BlendedAdmin.Models.Environments;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Environment = BlendedAdmin.DomainModel.Environments.Environment;
using BlendedAdmin.DomainModel.Users;
using BlendedAdmin.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BlendedAdmin.Services;
using BlendedAdmin.Models.Tenants;
using BlendedAdmin.DomainModel.Tenants;
using System.Transactions;
using System.Text;

namespace BlendedAdmin.Controllers
{
    [Authorize]
    public class TenantsController : Controller
    {
        private IDomainContext _domainContext;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;
        private ILogger<UsersController> _logger;
        private IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUrlService _urlService;

        public TenantsController(IDomainContext domainContext, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ILogger<UsersController> logger,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IUrlService urlService)
        {
            _domainContext = domainContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _urlService = urlService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("tenants/register")]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Route("tenants/register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var tenant = await _domainContext.Tenants.Get(model.Name);
            if (tenant != null)
            {
                ModelState.AddModelError("Name", "Name is already taken.");
                return View(model);
            }

            using (var scope = await _domainContext.BeginTransaction())
            {
                tenant = new Tenant();
                tenant.Id = model.Name;
                _domainContext.Tenants.Save(tenant);

                ApplicationUser user = new ApplicationUser();
                user.UserName = model.Email;
                user.NormalizedUserName = model.Email.ToUpper();
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                user.SecurityStamp = Guid.NewGuid().ToString();
                _domainContext.Users.Save(user, model.Name);
                await _domainContext.SaveAsync();
                scope.Commit();
            }

            StringBuilder body = new StringBuilder();
            body.Append("Thank you for registration.<br/><br/>");
            body.AppendFormat("Your url is <a href=\"{0}\">{0}</a>.<br/>", _urlService.GetUrlWithTenant(model.Name));
            await _emailService.SendEmailAsync(
                model.Email,
                "Thank you for registration",
                body.ToString());

            return this.RedirectToAction("RegistrationConfirmation", new { tenant = model.Name});
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("tenants/registrationconfirmation")]
        public IActionResult RegistrationConfirmation()
        {
            return View();
        }
    }
}