using PierreTreats.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace PierreTreats.Controllers
{
  public class HomeController : Controller
  {

    private readonly PierreTreatsContext _db;

    public HomeController(PierreTreatsContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Index(string SearchTerm, string SearchOption)
    {
      if (SearchOption == "Treat")
      {
        var treats = _db.Treats;
        foreach (var treat in treats)
        {
          if (treat.Name == SearchTerm)
          {
            return RedirectToAction("Details", "Treats", new { id = treat.TreatId });
          }
        }
      }
      else if (SearchOption == "Flavor")
      {
        var flavors = _db.Flavors;
        foreach (var flavor in flavors)
        {
          if (flavor.Name == SearchTerm)
          {
            return RedirectToAction("Details", "Flavors", new { id = flavor.FlavorId });
          }
        }
      }
      ViewBag.value = false;
      return View();
    }
  }
}