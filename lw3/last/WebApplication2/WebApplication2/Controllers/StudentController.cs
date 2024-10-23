using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;
using WebApplication2.Models;
using static WebApplication2.Test.XMLRes;

namespace WebApplication2.Controllers
{
    public class StudentController : ApiController
    {
        private Model1 _context = new Model1();

        [Route("api/student.{format}/{id}")]
        [ResponseType(typeof(Students))]
        public object GetStudent(
            int id,
            string columns = "id, name, phone",
            string type = "json")
        {
            Students student = _context.Students.Find(id);

            string acceptHeader = Request.Headers.Accept.FirstOrDefault()?.MediaType;

            string format = ControllerContext.Request.GetRouteData().Values["format"] as string;

            if (format == "json")
            {
                if (student == null)
                {
                    return Content(HttpStatusCode.NotFound, new HATEOAS("/api/error/400/1", "400.1", Request.Method.ToString()));
                }
                var links = GenerateLinks(new List<string> { "PUT", "DELETE" }, "application/" + format, student.Id);
                return Content(HttpStatusCode.OK, new { student, links });
            }
            else if (format == "xml")
            {
                if (student == null)
                {
                    var xHrefE = new XElement("href", $"/api/error/400/1");
                    var linksXmlE = new List<XElement>();
                    linksXmlE.Add(xHrefE);
                    return Content(HttpStatusCode.NotFound, string.Join("", linksXmlE), Configuration.Formatters.XmlFormatter);
                }
                
                var links = GenerateLinks(new List<string> { "PUT", "DELETE" }, "application/" + format, student.Id);

                var xId = new XAttribute("id", student.Id);
                var xName = new XAttribute("name", student.Name);
                var xPhone = new XAttribute("phone", student.Phone);
                var xUser = new XElement("student", links);

                if (columns.Contains("id") || columns.Contains("name") || columns.Contains("phone"))
                {
                    if (columns.Contains("id"))
                    {
                        xUser.Add(xId);
                    }
                    if (columns.Contains("name"))
                    {
                        xUser.Add(xName);
                    }
                    if (columns.Contains("phone"))
                    {
                        xUser.Add(xPhone);
                    }
                }
                else
                {
                    return Content(HttpStatusCode.OK, new XElement[0], Configuration.Formatters.XmlFormatter);
                }

                var xmlDocument = new XDocument(new XElement("root", xUser));
                return new XmlResult(xmlDocument);
            }
            else
            {
                return Content(HttpStatusCode.UnsupportedMediaType, new HATEOAS("/api/error/400/2", "400.2", Request.Method.ToString()));
            }
        }

        [HttpGet]
        public IHttpActionResult Get(
                int limit = 5,
                int offset = 0,
                int minId = 0,
                int maxId = 200,
                string sort = "ID",
                string columns = "id, name, phone",
                string like = null,
                string globalLike = null,
                String type = "json",
                int pagination = 0)
        {
            
            int tempOffset = offset;
            bool nextPageBlocked = false;
            string acceptHeader = Request.Headers.Accept.FirstOrDefault()?.MediaType;

            Console.WriteLine(limit);

            if (limit < 0 || offset < 0 || minId < 0 || maxId < 0)
            {
                return Content(HttpStatusCode.BadRequest, new HATEOAS("/api/error/400/4", "400.4", Request.Method.ToString()));
            }
            if (minId >= maxId)
            {
                return Content(HttpStatusCode.BadRequest, new HATEOAS("/api/error/400/5", "400.5", Request.Method.ToString()));
            }

            if (!string.IsNullOrEmpty(acceptHeader))
            {
                if (acceptHeader.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                {
                    type = "json";
                }
                else if (acceptHeader.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
                {
                    type = "xml";
                }
                else
                {
                    return Content(HttpStatusCode.UnsupportedMediaType, new HATEOAS("/api/error/400/2", "400.2", Request.Method.ToString()));

                }
            }

            var students = _context.Students.Where(x => x.Id > 0).AsNoTracking();

            if (sort.ToLower() == "name")
            {
                students = students.OrderBy(prop => prop.Name);
            }
            else if (sort.ToLower() == "id")
            {
                students = students.OrderBy(prop => prop.Id);
            }


            if (students.Count() - pagination > 0)
            {
                if (students.Count() < pagination + limit)
                {
                    nextPageBlocked = true;
                }
                else
                {
                    nextPageBlocked = false;
                }
                tempOffset = offset + pagination;
            }
            students = students
               .Where(prop => prop.Id >= minId && prop.Id <= maxId)
               .Skip(tempOffset)
               .Take(limit);

            if (like != null)
            {
                students = students.Where(prop => prop.Name.ToLower().Contains(like.ToLower()));
            }

            if (globalLike != null)
            {
                students = students.Where(prop => (prop.Id.ToString() + prop.Name + prop.Phone).ToLower().Contains(globalLike.ToLower()));
            }

            var resJSON = new List<dynamic>();
            XElement resXML = new XElement("example");

            if (type.ToLower() == "xml")
            {
                var arrayOfStudents = new XElement("ArrayOfStudents");

                foreach (var item in students)
                {
                    var linksXml = GenerateLinks(new List<string> { "GET", "PUT", "DELETE" }, acceptHeader, item.Id);

                    var xId = new XAttribute("id", item.Id);
                    var xName = new XAttribute("name", item.Name);
                    var xPhone = new XAttribute("phone", item.Phone);
                    var xUser = new XElement("student", linksXml);

                    if (columns.Contains("id") || columns.Contains("name") || columns.Contains("phone"))
                    {
                        if (columns.Contains("id"))
                        {
                            xUser.Add(xId);
                        }
                        if (columns.Contains("name"))
                        {
                            xUser.Add(xName);
                        }
                        if (columns.Contains("phone"))
                        {
                            xUser.Add(xPhone);
                        }
                        arrayOfStudents.Add(xUser);
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, new XElement[0], Configuration.Formatters.XmlFormatter);
                    }
                }

                var addStudent = new XElement("AddStudent", new XElement("link",
                        new XAttribute("rel", "add"),
                        new XAttribute("href", $"/api/student"),
                        new XAttribute("method", "POST")));

                var xNextPageBlocked = new XElement("btnBlock", nextPageBlocked);

                resXML = new XElement("Links",
                    arrayOfStudents,
                    addStudent,
                    xNextPageBlocked);
            }
            else
            {
                dynamic Links = new ExpandoObject();
                List<dynamic> arrayOfStudents = new List<dynamic>();
                foreach (var item in students)
                {
                    var links = GenerateLinks(new List<string> { "GET", "PUT", "DELETE" }, acceptHeader, item.Id);

                    dynamic studentObj = new ExpandoObject();

                    if (columns.Contains("id") || columns.Contains("name") || columns.Contains("phone"))
                    {
                        if (columns.Contains("id"))
                        {
                            studentObj.Id = item.Id;
                        }
                        if (columns.Contains("name"))
                        {
                            studentObj.Name = item.Name;
                        }
                        if (columns.Contains("phone"))
                        {
                            studentObj.Phone = item.Phone;
                        }
                        studentObj.Links = links;
                        arrayOfStudents.Add(studentObj);
                    }
                    else
                    {
                        arrayOfStudents.Add(null);
                    }
                }

                var addStudentJson = new
                {
                    link = new
                    {
                        rel = "add",
                        href = "/api/student",
                        method = "POST"
                    }
                };

                Links.students = arrayOfStudents;
                Links.addStudent = addStudentJson;
                Links.btnBlock = nextPageBlocked;

                resJSON.Add(Links);

            }
            switch (type)
            {
                case "xml":
                    {
                        return Content(HttpStatusCode.OK, resXML, Configuration.Formatters.XmlFormatter);
                    }
                default:
                    return Content(HttpStatusCode.OK, resJSON, Configuration.Formatters.JsonFormatter);
            }
        }

        [HttpPost]
        [ResponseType(typeof(Students))]
        public object PostStudent(Students student)
        {

            if (!ModelState.IsValid || student.Name == "" || student.Phone == "")
            {
                return Content(HttpStatusCode.BadRequest, new { ModelState, links = new HATEOAS("/api/error/400/3", "400.3", Request.Method.ToString()) });
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            string acceptHeader = Request.Headers.Accept.FirstOrDefault()?.MediaType;

            if (acceptHeader.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return Content(HttpStatusCode.OK, new { student, Links = new HATEOAS($"/api/student", "self", Request.Method.ToString()) });
            }
            else if (acceptHeader.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
            {
                return Content(HttpStatusCode.OK, student, Configuration.Formatters.XmlFormatter);
            }
            else
            {
                return Content(HttpStatusCode.UnsupportedMediaType, new HATEOAS("/api/error/400/2", "400.2", Request.Method.ToString()));
            }

        }

        [HttpPut]
        [ResponseType(typeof(Students))]
        public object PutStudent(int id, Students student)
        {
            student.Id = id;

            string acceptHeader = Request.Headers.Accept.FirstOrDefault()?.MediaType;

            if (!ModelState.IsValid || student.Name == "" || student.Phone == "")
            {
                return Content(HttpStatusCode.BadRequest, new { ModelState, links = new HATEOAS("/api/error/400/3", "400.3", Request.Method.ToString()) });
            }

            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();

            var links = GenerateLinks(new List<string> { "GET", "DELETE" }, acceptHeader, id);

            if (acceptHeader.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return Content(HttpStatusCode.OK, new { student, links });
            }
            else if (acceptHeader.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
            {

                var xUser = new XElement("student",
                    new XAttribute("id", student.Id),
                    new XAttribute("name", student.Name),
                    new XAttribute("phone", student.Phone),
                    links);

                return Content(HttpStatusCode.OK, xUser, Configuration.Formatters.XmlFormatter);
            }
            else
            {
                return Content(HttpStatusCode.UnsupportedMediaType, new HATEOAS("/api/error/400/2", "400.2", Request.Method.ToString()));
            }
        }

        [HttpDelete]
        [ResponseType(typeof(Students))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Students student = _context.Students.Find(id);
            if (student == null)
            {
                return Content(HttpStatusCode.NotFound, new HATEOAS("/api/error/400/1", "400.1", Request.Method.ToString()));
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            string acceptHeader = Request.Headers.Accept.FirstOrDefault()?.MediaType;
            var links = GenerateLinks(new List<string> { "GET", "PUT" }, acceptHeader, id);
            if (acceptHeader.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return Content(HttpStatusCode.OK, new { student, links });
            }
            else if (acceptHeader.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
            {

                var xUser = new XElement("student",
                    new XAttribute("id", student.Id),
                    new XAttribute("name", student.Name),
                    new XAttribute("phone", student.Phone),
                    links);
                return Content(HttpStatusCode.OK, xUser, Configuration.Formatters.XmlFormatter);
            }
            else
            {
                return Content(HttpStatusCode.UnsupportedMediaType, new HATEOAS("/api/error/400/2", "400.2", Request.Method.ToString()));
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Count(x => x.Id == id) > 0;
        }
        static List<object> GenerateLinks(List<string> methods, string format, int studentId)
        {
            var linksXml = new List<XElement>();
            var linksJson = new List<object>();
            if (format.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var method in methods)
                {
                    switch (method)
                    {
                        case "GET":
                            {
                                linksXml.Add(new XElement("link",
                                new XAttribute("rel", "self"),
                                new XAttribute("href", $"/api/student.xml/{studentId}"),
                                new XAttribute("method", method)));
                                break;
                            }
                        case "PUT":
                            {
                                linksXml.Add(new XElement("link",
                                new XAttribute("rel", "updateRecord"),
                                new XAttribute("href", $"/api/student/{studentId}"),
                                new XAttribute("method", method)));
                                break;
                            }
                        case "DELETE":
                            {
                                linksXml.Add(new XElement("link",
                                new XAttribute("rel", "deleteRecord"),
                                new XAttribute("href", $"/api/student/{studentId}"),
                                new XAttribute("method", method)));
                                break;
                            }
                        case "POST":
                            break;
                    }
                }
                return linksXml.Cast<object>().ToList();
            }
            else
            {
                foreach (var method in methods)
                {
                    switch (method)
                    {
                        case "GET":
                            {
                                linksJson.Add(new
                                {
                                    rel = "self",
                                    href = $"/api/student.json/{studentId}",
                                    method = method
                                });
                                break;
                            }
                        case "PUT":
                            {
                                linksJson.Add(new
                                {
                                    rel = "updateRecord",
                                    href = $"/api/student/{studentId}",
                                    method = method
                                });
                                break;
                            }
                        case "DELETE":
                            {
                                linksJson.Add(new
                                {
                                    rel = "deleteRecord",
                                    href = $"/api/student/{studentId}",
                                    method = method
                                });
                                break;
                            }
                        case "POST":
                            break;
                    }
                }
                return linksJson;
            }
        }

    }
}
