using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private readonly InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName," +
                "LastName,EmailAddress,DateOfBirth,CarYear,CarMake," +
                "CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                //Start with Base Rate
                insuree.Quote = 50.00;
                //Create Variables and convert Date of Birth to age
                var today = DateTime.Today;
                var age = today.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth.Date > today.AddYears(-age)) age--;

                //DECISION 1
                //"Is age 18 or younger?"
                if (age <= 18)
                {
                    insuree.Quote += 100.00;
                }


                //DECISION 2
                //"Is age between 19 and 25?"
                if ((age - 19) * (age - 25) <= 0)
                {
                    insuree.Quote += 50.00;
                }


                //Decision 3
                //"Is persons Age Over 25?"
                if (age > 25)
                {
                    insuree.Quote += 25.00;

                }


                // DECISION 4 AND 5
                //"Is Car Year Outside of range 2000 to 2015?"

                if ((insuree.CarYear - 2000) * (insuree.CarYear - 2015) >= 0)
                {
                    insuree.Quote += 25.00;

                }


                // DECISION 6
                //"Is Make Porsche?"

                if (insuree.CarMake == "porsche")
                {
                    insuree.Quote += 25.00;

                }


                // DECISION 7
                //"Is Model 911 Carrera?"

                if (insuree.CarModel == "911 carrera")
                {
                    insuree.Quote += 25.00;
                }


                // DECISION 8
                //"How Many Speeding Tickets?"

                if (insuree.SpeedingTickets > 0)
                {
                    decimal ticketsFee = insuree.SpeedingTickets * 10.00;
                    insuree.Quote += ticketsFee;
                }

                // DECISION 9
                //"Have you had a DUI?"

                if (insuree.DUI == true)
                {
                    insuree.Quote *= 1.25;
                }

                // DECISION 10
                //"Full Coverage?"


                if (insuree.CoverageType == true)
                {
                    insuree.Quote *= 1.50;
                }

                //// Auto Fill Quote JQuery testing 
                // ActionResult GetMP (string term)
                //{
                //    var result = from r in _db.GetMPDetails()
                //                 where r.Name.ToLower().Contains(term)
                //                 select r.Name;
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                //Adds the insuree object to the database
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        return View(insuree);
    }


    //EDIT, Delete records from Database
    // GET: Insuree/Edit
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Insuree insuree = db.Insurees.Find(id);
        if (insuree == null)
        {
            return HttpNotFound();
        }
        return View(insuree);
    }

    // POST: Insuree/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            db.Entry(insuree).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(insuree);
    }

    // GET: Insuree/Delete
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Insuree insuree = db.Insurees.Find(id);
        if (insuree == null)
        {
            return HttpNotFound();
        }
        return View(insuree);
    }

    // POST: Insuree/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Insuree insuree = db.Insurees.Find(id);
        db.Insurees.Remove(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
    }
}
    
