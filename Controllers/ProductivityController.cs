using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetProductivity.Models;

namespace NetProductivity.Controllers
{

    public class ProductivityController : Controller
    {
        private ProductivityContext db;

        public ProductivityController(ProductivityContext context)
        {
            db = context;
        }

        [HttpGet]
        public IEnumerable<Project> GetProject()
        {
            return db.Projects.ToList();
        }

        [HttpGet("{projectId}")]
        public IEnumerable<TaskP> GetTasks(Guid id)
        {
            return db.Tasks.ToList().Where(x => x.ProjectId == id);
        }

        [HttpGet]
        public IActionResult СreateProject()
        {
            return View();
        }

        public static string CurrentProj { get; set; }

        [HttpPost]
        public IActionResult СreateProject(Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }

            db.Projects.Add(project);
            var user = User.Identity.GetUserId();
            db.UserProjects.Add(
                new UserProjects
                {
                    UserId = User.Identity.GetUserId(), 
                    ProjectId = project.Id,
                    Status = Status.New.ToString()
                });
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet]
        public IActionResult СreateTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult СreateTask(TaskViewModel current)
        {
            if (current == null)
            {
                return BadRequest();
            }

            int prior = 0;
            if (current.Priority=="Hight")
            {
                prior = 1;
            }
            else if(current.Priority=="Medium")
            {
                prior = 2;
            }
            else
            {
                prior = 3;
            }

            var id = db.Projects.Where(p => p.Name == CurrentProj).FirstOrDefault();
            TaskP task = new TaskP
            {
                Id = Guid.NewGuid(),
                Name = current.Name,
                ProjectId = id.Id,
                Priority = prior,
                Status = Status.New.ToString(),
                EndDate = current.EndDate
            };

            task.Status = Status.New.ToString();
            db.Tasks.Add(task);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet]
        public IActionResult UpdateProject()
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateProject(Project project)
        {
            var current = db.Projects.FirstOrDefault(p => p.Id == project.Id);
            current.Name = project.Name;
            db.Projects.Update(current);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet]
        public IActionResult UpdateTask()
        {
            return View("СreateTask");
        }

        [HttpPut]
        public IActionResult UpdateTask(TaskP task)
        {
            var current = db.Tasks.FirstOrDefault(p => p.Id == task.Id);
            current.Name = task.Name;
            current.EndDate = task.EndDate;
            current.Priority = task.Priority;
            db.Tasks.Update(current);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpDelete]
        public IActionResult DeleteTask(Guid id)
        {
            var current = db.Tasks.FirstOrDefault(t => t.Id == id);
            db.Tasks.Remove(current);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpDelete]
        public IActionResult DeleteProject(string name)
        {
            var current = db.Projects.FirstOrDefault(t => t.Name == name);
            db.Projects.Remove(current);
            foreach (var task in db.Tasks)
            {
                if (task.ProjectId == current.Id)
                {
                    db.Tasks.Remove(task);
                }
            }

            var proj = db.UserProjects.FirstOrDefault(p => p.ProjectId == current.Id);
            db.UserProjects.Remove(proj);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult Main()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Guid> projectsId = new List<Guid>();
                foreach (var proj in db.UserProjects)
                {
                    if (proj.UserId == User.Identity.GetUserId())
                    {
                        projectsId.Add(proj.ProjectId);
                    }
                }

                List<Project> projects = new List<Project>();
                foreach (var res in db.Projects)
                {
                    if (projectsId.Contains(res.Id))
                    {
                        projects.Add(res);
                    }
                }

                Dictionary<string, List<TaskP>> tasks = new Dictionary<string, List<TaskP>>();
                foreach (var project in projects)
                {
                    var projectTasks = db.Tasks.Where(id => id.ProjectId == project.Id).ToList();

                    tasks.Add(project.Name, projectTasks);
                }

                //ViewBag.Info = general;

                return View(tasks);
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
