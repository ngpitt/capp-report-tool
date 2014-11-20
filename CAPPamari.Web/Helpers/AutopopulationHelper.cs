using System;
using System.Collections.Generic;
using System.Linq;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    public static class AutopopulationHelper
    {
        public static int courseNumToInt(string courseNum)
        {
            if (courseNum.Contains('x'))
            {
                return Convert.ToInt32(courseNum[0]) * 1000;
            }
            else
            {
                return Convert.ToInt32(courseNum);
            }
        }

        //function to autopopulate the HASS requirement set
        public static void FillHass(RequirementSetModel hassReqSet, List<CourseModel> coursesTaken)
        {
            var humDepts = new List<string>
            {
                "IHSS",
                "ARTS",
                "LANG",
                "LITR",
                "COMM",
                "WRIT",
                "STSH",
                "PHIL"
            };
            var ssciDepts = new List<string>
            {
                "COGS",
                "STSS",
                "ECON",
                "PSYC",
                "IHSS",
            };

            //create sorted lists of humanities and ssci courses
            //sorted with highest code first
            var humCourses = new SortedDictionary<int, List<CourseModel>>();
            var ssciCourses = new SortedDictionary<int, List<CourseModel>>();

            foreach (CourseModel courseTaken in coursesTaken)
            {
                if (humDepts.Contains(courseTaken.DepartmentCode))
                {
                    int courseNum = courseNumToInt(courseTaken.CourseNumber);
                    if (humCourses.ContainsKey(courseNum))
                    {
                        humCourses[courseNum].Add(courseTaken);
                    }
                    else
                    {
                        var tempCourse = new List<CourseModel>
                        {
                            courseTaken,
                        };
                        humCourses[courseNum] = tempCourse;
                    }
                }
                else if (ssciDepts.Contains(courseTaken.DepartmentCode))
                {
                    int courseNum = courseNumToInt(courseTaken.CourseNumber);
                    if (ssciCourses.ContainsKey(courseNum))
                    {
                        ssciCourses[courseNum].Add(courseTaken);
                    }
                    else
                    {
                        var tempCourse = new List<CourseModel>
                        {
                            courseTaken,
                        };
                        ssciCourses[courseNum] = tempCourse;
                    }
                }
            }

            //alternate between adding hum and ssci to HASS 
            //until full (or run out of courses, caught below)
            while (!hassReqSet.IsFulfilled())
            {
                if (humCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = humCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        if (hassReqSet.CanApplyCourse(maxCourseList[0]))
                        {
                            hassReqSet.ApplyCourse(maxCourseList[0]);
                            maxCourseList[0].RequirementSetName = "HASS";
                            //remove the key from the list of remaining courses
                            humCourses.Remove(humCourses.Last().Key);
                        }
                    }
                    else
                    {
                        //apply the first course in the list
                        if (hassReqSet.CanApplyCourse(maxCourseList[0]))
                        {
                            hassReqSet.ApplyCourse(maxCourseList[0]);
                            maxCourseList[0].RequirementSetName = "HASS";
                            //remove the course from list of remaining courses
                            maxCourseList.RemoveAt(0);
                            humCourses[humCourses.Count - 1] = maxCourseList;
                        }
                    }
                }
                if (ssciCourses.Count > 0)
                {
                    //find the largest course number (last b/c ascending order)
                    List<CourseModel> maxCourseList = ssciCourses.Last().Value;
                    if (maxCourseList.Count == 1)
                    {
                        if (hassReqSet.CanApplyCourse(maxCourseList[0]))
                        {
                            hassReqSet.ApplyCourse(maxCourseList[0]);
                            maxCourseList[0].RequirementSetName = "HASS";
                            //remove the key from the list of remaining courses
                            ssciCourses.Remove(ssciCourses.Last().Key);
                        }
                    }
                    else
                    {
                        //apply the first course in the list
                        if (hassReqSet.CanApplyCourse(maxCourseList[0]))
                        {
                            hassReqSet.ApplyCourse(maxCourseList[0]);
                            maxCourseList[0].RequirementSetName = "HASS";
                            //remove the course from list of remaining courses
                            maxCourseList.RemoveAt(0);
                            ssciCourses[humCourses.Count - 1] = maxCourseList;
                        }
                    }
                }

                //no more courses to add, you're done
                if (humCourses.Count == 0 && ssciCourses.Count == 0)
                {
                    return;
                }
            }
        }
        public static void FillNamedRequirements(List<RequirementSetModel> allSets, List<CourseModel> courses)
        {
            foreach (RequirementSetModel reqset in allSets)
            {
                if (reqset.Name == "Free Electives") continue; 
                foreach (CourseModel course in courses.Where(reqset.CanApplyCourse))
                {
                    reqset.ApplyCourse(course);
                    course.RequirementSetName = reqset.Name;
                }
            }
            var freeElectives = allSets.FirstOrDefault(set => set.Name == "Free Electives");
            if (freeElectives == null) return;
            foreach (var course in courses.Where(c => string.IsNullOrEmpty(c.RequirementSetName) || c.RequirementSetName == "Unapplied Courses"))
            {
                if (freeElectives.CanApplyCourse(course))
                {
                    freeElectives.ApplyCourse(course);
                    course.RequirementSetName = "Free Electives";
                }
            }
        }

        /// <summary>
        ///     Fills in classes automatically.
        ///     Note that the HASS RequirementSet must be named "HASS"
        /// </summary>
        /// <param name="requirementSets"></param>
        /// <param name="courses"></param>
        public static void AutoPopulate(List<RequirementSetModel> requirementSets, List<CourseModel> courses)
        {
            FillNamedRequirements(requirementSets, courses);

            //find and check HASS
            foreach (RequirementSetModel reqset in requirementSets.Where(reqset => reqset.Name == "HASS"))
            {
                FillHass(reqset, courses);
            }
        }
    }
}