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
    public class OpinionController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Opinion
        public ActionResult Index()
        {
            var opinions = db.Opinions.Include(o => o.Member).Include(o => o.Report);
            return View(opinions.ToList());
        }

        // GET: Opinion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opinion opinion = db.Opinions.Find(id);
            if (opinion == null)
            {
                return HttpNotFound();
            }
            return View(opinion);
        }

        public ActionResult Acknowledge(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opinion opinion = db.Opinions.Find(id);
            if (opinion == null)
            {
                return HttpNotFound();
            }
            opinion.Status = "acknowledge";

            db.Entry(opinion).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = opinion.Report.ProjectID, reportID = opinion.ReportID });

        }
        // GET: Opinion/Create
        public ActionResult Create(int mid, int rid, DateTime date)
        {
            //ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition");
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName");
            Opinion op = new Opinion()
            {
                MemberID = mid,
                ReportID = rid,
                DateCreated = date,
                Status = "not acknowledge"
                
            };
            return View(op);
        }

        // POST: Opinion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpinionID,ReportID,MemberID,DateCreated,Opinion1,Status")] Opinion opinion)
        {

            if (ModelState.IsValid)
            {
                db.Opinions.Add(opinion);
                db.SaveChanges();

                int? pid = db.Reports.Where(i => i.ReportID == opinion.ReportID).Select(i => i.ProjectID).FirstOrDefault();

                return RedirectToAction("Index", "Project", new { id = pid, reportID = opinion.ReportID });
            }

            //ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", opinion.MemberID);
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", opinion.ReportID);
            return View(opinion);
        }

        // GET: Opinion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opinion opinion = db.Opinions.Find(id);
            if (opinion == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", opinion.MemberID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", opinion.ReportID);
            return View(opinion);
        }

        // POST: Opinion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpinionID,ReportID,MemberID,DateCreated,Opinion1")] Opinion opinion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opinion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", opinion.MemberID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", opinion.ReportID);
            return View(opinion);
        }

        // GET: Opinion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opinion opinion = db.Opinions.Find(id);
            if (opinion == null)
            {
                return HttpNotFound();
            }
            return View(opinion);
        }

        // POST: Opinion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opinion opinion = db.Opinions.Find(id);
            int? reportID = opinion.ReportID;
            int? projectID = opinion.Report.ProjectID;
            db.Opinions.Remove(opinion);
            db.SaveChanges();
            return RedirectToAction("Index", "Project", new { id = projectID, reportID = reportID });
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
