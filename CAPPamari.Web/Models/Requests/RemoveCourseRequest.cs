﻿namespace CAPPamari.Web.Models.Requests
{
    public class RemoveCourseRequest
    {
        #region Properties

        public string Username { get; set; }
        public CourseModel CourseToRemove { get; set; }

        #endregion
    }
}