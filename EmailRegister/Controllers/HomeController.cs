using EmailRegister.MailServices;
using EmailRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmailRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;
        private readonly SignInManager<IdentityUser> _signInManager;


        public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager, IMailService mailService, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _mailService = mailService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result= await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var codeToken=await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackURL = Url.Action("ConfirmEmail", "Home", new {userId= user.Id,code=codeToken},protocol:Request.Scheme);
                var mailMessage = $"Please confirm your account by <br/> <a href='{callBackURL}'> clicking here</a>";
                await _mailService.SendMailAsync(model.Email, "Confirm your mail", mailMessage);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index");
            }
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Kullanýcý gecersiz");
            }
            var result= await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "Index" : "Error");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user= await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                await Console.Out.WriteLineAsync("Kullanýcý adý veya sifre hatalý");
                return View(model);
            }
            var chechPassword = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!chechPassword.Succeeded)
            {
                await Console.Out.WriteLineAsync("Kullaýcý adý veya sifre hatalý");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
