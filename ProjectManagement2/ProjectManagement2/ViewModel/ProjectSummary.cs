using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagement2;

namespace ProjectManagement2.ViewModel
{
    public class ProjectSummary
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Opinion> Opinions { get; set; }
        //public IEnumerable<Action> Actions { get; set; }
    }
}