﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class JustinEntities : DbContext
{
    public JustinEntities()
        : base("name=JustinEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<Advisor> Advisors { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<CAPPReport> CAPPReports { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<CourseFulfillment> CourseFulfillments { get; set; }

    public DbSet<Requirement> Requirements { get; set; }

    public DbSet<UserSession> UserSessions { get; set; }

    public DbSet<RequirementSet> RequirementSets { get; set; }

}

}

