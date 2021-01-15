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
    public class FlavorsController : Controller
    {
      private readonly PierreTreatsContext _db;

      public FlavorsController(PierreTreatsContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        List<Flavor> model = _db.Flavors.ToList();
        return View(model);
      }

      public ActionResult Details(int id)
      {
        var thisFlavor = _db.Flavors
          .Include(flavor => flavor.Treats)
          .ThenInclude(join => join.Treat)
          .FirstOrDefault(flavor => flavor.FlavorId == id);
        return View(thisFlavor);
      }

      public ActionResult Create()
      {
        return View();
      }

      [HttpPost]
      public ActionResult Create(Flavor flavor)
      {
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }

      public ActionResult Edit(int id)
      {
        var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
        ViewBag.RecipeId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(thisFlavor);
      }

      [HttpPost]
      public ActionResult Edit(Flavor flavor, int TreatId)
      {
        if (TreatId != 0)
        {
          _db.TreatFlavor.Add(new TreatFlavor() { TreatId = TreatId, FlavorId = flavor.FlavorId });
        }
        _db.Entry(flavor).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      public ActionResult AddTreat(int id)
      {
        var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(thisFlavor);
      }

      [HttpPost]
      public ActionResult AddTreat(Flavor flavor, int TreatId)
      {
        if (TreatId != 0)
        {
          _db.TreatFlavor.Add(new TreatFlavor() { TreatId = TreatId, FlavorId = flavor.FlavorId });
        }
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }

      public ActionResult Delete(int id)
      {
        var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
        return View(thisFlavor);
      }

      [HttpPost, ActionName("Delete")]
      public ActionResult DeleteConfirmed(int id)
      {
        var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
        _db.Flavors.Remove(thisFlavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
}