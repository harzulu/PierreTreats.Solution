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
  }
}