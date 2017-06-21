using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagement2;
using PagedList;

namespace ProjectManagement2.Controllers
{
    public class ReportController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();
        //public enum eReportStatus { DRAFTED, REVIEWED, APPROVED };


        // GET: Report
        //public ActionResult Index()
        //{
        //    var reports = db.Reports.Include(r => r.Project);
        //    return View(reports.ToList());
        //}

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var reports = from s in db.Reports
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                reports = reports.Where(s => s.ReportName.Contains(searchString)
                || s.Project.ProjectName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    reports = reports.OrderByDescending(s => s.ReportName);
                    break;
                case "Status":
                    reports = reports.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    reports = reports.OrderByDescending(s => s.Status);
                    break;
                case "date_desc":
                    reports = reports.OrderByDescending(s => s.CreatedDate);
                    break;
                case "Date":
                    reports = reports.OrderBy(s => s.CreatedDate);
                    break;
                default:
                    reports = reports.OrderBy(s => s.ReportName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(reports.ToPagedList(pageNumber, pageSize));
        }

        // GET: Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Report report = db.Reports.Find(id);
            Report report = db.Reports.Where(m => m.ReportID == id).Include(i => i.Actions).SingleOrDefault();

            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Report/Create
        public ActionResult Create(int? id)
        {
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            Report report = new Report()
            {
                ProjectID = id,
                Status = "Drafted",
                CreatedDate = DateTime.Now
            };
            return View(report);
        }

        // POST: Report/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportID,ReportName,Status,ProjectID,CreatedDate")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", report.ProjectID);
            return View(report);
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", report.ProjectID);
            return View(report);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportID,ReportName,Status,ProjectID,CreatedDate")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", report.ProjectID);
            return View(report);
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
