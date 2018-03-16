using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web;

namespace WebApi.Controllers
{
    public class StudentsController : BaseController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Student> list = new List<Student>();
            list = db.Students.ToList();
            return Ok(list.Select(s => new Student
            {
                StudentId = s.StudentId,
                StudentName = s.StudentName,
                Password = s.Password,
                Address = s.Address,
                PhoneNumber = s.PhoneNumber,
                MajorId = s.MajorId
            }));
        }

        [HttpGet]
        [Route("api/Books/GetMajor")]
        public IHttpActionResult GetMajor()
        {
            List<Major> list = new List<Major>();
            list = db.Majors.ToList();
            return Ok(list.Select(s => new Major
            {
                MajorId = s.MajorId,
                MajorName = s.MajorName
            }));

        }

        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret;
            List<Student> list = new List<Student>();
            Student student = new Student();

            student = db.Students.FirstOrDefault(p => p.StudentId == id);
            if (student == null)
            {
                ret = NotFound();
            }
            else
            {
                ret = Ok(student);
            }
            return ret;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != student.StudentId)
            {
                return BadRequest();
            }
            db.Entry(student).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (StudentRent(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public IHttpActionResult Post(Student student)
        {
            IHttpActionResult ret = null;
            student = db.Students.Add(student);
            db.SaveChanges();
            if (student != null)
            {
                ret = Created<Student>(Request.RequestUri +
                     student.StudentId.ToString(), student);
            }
            else
            {
                ret = NotFound();
            }
            return ret;
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            db.Students.Remove(student);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudentRent(id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);

        }

        private bool StudentRent(int id)
        {
            return db.RentalDetails.Count(e => e.StudentId == id) > 0;
        }
        [HttpGet]
        [Route("api/Students/PassStudentSession")]
        public IHttpActionResult PassStudentSession()
        {
            List<string> listSession = new List<string>();
            if (HttpContext.Current.Session["StudentName"] != null)
            {
                listSession.Add(HttpContext.Current.Session["StudentName"].ToString());
                listSession.Add(HttpContext.Current.Session["StudentId"].ToString());
            }            
            return Ok(listSession);
        }
    }
}

