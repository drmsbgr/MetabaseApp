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
                return RedirectToActionPermanent("Dashboard", new DashboardViewModel { Token = loadedToken });
            }
            else
            {
                ModelState.AddModelError("", "Oturumunuzdaki JWT geçersiz veya süresi dolmuş.");
                HttpContext.Session.SetString("token", "");
            }
        }

        return View(model);
    }

    public IActionResult Dashboard(DashboardViewModel model)
    {
        if (string.IsNullOrEmpty(model.Token))
            return RedirectToActionPermanent("Index");

        if (!tokenProvider.ValidateJwtToken(model.Token, out _))
        {
            ModelState.AddModelError("", "Oturumunuzdaki JWT geçersiz veya süresi dolmuş.");
            HttpContext.Session.SetString("token", "");
            return RedirectToActionPermanent("Index");
        }

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
        }
        return RedirectToActionPermanent("Index", model);
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

        if (ModelState.IsValid)
        {
            HttpContext.Session.SetString("token", model.Token!);
            return RedirectToActionPermanent("Dashboard", new DashboardViewModel { Token = model.Token });
        }

        return View("Index", model);
    }
}