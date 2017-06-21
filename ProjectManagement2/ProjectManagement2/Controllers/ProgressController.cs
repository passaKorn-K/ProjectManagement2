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
    public class ProgressController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Progress
        public ActionResult Index()
        {
            var progresses = db.Progresses.Include(p => p.Project).Include(p => p.Report);
            return View(progresses.ToList());
        }

        // GET: Progress/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = db.Progresses.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            return View(progress);
        }

        // GET: Progress/Create
        public ActionResult Create(int rid, int pid)
        {
            Progress progress = new Progress()
            {
                ReportID = rid,
                ProjectID = pid
            };

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName");
            return View(progress);
        }

        // POST: Progress/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgressID,ProgressDetail,ProjectID,ReportID")] Progress progress)
        {
            if (ModelState.IsValid)
            {
                db.Progresses.Add(progress);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "Report", new { id = progress.ReportID });

            }

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", progress.ProjectID);
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", progress.ReportID);
            //return View(progress);
            return RedirectToAction("Details", "Report");
        }

        // GET: Progress/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = db.Progresses.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", progress.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", progress.ReportID);
            return View(progress);
        }

        // POST: Progress/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgressID,ProgressDetail,ProjectID,ReportID")] Progress progress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(progress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", progress.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", progress.ReportID);
            return View(progress);
        }

        // GET: Progress/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = db.Progresses.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            return View(progress);
        }

        // POST: Progress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Progress progress = db.Progresses.Find(id);
            db.Progresses.Remove(progress);
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
