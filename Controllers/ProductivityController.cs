using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetProductivity.Helpers;
using NetProductivity.Models;

namespace NetProductivity.Controllers
{

    public class ProductivityController : Controller
    {
        private static ProductivityContext db;

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
        public static Guid CurrentTaskId { get; set; }

        [HttpPost]
        public IActionResult СreateProject(Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }

            var id = Guid.NewGuid();
            project.Id = id;
            db.Projects.Add(project);
            var user = User.Identity.GetUserId();
            db.UserProjects.Add(
                new UserProjects
                {
                    UserId = user, 
                    ProjectId = id,
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

        [HttpGet("/UpdateProject/{name}")]
        public IActionResult UpdateProject(string name)
        {
            CurrentProj = name;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProjects(Project project)
        {
            var current = db.Projects.FirstOrDefault(p => p.Name == CurrentProj);
            current.Name = project.Name;
            db.Projects.Update(current);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet("/UpdateTask/{name}")]
        public IActionResult UpdateTask(Guid name)
        {
            CurrentTaskId = name;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateTask(TaskViewModel task)
        {
            var current = db.Tasks.FirstOrDefault(p => p.Id == CurrentTaskId);
            if (task.Name != null)
            {
                current.Name = task.Name;
            }
            int prior = 0;
            if (task.Priority == "Hight")
            {
                prior = 1;
            }
            else if (task.Priority == "Medium")
            {
                prior = 2;
            }
            else
            {
                prior = 3;
            }
            current.EndDate = task.EndDate;
            current.Priority = prior;
            db.Tasks.Update(current);
            var currentP = db.UserProjects.FirstOrDefault(p => p.ProjectId == current.ProjectId);
            currentP.Status = Status.Active.ToString();
            db.UserProjects.Update(currentP);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet("/DeleteTask/{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var current = db.Tasks.FirstOrDefault(t => t.Id == id);
            db.Tasks.Remove(current);
            db.SaveChanges();
            return RedirectToAction("Main", "Productivity");
        }

        [HttpGet("/DeleteProject/{name}")]
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
                Load();

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
                DictComparer<TaskP> comparer = new DictComparer<TaskP>();
                Dictionary<string, List<TaskP>> tasks = new Dictionary<string, List<TaskP>>();
                Random rand = new Random();
                foreach (var project in projects)
                {
                    var projectTasks = db.Tasks.Where(id => id.ProjectId == project.Id).ToList();
                    projectTasks.Sort(comparer);
                    if (tasks.Keys.Contains(project.Name))
                    {
                        project.Name = project.Name + rand.Next(1, 100);
                        db.Projects.Update(project);
                        db.SaveChanges();
                    }
                    tasks.Add(project.Name, projectTasks);
                }

                return View(tasks);
            }
            else
            {
                return RedirectToAction("Login", "Register");
            }
        }

        public static bool IsActive(string name)
        {
            var proj = db.Projects.FirstOrDefault(p => p.Name == name);
            var current = db.UserProjects.FirstOrDefault(p => p.ProjectId == proj.Id);
            if (current.Status == Status.Done.ToString())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Load()
        {
            foreach (var proj in db.Projects)
            {
                var projectTasks = db.Tasks.Where(id => id.ProjectId == proj.Id).ToList();
                var res = projectTasks.Find(t => t.EndDate > DateTime.Today);
                if (res == null)
                {
                    var current = db.UserProjects.FirstOrDefault(p => p.ProjectId == proj.Id);
                    current.Status = Status.Done.ToString();
                    //db.SaveChanges();
                }
            }
            db.SaveChanges();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
