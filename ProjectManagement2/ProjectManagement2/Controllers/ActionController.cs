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
    public class ActionController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        //[HttpGet]
        //public ActionResult Approve(int? id)
        //{

        //    Report report = db.Reports.Find(id);
        //    return View(report);
        //}


        [HttpGet]
        public ActionResult Reject(int id, int userID, DateTime date)
        {


            Opinion opinion = new Opinion()
            {
                ReportID = id,
                MemberID = userID,
                DateCreated = date,
                Status = "not acknowledge"

            };

            //Report report = db.Reports.Find(id);
            // Status depend on previous status
            //report.Status = "Rejected";

            //if (ModelState.IsValid)
            //{
            //    db.Opinions.Add(opinion);
            //db.Entry(report).State = EntityState.Modified;
            //db.SaveChanges();
            //return RedirectToAction("Index");
            //return RedirectToAction("Create", new { id = id, userID = userID, actionName = "Reject", date = date });
            //}

            //return View("Details", "Report", new { id = id });
            return View(opinion);
        }

        [HttpPost]
        public ActionResult Reject(Opinion op)
        {
            
            Report report = db.Reports.Find(op.ReportID);
            // Status depend on previous status
            report.Status = "Drafted";

            if (ModelState.IsValid)
            {
                db.Opinions.Add(op);
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return RedirectToAction("Create", new { id = id, userID = userID, actionName = "Reject", date = date });
            }

            return RedirectToAction("Index", "Project", new { id = op.Report.ProjectID, reportID = op.ReportID });
        }
        // GET: Action
        public ActionResult Index()
        {
            var actions = db.Actions.Include(a => a.Member).Include(a => a.Report);
            return View(actions.ToList());
        }

        // GET: Action/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        // GET: Action/Create

        public ActionResult Activity(int id, int userID, string actionName, DateTime? date)
        {
            Action action = new Action()
            {
                ReportID = id,
                MemberID = userID,
                ActionName = actionName,
                ActionDate = date
            };

            Report report = db.Reports.Find(id);
            if (actionName.Equals("Rejected") && (report.Status.Equals("Reviewed") || report.Status.Equals("Submitted")))
            {
                report.Status = "Drafted";
            }
            else if (actionName.Equals("Approved") && report.Status.Equals("Reviewed"))
            {
                report.Status = "Approved";
            }
            else if (actionName.Equals("Reviewed") && report.Status.Equals("Submitted"))
            {
                report.Status = "Reviewed";
            }
            else if( actionName.Equals("Submitted") && report.Status.Equals("Drafted"))
            {
                report.Status = "Submitted";
            }
            else
            {
                report.Status = "Drafted";
            }

            if (ModelState.IsValid)
            {
                db.Actions.Add(action);
                db.Entry(report).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details", "Report", new { id = id, pid = report.ProjectID});
            }
            //ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition");
            //ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName");
            return RedirectToAction("Index");
        }

        //// POST: Action/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ActionID,ActionName,MemberID,ActionDate,ReportID")] Action action)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Actions.Add(action);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", action.MemberID);
        //    ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", action.ReportID);
        //    return View(action);
        //}

        // GET: Action/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", action.MemberID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", action.ReportID);
            return View(action);
        }

        // POST: Action/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActionID,ActionName,MemberID,ActionDate,ReportID")] Action action)
        {
            if (ModelState.IsValid)
            {
                db.Entry(action).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "MemberPosition", action.MemberID);
            ViewBag.ReportID = new SelectList(db.Reports, "ReportID", "ReportName", action.ReportID);
            return View(action);
        }

        // GET: Action/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        // POST: Action/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Action action = db.Actions.Find(id);
            db.Actions.Remove(action);
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
