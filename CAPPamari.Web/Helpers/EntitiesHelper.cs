﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAPPamari.Web.Models;
using CAPPamari.Web.Models.Requirements;

namespace CAPPamari.Web.Helpers
{
    internal static class EntitiesHelper
    {
        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <param name="username">Username for new user</param>
        /// <param name="password">Password for new user</param>
        /// <param name="major">Major for new user</param>
        public static void CreateNewUser(string username, string password, string major)
        {
            using (var entities = GetEntityModel())
            {
                var newUser = new ApplicationUser
                {
                    Username = username,
                    Password = password,
                    Major = major
                };

                entities.ApplicationUsers.Add(newUser);
                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Checks to see if Username is already taken
        /// </summary>
        /// <param name="username">Username to check in the database for existence</param>
        /// <returns>True if Username is taken, false otherwise</returns>
        public static bool UsernameExists(string username)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                return user != null;
            }
        }

        /// <summary>
        ///     Gets the password for the user name given.
        /// </summary>
        /// <param name="username">Username of user to get password for</param>
        /// <returns>Password for user with Username or string.Empty if no user is found</returns>
        public static string GetPassword(string username)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? string.Empty : user.Password;
            }
        }

        /// <summary>
        ///     Update user information
        /// </summary>
        /// <param name="username">Username of user to update</param>
        /// <param name="password">Password to update to</param>
        /// <param name="major">Major to update to</param>
        /// <returns>True if user was updated, false otherwise</returns>
        public static bool UpdateUser(string username, string password, string major)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                if (user == null) return false;

                user.Password = password;
                user.Major = major;
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Gets the list of advisors for the user with Username
        /// </summary>
        /// <param name="username">Username for user to look up advisors</param>
        /// <returns>List of advisors for user with Username or empty list if no user is found</returns>
        public static List<Advisor> GetAdvisors(string username)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? new List<Advisor>() : user.Advisors.ToList();
            }
        }

        public static string GetMajor(string username)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);

                return user == null ? string.Empty : user.Major;
            }
        }

        /// <summary>
        ///     Gets active SessionID if one exists
        /// </summary>
        /// <param name="username">Username for user to look up SessionID</param>
        /// <returns>SessionID of valid session or -1 if no such session exists</returns>
        public static int GetSessionId(string username)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);
                if (user == null) return -1;

                var session = user.UserSessions.FirstOrDefault();
                if (session == null) return -1;
                return session.Expiration < DateTime.Now ? -1 : session.SessionID;
            }
        }

        /// <summary>
        ///     Creates a new session for the user, deleting a current session if one exists
        /// </summary>
        /// <param name="username">Username to create session for</param>
        /// <returns>SessionID of newly created session</returns>
        public static int CreateNewSession(string username)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.Username == username);
                if (session != null) entities.UserSessions.Remove(session);

                var newSession = new UserSession
                {
                    Username = username,
                    Expiration = DateTime.Now.AddMinutes(30)
                };
                entities.UserSessions.Add(newSession);
                entities.SaveChanges();

                return newSession.SessionID;
            }
        }

        /// <summary>
        ///     Gets the expiration of the session refered to by SessionID
        /// </summary>
        /// <param name="sessionId">SessionID for session to look up expiration</param>
        /// <returns>Expriation of session or DateTime.MinValue</returns>
        public static DateTime GetSessionExpiration(int sessionId)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == sessionId);

                return session == null ? DateTime.MinValue : session.Expiration;
            }
        }

        /// <summary>
        ///     Removes session for SessionID and Username
        /// </summary>
        /// <param name="sessionId">SessionID for session to clear</param>
        /// <param name="username">Username for session to clear</param>
        public static void RemoveSession(int sessionId, string username)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.SessionID == sessionId);
                if (session == null) return;
                entities.UserSessions.Remove(session);

                entities.SaveChanges();
            }
        }

        /// <summary>
        ///     Updates a session for a user because they have committed an action
        /// </summary>
        /// <param name="username">Username of user to update session for</param>
        /// <returns>True if session is active and refreshed, false otherwise</returns>
        public static bool UpdateSession(string username)
        {
            using (var entities = GetEntityModel())
            {
                var session = entities.UserSessions.FirstOrDefault(sess => sess.Username == username);
                if (session == null) return false;

                session.Expiration = DateTime.Now.AddMinutes(30);
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Change a major for a specific user.
        /// </summary>
        /// <param name="username">Username of user to change major</param>
        /// <param name="major">Major to change to.</param>
        /// <returns>Success status of change.</returns>
        public static bool ChangeMajor(string username, string major)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appUser => appUser.Username == username);
                if (user == null) return false;

                user.Major = major;
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Add an advisor to the database
        /// </summary>
        /// <param name="name">Name of the new advisor to add</param>
        /// <param name="email">Email address of the new advisor to add</param>
        /// <returns>The AdvisorID of the new advisor or -1 if the advisor already exists in the database</returns>
        public static int AddAdvisor(string name, string email)
        {
            using (var entities = GetEntityModel())
            {
                var existingAdvisor =
                    entities.Advisors.FirstOrDefault(
                        dbadvisor => dbadvisor.Name == name && dbadvisor.EmailAddress == email);
                if (existingAdvisor != null) return -1;

                var newAdvisor = new Advisor
                {
                    EmailAddress = email,
                    Name = name
                };
                entities.Advisors.Add(newAdvisor);
                entities.SaveChanges();

                return newAdvisor.AdvisorID;
            }
        }

        /// <summary>
        ///     Update advisor in database
        /// </summary>
        /// <param name="name">Name of advisor to update</param>
        /// <param name="email">Email to update advisor to</param>
        /// <returns>Success status of the update</returns>
        public static bool UpdateAdvisor(string name, string email)
        {
            using (var entities = GetEntityModel())
            {
                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.Name == name);
                if (advisor == null) return false;

                advisor.EmailAddress = email;
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Gets the AdvisorID of an advisor
        /// </summary>
        /// <param name="name">Name of the advisor to get the AdvisorID of</param>
        /// <param name="email">EmailAddress of the advisor to get the AdvisorID of</param>
        /// <returns>AdvisorID corresponding to the right advisor or -1 if no such advisor exists</returns>
        public static int GetAdvisorId(string name, string email)
        {
            using (var entities = GetEntityModel())
            {
                var advisor =
                    entities.Advisors.FirstOrDefault(
                        dbadvisor => dbadvisor.Name == name && dbadvisor.EmailAddress == email);
                if (advisor == null) return -1;

                return advisor.AdvisorID;
            }
        }

        /// <summary>
        ///     Add an association between a user and an advisor
        /// </summary>
        /// <param name="username">Username of user to create the association with</param>
        /// <param name="advisorId">AdvisorID of the advisor to create the association with</param>
        /// <returns>Success state of the association creation</returns>
        public static bool AssociateAdvisorAndUser(string username, int advisorId)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == advisorId);
                if (advisor == null) return false;

                user.Advisors.Add(advisor);
                entities.SaveChanges();

                return true;
            }
        }

        /// <summary>
        ///     Remove an association between a user and an advisor
        /// </summary>
        /// <param name="username">Username of user to remove the association with</param>
        /// <param name="advisorId">AdvisorID of the advisor to remove the association with</param>
        /// <returns>Success state of the association deletion</returns>
        public static bool DisassociateAdvisorAndUser(string username, int advisorId)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                var advisor = entities.Advisors.FirstOrDefault(dbadvisor => dbadvisor.AdvisorID == advisorId);
                if (advisor == null) return false;

                var success = user.Advisors.Remove(advisor);
                if (success) entities.SaveChanges();
                return success;
            }
        }

        /// <summary>
        ///     Adds a new course to the Unapplied Courses RequirementSet for a specified user
        /// </summary>
        /// <param name="username">Username for user to add new course for</param>
        /// <param name="newCourse">CourseModel containing information about the new course</param>
        /// <param name="requirementSetName">Name of the requirement set to retrieve</param>
        /// <returns>Success state of the course addition</returns>
        public static bool AddNewCourse(string username, CourseModel newCourse, string requirementSetName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                var reqSet = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSetName);
                if (reqSet == null) return false;

                reqSet.Courses.Add(new Course
                {
                    Credits = newCourse.Credits,
                    Department = newCourse.DepartmentCode,
                    Grade = newCourse.Grade,
                    Number = newCourse.CourseNumber,
                    PassNC = newCourse.PassNoCredit,
                    Semester = newCourse.Semester,
                    CommunicationIntensive = newCourse.CommIntensive
                });
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Remove a course for a specified user
        /// </summary>
        /// <param name="username">Username of user to remove course for</param>
        /// <param name="oldCourse">CourseModel containing information about course to remove</param>
        /// <returns>Success state of course removal</returns>
        public static bool RemoveCourse(string username, CourseModel oldCourse)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                foreach (var reqSet in report.RequirementSets)
                {
                    var remover =
                        reqSet.Courses.FirstOrDefault(
                            course =>
                                course.Department == oldCourse.DepartmentCode && course.Number == oldCourse.CourseNumber);
                    if (remover == null) continue;
                    reqSet.Courses.Remove(remover);
                    entities.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///     Gets a RequirementSet for a user
        /// </summary>
        /// <param name="username">Username of user to get RequirementSet for</param>
        /// <param name="requirementSetName">Name of the requirement set to retrieve</param>
        /// <returns>RequirementSet desired or null if no such RequirementSet exists</returns>
        public static RequirementSetModel GetRequirementSet(string username, string requirementSetName)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                var dbset = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSetName);
                return dbset == null ? null : dbset.ToRequirementSetModel();
            }
        }

        /// <summary>
        ///     Apply a course to a requirement set for a user
        /// </summary>
        /// <param name="username">Username of user to move course for</param>
        /// <param name="course">CourseModel for course to move</param>
        /// <param name="requirementSet">RequirementSetModel to move course into</param>
        /// <returns>Success status of move</returns>
        public static bool ApplyCourse(string username, CourseModel course, RequirementSetModel requirementSet)
        {
            using (JustinEntities entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return false;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return false;

                var dbset = report.RequirementSets.FirstOrDefault(set => set.Name == requirementSet.Name);
                if (dbset == null) return false;

                var newCourse = entities.Courses.FirstOrDefault(c => c.CommunicationIntensive == course.CommIntensive &&
                                                                     c.Credits == course.Credits &&
                                                                     c.Department == course.DepartmentCode &&
                                                                     c.Grade == course.Grade &&
                                                                     c.Number == course.CourseNumber &&
                                                                     c.PassNC == course.PassNoCredit &&
                                                                     c.Semester == course.Semester &&
                                                                     c.RequirementSet.CAPPReport.ApplicationUser
                                                                         .Username == username);
                if (newCourse == null) return false;

                newCourse.RequirementSet = dbset;
                entities.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Gets all the RequirementSets for a user
        /// </summary>
        /// <param name="username">Username for user to get all the RequirementSets for</param>
        /// <returns>List of all RequirementSets</returns>
        public static CappReportModel GetCappReport(string username)
        {
            using (var entities = GetEntityModel())
            {
                var user = entities.ApplicationUsers.FirstOrDefault(appuser => appuser.Username == username);
                if (user == null) return null;

                var report = user.CAPPReports.FirstOrDefault();
                if (report == null) return null;

                return report.ToCappReportModel();
            }
        }

        /// <summary>
        ///     Returns new entities object.
        /// </summary>
        /// <returns></returns>
        private static JustinEntities GetEntityModel()
        {
            return new JustinEntities();
        }
    }
}