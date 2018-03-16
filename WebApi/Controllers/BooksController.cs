using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BooksController : BaseController
    {
        [HttpGet]
        [Route("api/Books/GetCategory")]
        public IHttpActionResult GetCategory()
        {
            List<Category> list = new List<Category>();
            list = db.Categories.ToList();
            return Ok(list.Select(s => new Category
            {
                CategoryId = s.CategoryId,
                CategoryName = s.CategoryName
            }));

        }

        [HttpGet]
        [Route("api/Books/GetAuthor")]
        public IHttpActionResult GetAuthor()
        {
            List<Author> list = new List<Author>();
            list = db.Authors.ToList();
            return Ok(list.Select(s => new Author
            {
                AuthorId= s.AuthorId,
                AuthorName = s.AuthorName,
                Introduction = s.Introduction
            }));
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Book> list = new List<Book>();
            list = db.Books.ToList();
            return Ok(list.Select(s => new Book
            {
                CallNumber = s.CallNumber,
                ISBN = s.ISBN,
                Title = s.Title,
                AuthorId = s.AuthorId,
                CategoryId = s.CategoryId,
                Photo = s.Photo,
                Views = s.Views
            }));

        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;
            Book book = new Book();
            book = db.Books.FirstOrDefault(s => s.CallNumber == id);
            if (book == null)
            {
                ret = NotFound();
            }
            else
            {
                ret = Ok(book);
            }
            return ret;
        }

        [HttpPost]
        public IHttpActionResult Post(Book book)
        {
            IHttpActionResult ret = null;
            book = db.Books.Add(book);
            db.SaveChanges();
            if (book != null)
            {
                ret = Created<Book>(Request.RequestUri +
                     book.CallNumber.ToString(), book);
            }
            else
            {
                ret = NotFound();
            }
            return ret;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id,
                             Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.CallNumber)
            {
                return BadRequest();
            }
            db.Entry(book).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (BookBorrowed(id))
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

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            db.Books.Remove(book);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookBorrowed(id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = book.CallNumber }, book);

        }
        private bool BookBorrowed(int id)
        {
            return db.RentalDetails.Count(e => e.CallNumber == id) > 0;
        }
        [HttpGet]
        [Route("api/Books/PassSession")]
        public IHttpActionResult PassSession()
        {
            List<string> listSession = new List<string>();
            if (HttpContext.Current.Session["AdminName"] != null)
            {
                listSession.Add(HttpContext.Current.Session["AdminName"].ToString());                
            }
            return Ok(listSession);
        }
    }
}

