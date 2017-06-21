using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagement2;
using ProjectManagement2.ViewModel;

namespace ProjectManagement2.Controllers
{
    public class ProjectController : Controller
    {
        private ReportManagementEntities db = new ReportManagementEntities();

        // GET: Project
        //public ActionResult Index()
        //{
        //    return View(db.Projects.ToList());
        //}
        public ActionResult Index(int? id, int? reportID)
        {
            var uid = Convert.ToInt32(Session["UserID"]);
            if (Session["UserID"] != null)
            {
                var viewModel = new ProjectSummary();
                viewModel.Projects = db.Projects
                                    .Include(i => i.Reports)
                                    //.Include(i => i.Members)
                                    .OrderBy(i => i.ProjectName);


                if (id != null)
                {
                    String position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    ViewBag.Position = position;
                    //String position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    //ViewBag.Position = position;
                    ViewBag.ProjectID = id.Value;
                    viewModel.Reports = viewModel.Projects.Where(i => i.ProjectID == id.Value).Single().Reports;
                }
                else
                {
                    id = 0;
                    //String position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    ViewBag.Position = "null";
                    ViewBag.ProjectID = id.Value;
                    //viewModel.Reports = viewModel.Projects.Where(i => i.ProjectID == id.Value).Single().Reports;
                }

                if (reportID != null)
                {
                    ViewBag.ReportID = reportID.Value;
                    var selectedReport = viewModel.Reports.Where(x => x.ReportID == reportID).Single();
                    db.Entry(selectedReport).Collection(x => x.Opinions).Load();
                    foreach (Opinion opinion in selectedReport.Opinions)
                    {
                        db.Entry(opinion).Reference(x => x.Member).Load();
                    }
                    viewModel.Opinions = selectedReport.Opinions;
                }
                else{
                    reportID = 0;
                    ViewBag.ReportID = reportID.Value;

                }
                //return View(db.Projects.ToList());
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectName,ProjectStatus,ProjectStartDate,ProjectEndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,ProjectStatus,ProjectStartDate,ProjectEndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
