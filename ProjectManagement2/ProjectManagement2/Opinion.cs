//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectManagement2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Opinion
    {
        public int OpinionID { get; set; }
        public int ReportID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Opinion1 { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Report Report { get; set; }
    }
}
