//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAPPamari.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DepartmentRSR
    {
        public DepartmentRSR()
        {
            this.RequirementSets = new HashSet<RequirementSet>();
        }
    
        public int DepartmentRSRID { get; set; }
        public string DepartmentCode { get; set; }
        public int NumberOfCourses { get; set; }
    
        public virtual ICollection<RequirementSet> RequirementSets { get; set; }
    }
}
