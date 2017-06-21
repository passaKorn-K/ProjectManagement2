using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagement2;

namespace ProjectManagement2.Controllers
{
    public class ResultController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Result
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Project).Include(r => r.Report);
            return View(results.ToList());
        }

        // GET: Result/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Result/Create
        public ActionResult Create(int rid, int pid)
        {
            Result result = new Result()
            {
                ReportID = rid,
                ProjectID = pid
            };

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName");
            return View(result);
        }

        // POST: Result/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResultID,ResultItem,ProjectID,ReportID")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Details", "Report", new { id = result.ReportID });
            }

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", result.ProjectID);
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", result.ReportID);
            //return View(result);

            return RedirectToAction("Details", "Report");
        }

        // GET: Result/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", result.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", result.ReportID);
            return View(result);
        }

        // POST: Result/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResultID,ResultItem,ProjectID,ReportID")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", result.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", result.ReportID);
            return View(result);
        }

        // GET: Result/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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
