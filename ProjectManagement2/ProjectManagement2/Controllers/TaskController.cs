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
    public class TaskController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Task
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Project).Include(t => t.Report);
            return View(tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Task/Create
        public ActionResult Create(int rid, int pid)
        {
            Task task = new Task()
            {
                ReportID = rid,
                ProjectID = pid

            };
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName");
            return View(task);
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskID,TaskName,TaskStartDate,TaskEndDate,TaskType,ProjectID,ReportID")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Details", "Report", new { id = task.ReportID, pid = task.ProjectID });
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", task.ReportID);
            return View(task);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", task.ReportID);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskID,TaskName,TaskStartDate,TaskEndDate,TaskType,ProjectID,ReportID")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", task.ReportID);
            return View(task);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
