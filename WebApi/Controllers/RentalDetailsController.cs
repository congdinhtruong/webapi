using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class RentalDetailsController : BaseController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<RentalDetail> list = new List<RentalDetail>();
            list = db.RentalDetails.ToList();
            return Ok(list.Select(s => new RentalDetail
            {
                CallNumber = s.CallNumber,
                StudentId = s.StudentId,
                DateRent = s.DateRent,
                DatePay = s.DatePay,
                LateFee = s.LateFee,
                IssueStatus = s.IssueStatus,
                CheckoutDate = s.CheckoutDate,
                FeePay = s.FeePay
            }));
        }
        [HttpGet]
        public IHttpActionResult Get(int id, int stuId)
        {
            IHttpActionResult ret = null;
            RentalDetail rentalDetail = new RentalDetail();
            rentalDetail = db.RentalDetails.SingleOrDefault(r => r.CallNumber == id && r.StudentId == stuId);
            if (rentalDetail == null)
            {
                ret = NotFound();
            }
            else
            {
                ret = Ok(rentalDetail);
            }
            return ret;
        }
        [HttpPost]        
        public IHttpActionResult Post(RentalDetail rentaldetail)
        {
            IHttpActionResult ret = null;
            rentaldetail = db.RentalDetails.Add(rentaldetail);
            db.SaveChanges();
            if (rentaldetail != null)   
            {
                ret = Created<RentalDetail>(Request.RequestUri +
                     rentaldetail.CallNumber.ToString() + rentaldetail.StudentId.ToString(), rentaldetail);
            }
            else
            {
                ret = NotFound();
            }
            return ret;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, int stuId,
                             RentalDetail rentaldetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rentaldetail.CallNumber && stuId != rentaldetail.StudentId)
            {
                return BadRequest();
            }
            db.Entry(rentaldetail).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        
    }
}
