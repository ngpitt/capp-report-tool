
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
    using System.Collections.Generic;
    
public partial class CAPPReport
{

    public CAPPReport()
    {

        this.Requirements = new HashSet<Requirement>();

        this.RequirementSets = new HashSet<RequirementSet>();

    }


    public int ReportID { get; set; }

    public string Username { get; set; }

    public string Name { get; set; }



    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual ICollection<Requirement> Requirements { get; set; }

    public virtual ICollection<RequirementSet> RequirementSets { get; set; }

}

}
