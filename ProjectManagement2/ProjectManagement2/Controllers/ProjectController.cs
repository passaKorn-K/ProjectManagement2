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
            ViewBag.UserID = uid;
            if (Session["UserID"] != null)
            {
                var viewModel = new ProjectSummary();
                viewModel.Projects = db.Projects
                                    .Include(i => i.Reports)
                                    //.Include(i => i.Members)
                                    .OrderBy(i => i.ProjectName);

                
                if (id != null)
                {
                    //Project must have member
                    Member member = db.Members.Where(a => a.ProjectID == id.Value).FirstOrDefault();
                    //var member = db.Members.Where(a => a.ProjectID == id.Value).ToList();
                    var mid = db.Members.Where(a => a.ProjectID == id.Value && a.UserID == uid).Select(a => a.MemberID).FirstOrDefault();


                    //foreach( var item in member)
                    //{
                    //    //Admin can add member
                    //    if (item == null && uid == 1)
                    //    {
                    //        return RedirectToAction("Create", "Member", new { pid = id });
                    //    }

                    //    //Non-Admin can't add member
                    //    else if (item == null && uid != 1)
                    //    {
                    //        return Content("Please Contact Admin to Assign Member.");
                    //    }

                    //    //Member Exist 
                    //    else if (item != null && item.MemberID == uid)
                    //    {
                    //        //return Content("Please Contact Admin to Create role.");
                    //        String position = db.Members.Where(a => a.ProjectID == id.Value && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    //        ViewBag.Position = position;


                    //    }
                    //    else
                    //    {
                    //        id = 0;
                    //        //String position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    //        ViewBag.Position = "null";
                    //        ViewBag.ProjectID = id.Value;
                    //    }
                    //}

                    if (member == null && uid == 1)
                    {
                        return RedirectToAction("Create", "Member", new { pid = id });
                    }

                    else if (member == null && uid != 1)
                    {
                        return Content("Please Contact Admin to Assign Member.");
                    }

                    else if (member != null && mid != 0)
                    {
                        

                    }
                    //else

                    String position = db.Members.Where(a => a.ProjectID == id.Value && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    if (position == null && uid == 1)
                    {
                        return RedirectToAction("Create", "Member", new { pid = id });

                    }
                    else if (position == null && uid != 1)
                    {
                        //ViewBag.ErrorMessage = "You don't have authorize to access this project";
                        //return View();
                        return Content("Please Contact Admin.");
                    }

                    Session["Position"] = position;
                    //ViewBag.Position = position;
                   // String position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
                    ViewBag.Position = position;
                    ViewBag.ProjectID = id.Value;

                    viewModel.Reports = viewModel.Projects.Where(i => i.ProjectID == id.Value).SingleOrDefault().Reports;
                    
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

            //var uid = Convert.ToInt32(Session["UserID"]);
            //string position = db.Members.Where(a => a.ProjectID == id && a.UserID == uid).Select(a => a.MemberPosition).FirstOrDefault();
            //ViewBag.Position = position;

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

            var report = db.Reports.Where(i => i.ProjectID == id).ToList();

            foreach( var item in report)
            {
                item.Opinions.ToList().ForEach(p => db.Opinions.Remove(p));
                item.Progresses.ToList().ForEach(p => db.Progresses.Remove(p));
                item.Results.ToList().ForEach(p => db.Results.Remove(p));
                item.Actions.ToList().ForEach(p => db.Actions.Remove(p));
            }
       
            project.Members.ToList().ForEach(p => db.Members.Remove(p));
            project.Progresses.ToList().ForEach(p => db.Progresses.Remove(p));
            project.Results.ToList().ForEach(p => db.Results.Remove(p));
            project.Tasks.ToList().ForEach(p => db.Tasks.Remove(p));
            project.Reports.ToList().ForEach(p => db.Reports.Remove(p));



            db.Entry(project).State = EntityState.Deleted;

            //db.Projects.Remove(project);
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
