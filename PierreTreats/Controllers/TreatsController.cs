using PierreTreats.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PierreTreats.Controllers
{
    public class TreatsController : Controller
    {
      private readonly PierreTreatsContext _db;

      public TreatsController(PierreTreatsContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        List<Treat> model = _db.Treats.ToList();
        return View(model);
      }

      public ActionResult Details(int id)
      {
        var thisTreat = _db.Treats
          .Include(Treat => Treat.Flavors)
          .ThenInclude(join => join.Flavor)
          .FirstOrDefault(Treat => Treat.TreatId == id);
        return View(thisTreat);
      }

      public ActionResult Create()
      {
        return View();
      }

      [HttpPost]
      public ActionResult Create(Treat Treat)
      {
        _db.Treats.Add(Treat);
        _db.SaveChanges();
        return RedirectToAction("Create", "Flavors", new { id = Treat.TreatId });
      }

      public ActionResult Edit(int id)
      {
        var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
        ViewBag.RecipeId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(thisTreat);
      }

      [HttpPost]
      public ActionResult Edit(Treat Treat, int FlavorId)
      {
        if (FlavorId != 0)
        {
          _db.TreatFlavor.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = Treat.TreatId });
        }
        _db.Entry(Treat).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      public ActionResult AddFlavor(int id)
      {
        var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(thisTreat);
      }

      [HttpPost]
      public ActionResult AddFlavor(Treat Treat, int FlavorId)
      {
        if (FlavorId != 0)
        {
          _db.TreatFlavor.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = Treat.TreatId });
        }
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = Treat.TreatId });
      }

      public ActionResult Delete(int id)
      {
        var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
        return View(thisTreat);
      }

      [HttpPost, ActionName("Delete")]
      public ActionResult DeleteConfirmed(int id)
      {
        var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
        _db.Treats.Remove(thisTreat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      [HttpPost]
      public ActionResult DeleteFlavor(int joinId)
      {
        var joinEntry = _db.TreatFlavor.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
        _db.TreatFlavor.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
}