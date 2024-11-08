using StudentsModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication4.Controllers
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
                _service.SaveChanges();

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
                var student = _service.student.AsEnumerable().First(i => i.id == id);

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
                var student = _service.student.AsEnumerable().First(i => i.id == id);
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


        // POST: Notes/Create
        [HttpPost]
        public ActionResult CreateNote(int studentId, string subject, int noteValue)
        {
            try
            {
                var note = new note
                {
                    stud_id = studentId,
                    subject = subject,
                    note1 = noteValue
                };
                _service.AddTonote(note);
                _service.SaveChanges(); // Save changes in OData service
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // POST: Notes/Delete/5
        [HttpPost]
        public ActionResult DeleteNote(int id)
        {
            try
            {
                var note = _service.note.AsEnumerable().First(n => n.id == id);
                if (note != null)
                {
                    _service.DeleteObject(note);
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

        // POST: Notes/Update
        [HttpPost]
        public ActionResult UpdateNote(int id, int studentId, string subject, int noteValue)
        {
            try
            {
                var note = _service.note.AsEnumerable().First(n => n.id == id);
                if (note != null)
                {
                    note.stud_id = studentId;
                    note.subject = subject;
                    note.note1 = noteValue;
                    _service.UpdateObject(note);
                    _service.SaveChanges(); // Save the updated note
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Similar methods can be created for managing notes
    }
}