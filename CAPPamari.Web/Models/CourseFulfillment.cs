
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
    
public partial class CourseFulfillment
{

    public CourseFulfillment()
    {

        this.Requirements = new HashSet<Requirement>();

    }


    public int CourseFulfillmentID { get; set; }

    public string DepartmentCode { get; set; }

    public string CourseNumber { get; set; }



    public virtual ICollection<Requirement> Requirements { get; set; }

}

}
