﻿
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
    public class MemberController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Member
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Project).Include(m => m.User);
            return View(members.ToList());
        }

        // GET: Member/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Member/Create
        public ActionResult Create(int pid)
        {
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");

            Member obj = db.Members.Where(a => a.ProjectID == pid).FirstOrDefault();
            if(obj == null)
            {
                ViewBag.Message = "Please Assign Project to Project Manager";

            }

            Member member = new Member()
            {
                ProjectID = pid,
                //ReportID = rid
            };

            return View(member);
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,MemberPosition,Wages,WorkingHour,TotalWages,ProjectID,UserID")] Member member)
        {
            if (ModelState.IsValid)
            {
                //Report report = db.Reports.Where(a => a.ProjectID == member.ProjectID).SingleOrDefault();
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = member.ProjectID });
            }

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", member.ProjectID);
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", member.UserID);
            return View(member);
        }

        public ActionResult CreateFirst(int pid)
        {
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");

            Member obj = db.Members.Where(a => a.ProjectID == pid).FirstOrDefault();
            if (obj == null)
            {
                ViewBag.Message = "Please Assign Project to Project Manager";

            }


            Member member = new Member()
            {
                ProjectID = pid,
                //ReportID = rid
            };

            return View(member);
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFirst([Bind(Include = "MemberID,MemberPosition,Wages,WorkingHour,TotalWages,ProjectID,UserID")] Member member)
        {
            if (ModelState.IsValid)
            {
                //Report report = db.Reports.Where(a => a.ProjectID == member.ProjectID).SingleOrDefault();
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index", "Project", new { id = member.ProjectID });
            }

            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", member.ProjectID);
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", member.UserID);
            return View(member);
        }
        // GET: Member/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", member.ProjectID);
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", member.UserID);
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,MemberPosition,Wages,WorkingHour,TotalWages,ProjectID,UserID")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Project", new { id = member.ProjectID });
            }
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", member.ProjectID);
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", member.UserID);
            return RedirectToAction("Details", "Project", new { id = member.ProjectID });
        }

        // GET: Member/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            int pid = member.ProjectID;
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Details", "Project", new { id = pid });
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
