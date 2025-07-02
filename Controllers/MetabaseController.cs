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
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Authorize([FromForm] MetabaseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = tokenProvider.Create(model.UserName);
            model.CreatedToken = token;
        }
        return RedirectToActionPermanent("Index", model);
    }
}