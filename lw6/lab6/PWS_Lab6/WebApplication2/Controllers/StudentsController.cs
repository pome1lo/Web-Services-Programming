using StudentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class StudentsController : Controller
    {
        private StudentsEntities _service;

        public StudentsController()
        {
            _service = new StudentsEntities(new Uri("http://localhost:51565/Service1.svc"));
        }

        // GET: Students
        public ActionResult Index()
        {
            // Fetching all students and notes to display in the view
            var students = _service.student.AsEnumerable();
            var notes = _service.note.AsEnumerable();
            ViewBag.Notes = notes; // Passing notes to the view via ViewBag
            return View(students);
        }

        // POST: Students/Create
        [HttpPost]
        public ActionResult Create(string studentName)
        {
            try
            {
                var student = new student { name = studentName };
                _service.AddTostudent(student);
                _service.SaveChanges(); // Save changes in OData service
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // POST: Students/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var student = _service.student.FirstOrDefault(s => s.id == id);
                if (student != null)
                {
                    _service.DeleteObject(student);
                    _service.SaveChanges(); // Persist the deletion
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // POST: Students/Update
        [HttpPost]
        public ActionResult Update(int id, string newName)
        {
            try
            {
                var student = _service.student.FirstOrDefault(s => s.id == id);
                if (student != null)
                {
                    student.name = newName;
                    _service.UpdateObject(student);
                    _service.SaveChanges(); // Save the updated student
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
    }