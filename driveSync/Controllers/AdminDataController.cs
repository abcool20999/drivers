using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using driveSync.Models;

namespace driveSync.Controllers
{

    public class AdminDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Validates a admin's credentials by checking if the user exists in the database and if the provided password matches.
        /// </summary>
        /// <param name="admin">The Passenger object containing username and password for validation.</param>
        /// <returns>
        /// IHttpActionResult representing the result of the validation process:
        ///   - If the user exists and the password matches, returns Ok with the validated Passenger object.
        ///   - If the user exists but the password does not match, returns BadRequest with a message indicating incorrect password.
        ///   - If the user does not exist, returns BadRequest with a message indicating that the user was not found.
        /// </returns>
        /// <example></example>

        [HttpPost]
        [Route("api/AdminData/Validate")]
        public IHttpActionResult Validate(Admin admin)
        {
            Debug.WriteLine(admin.username);
            Debug.WriteLine(admin.password);

            // returns true or false if user exist or not

            bool isUserExist = (db.Admins.Where(a => a.username == admin.username)
                                   .FirstOrDefault() == null) ? false : true;

            // Debug.WriteLine(isUserExist + "isUserExists");

            if (isUserExist)
            {
                // validate user
                Admin validatedAdmin = db.Admins.Where(a => a.username == a.username)
                                               .Where(a => a.password == a.password).FirstOrDefault();
                if (validatedAdmin != null)
                {
                    return Ok(validatedAdmin);

                }

                return BadRequest("Wrong password");

            }
            else
            {
                // return with a message
                return BadRequest("User not found");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PassengerExists(int id)
        {
            return db.Passengers.Count(e => e.PassengerId == id) > 0;
        }
    }
}
