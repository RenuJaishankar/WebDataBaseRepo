using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDatabase.Models;

namespace WebDatabase.Controllers
{
    public class TrackController : Controller
    {
        Models.ICRUD _crud;

        public TrackController(Models.ICRUD crud)
        {
            _crud = crud;
        }

        // GET: TrackController
        public ActionResult Index()
        {
            var data = _crud.GetTracks(5, 20);
            return View(data);
        }      // List functionality Read many records

        // GET: TrackController/Details/5
        public ActionResult Details(int id)
        {
            Track t = _crud.FindById(id);
            if (t == null)
            {
                return View("NoSuchRecord");
            }
            return View(t);
        }   // Read a single record

        // the following group of methods come in pairs because they all modify the data in the database
        // first show/collect the data to be modified
        // then actually perform the action in the POST

        // GET: TrackController/Create
        public ActionResult Create()
        {
            return View();
        }        // create a record [get the data from the user] show a blank form


        // add the data to the database when the user
        // clicks on the create button to post the data

        // POST: TrackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }   // Create
        

        // GET: TrackController/Edit/5
        public ActionResult Edit(int id)
        {
            Track t = _crud.FindById(id);
            if (t == null)
            {
                return View("NoSuchRecord");
            }
            return View(t);
        }    // display the record to the user for updating

        // POST: TrackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Track t)
        {
            try
            {
                _crud.Update(id, t);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

               return View();
            }
        }  // update the record when the data is POSTED

        // GET: TrackController/Delete/5      // show the record to be deleted with confirmation
        public ActionResult Delete(int id)
        {
            Track t = _crud.FindById(id);
            if (t == null)
            {
                return View("NoSuchRecord");
            }
            return View(t);
        }

        // POST: TrackController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Track t)
        {
            try
            {
                _crud.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ErrorViewModel m = new ErrorViewModel();
                m.RequestId = ex.Message;
                
                return View("Error",m);
            }
        }  // actually delete it
    }
}
