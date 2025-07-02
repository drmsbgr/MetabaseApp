using System.IdentityModel.Tokens.Jwt;
using MetabaseApp.Infrastructure;
using MetabaseApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetabaseApp.Controllers;

public class MetabaseController : Controller
{
    private readonly TokenProvider tokenProvider;

    public MetabaseController(TokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }
    public IActionResult Index(MetabaseViewModel model)
    {
        string? loadedToken = HttpContext.Session.GetString("token");
        if (!string.IsNullOrEmpty(loadedToken))
        {
            if (tokenProvider.ValidateJwtToken(loadedToken!, out _))
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Oturumunuzdaki JWT geçersiz veya süresi dolmuş.");
                HttpContext.Session.SetString("token", "");
            }
        }

        return View(model);
    }

    public IActionResult Dashboard()
    {
        var model = new DashboardViewModel { Token = HttpContext.Session.GetString("token") };

        if (string.IsNullOrEmpty(model.Token))
            return RedirectToActionPermanent("Index");

        if (!tokenProvider.ValidateJwtToken(model.Token, out var securityToken))
        {
            ModelState.AddModelError("", "Oturumunuzdaki JWT geçersiz veya süresi dolmuş.");
            HttpContext.Session.SetString("token", "");
            return RedirectToActionPermanent("Index");
        }

        var jwtSec = tokenProvider.ReadToken(model.Token);
        model.UserName = jwtSec.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Authorize([FromForm] MetabaseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = tokenProvider.Create(model.UserName!);
            model.Token = token;
            HttpContext.Session.SetString("token", token);
            return RedirectToAction("Dashboard");
        }
        return View("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UseToken([FromForm] MetabaseViewModel model)
    {
        var targetToken = model.Token;

        if (!tokenProvider.ValidateJwtToken(targetToken!, out _))
        {
            ModelState.AddModelError("", "Geçersiz veya süresi dolmuş JWT token girdiniz.");
            model.Token = string.Empty;
            HttpContext.Session.SetString("token", string.Empty);
            return View("Index", model);
        }

        HttpContext.Session.SetString("token", model.Token!);
        return RedirectToAction("Dashboard");
    }

    public IActionResult ClearSession()
    {
        HttpContext.Session.SetString("token", string.Empty);
        return RedirectToAction("Index");
    }
}