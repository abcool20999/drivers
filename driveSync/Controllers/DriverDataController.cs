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
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of drivers from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of DriverDTO objects representing the list of drivers.
        /// </returns>
        /// <example>
        /// GET: api/DriverData/ListDrivers
        /// </example>
        [HttpGet]
        [Route("api/DriverData/ListDrivers")]
        public IEnumerable<DriverDTO> Drivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDTO> DriverDTOs = new List<DriverDTO>();

            Drivers.ForEach(d => DriverDTOs.Add(new DriverDTO()
            {
                DriverId = d.DriverId,
                firstName = d.firstName,
                lastName = d.lastName,
                username = d.username,
                email = d.email,
                Age = d.Age,
                CarType = d.CarType
            }));

            return DriverDTOs;

        }
        /// <summary>
        /// Validates a passenger's credentials by checking if the user exists in the database and if the provided password matches.
        /// </summary>
        /// <param name="passenger">The Passenger object containing username and password for validation.</param>
        /// <returns>
        /// IHttpActionResult representing the result of the validation process:
        ///   - If the user exists and the password matches, returns Ok with the validated Passenger object.
        ///   - If the user exists but the password does not match, returns BadRequest with a message indicating incorrect password.
        ///   - If the user does not exist, returns BadRequest with a message indicating that the user was not found.
        /// </returns>
        /// <example></example>

        [HttpPost]
        [Route("api/DriverData/Validate")]
        public IHttpActionResult Validate(Driver driver)
        {
            Debug.WriteLine(driver.username);
            Debug.WriteLine(driver.password);

            // returns true or false if user exist or not

            bool isUserExist = (db.Drivers.Where(p => p.username == driver.username)
                                   .FirstOrDefault() == null) ? false : true;

            // Debug.WriteLine(isUserExist + "isUserExists");

            if (isUserExist)
            {
                // validate user
                Driver validatedDriver = db.Drivers.Where(d => d.username == d.username)
                                               .Where(d => d.password == d.password).FirstOrDefault();
                if (validatedDriver != null)
                {
                    return Ok(validatedDriver);

                }

                return BadRequest("Wrong password");

            }
            else
            {
                // return with a message
                return BadRequest("User not found");
            }
        }

        // <summary>
        /// Adds a new driver to the database.
        /// </summary>
        /// <param name="driver">The driver object containing information about the new driver.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/AddDriver
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = driver.DriverId }, driver);
        }

        /// <summary>
        /// Retrieves information about a specific driver from the database.
        /// </summary>
        /// <param name="id">The ID of the driver to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the driver.
        /// </returns>
        /// <example>
        /// GET: api/DriverData/FindDriver/{id}
        /// </example>

        [ResponseType(typeof(Driver))]
        [HttpGet]
        [Route("api/DriverData/FindDriver/{id}")]
        public IHttpActionResult FindDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            DriverDTO driverDTO = new DriverDTO()
            {
                DriverId = driver.DriverId,
                firstName = driver.firstName,
                lastName = driver.lastName,
                email = driver.email,
                username = driver.username,
                Age = driver.Age,
                CarType = driver.CarType
            };

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driverDTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriverId == id) > 0;
        }

        /// <summary>
        /// Updates information about a specific driver in the database.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="driver">The updated information of the driver.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/UpdateDriver/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/DriverData/UpdateDriver/{id}")]
        public IHttpActionResult UpdateDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id == default)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + driver.DriverId);
                Debug.WriteLine("POST parameter" + driver.firstName);
                Debug.WriteLine("POST parameter" + driver.lastName);
                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    Debug.WriteLine("Driver not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes a driver from the database.
        /// </summary>
        /// <param name="id">The ID of the driver to delete.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the deletion operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/DeleteDriver/5
        /// </example>

        [ResponseType(typeof(Driver))]
        [HttpPost]
        [Route("api/DriverData/DeleteDriver/{id}")]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            db.SaveChanges();

            return Ok();
        }
    }
}

