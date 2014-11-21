﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CAPPamari.Web.Helpers;
using CAPPamari.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAPPamari.Test
{
    [TestClass]
    public class CsvParserHelperTests
    {
        [TestMethod]
        public void CsvTest1()
        {
            //take a sample csv file and convert it to a string- this part works
            var reader = new StreamReader(File.OpenRead(@"testParser.csv"));
            string input = "";
            while (!reader.EndOfStream)
            {
                input = input + reader.ReadLine() + "\n";
            }
            reader.Close();

            //parse it
            IEnumerable<CourseModel> courses = CsvParserHelper.Parse(input);

            if (courses == null)
            {
                Debug.Print("courses was null");
            }

            // and spit out the info
            foreach (CourseModel course in courses)
            {
                Debug.Print("{0} {1}, {2} credits, {3}, {4}, PNC {5}, Comm {6}", course.DepartmentCode,
                    course.CourseNumber, course.Credits,
                    course.Semester, course.Grade, course.PassNoCredit, course.CommIntensive);
            }
        }
    }
}