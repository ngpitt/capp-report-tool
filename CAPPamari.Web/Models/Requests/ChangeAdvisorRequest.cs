﻿namespace CAPPamari.Web.Models.Requests
{
    public class ChangeAdvisorRequest
    {
        #region Properties

        public string Username { get; set; }
        public AdvisorModel Advisor { get; set; }

        #endregion
    }
}