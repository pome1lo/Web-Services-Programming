using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly HttpClient _client;

        public StudentsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:51565/Service1.svc/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("student");
            if (response.IsSuccessStatusCode)
            {
                var students = await response.Content.ReadAsAsync<IEnumerable<Student>>();
                return View(students);
            }
            return View(new List<Student>());
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync("student", student);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error adding student");
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var response = await _client.DeleteAsync($"student({id})");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Error deleting student");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PutAsJsonAsync($"student({student.Id})", student);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error updating student");
            }
            return View(student);
        }
    }

}