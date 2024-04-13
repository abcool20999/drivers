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
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of bookings from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of BookingDTO objects representing the list of bookings.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/ListBookings
        /// </example>
        [HttpGet]
        [Route("api/BookingData/ListBookings")]
        public IEnumerable<BookingDTO> ListBookings()
        {
            List<Booking> bookings = db.Bookings.ToList();
            List<BookingDTO> BookingDTOs = new List<BookingDTO>();
           
          foreach (Booking booking in bookings)
                {
                    var passenger = db.Passengers.FirstOrDefault(p => p.PassengerId == booking.PassengerId);
                    BookingDTOs.Add(new BookingDTO()
                    {
                        BookingId = booking.BookingId,
                        passengerFirstName = booking.Passenger.firstName,
                        driverFirstName = booking.Ride.Driver.firstName,
                        startLocation = booking.Ride.startLocation,
                        endLocation = booking.Ride.endLocation,
                        price = booking.Ride.price,
                        Time = booking.Ride.Time,
                        dayOftheweek = booking.Ride.dayOftheweek,
                        LuggageWeight = booking.Ride.LuggageWeight,
                        LuggageSize = booking.Ride.LuggageSize,
                        LuggageQuantity = booking.Ride.LuggageQuantity,
                        BagWeight = booking.Ride.BagWeight,
                        BagSize = booking.Ride.BagSize,
                        BagQuantity = booking.Ride.BagQuantity
                    });
                
            }

            return BookingDTOs;

        }
        /// <summary>
        /// Retrieves information about a specific booking from the database.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the booking.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/FindBooking/{id}
        /// </example>

        [ResponseType(typeof(Booking))]
        [HttpGet]
        [Route("api/BookingData/FindBooking/{id}")]
        public IHttpActionResult FindBooking(int id)
        {
            Booking booking = db.Bookings.Include(b => b.Ride).FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            BookingDTO bookingDTO = new BookingDTO()
            {
                BookingId = booking.BookingId,
                PassengerId = booking.PassengerId,
                DriverId = booking.Ride.DriverId,
                RideId = booking.RideId,
                passengerFirstName = booking.Passenger.firstName,
                driverFirstName = booking.Ride.Driver.firstName,
                startLocation = booking.Ride.startLocation,
                endLocation = booking.Ride.endLocation,
                price = booking.Ride.price,
                Time = booking.Ride.Time,
                dayOftheweek = booking.Ride.dayOftheweek,
                LuggageWeight = booking.Ride.LuggageWeight,
                LuggageSize = booking.Ride.LuggageSize,
                LuggageQuantity = booking.Ride.LuggageQuantity,
                BagWeight = booking.Ride.BagWeight,
                BagSize = booking.Ride.BagSize,
                BagQuantity = booking.Ride.BagQuantity
            };



            return Ok(bookingDTO);
        }

        /// <summary>
        /// Updates information about a specific booking in the database.
        /// </summary>
        /// <param name="id">The ID of the booking to be updated.</param>
        /// <param name="bookingDTO">The updated information of the booking.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation:
        ///   - If the ModelState is not valid, returns BadRequest with ModelState errors.
        ///   - If the provided ID is default, returns BadRequest indicating ID mismatch.
        ///   - If the booking is successfully updated, returns NoContent status code.
        ///   - If the booking is not found, returns NotFound status code.
        /// </returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/BookingData/UpdateBooking/{id}")]
        public IHttpActionResult UpdateBooking(int id, BookingDTO bookingDTO)
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
                Debug.WriteLine("POST parameter" + bookingDTO.Time);
                Debug.WriteLine("POST parameter" + bookingDTO.dayOftheweek);
                Debug.WriteLine("POST parameter" + bookingDTO.endLocation);
                return BadRequest();
            }
            var passenger = db.Passengers.FirstOrDefault(x => x.PassengerId == bookingDTO.PassengerId);
            var driver = db.Drivers.FirstOrDefault(x => x.DriverId == bookingDTO.DriverId);
            var ride = db.Rides.FirstOrDefault(x => x.RideId == bookingDTO.RideId);
            var booking = db.Bookings.FirstOrDefault(x => x.BookingId == bookingDTO.BookingId);

            passenger.firstName = bookingDTO.passengerFirstName;
            driver.firstName = bookingDTO.driverFirstName;
            ride.startLocation = bookingDTO.startLocation;
            ride.endLocation = bookingDTO.endLocation;
            ride.price = bookingDTO.price;
            ride.Time = bookingDTO.Time;
            ride.dayOftheweek = bookingDTO.dayOftheweek;
            ride.LuggageWeight = booking.Ride.LuggageWeight;
            ride.LuggageSize = booking.Ride.LuggageSize;
            ride.LuggageQuantity = booking.Ride.LuggageQuantity;
            ride.BagWeight = booking.Ride.BagWeight;
            ride.BagSize = booking.Ride.BagSize;
            ride.BagQuantity = booking.Ride.BagQuantity;

            db.Entry(passenger).State = EntityState.Modified;
            db.Entry(driver).State = EntityState.Modified;
            db.Entry(ride).State = EntityState.Modified;
            db.Entry(booking).State = EntityState.Modified;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    Debug.WriteLine("Booking not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BookingData
        [ResponseType(typeof(Booking))]
        public IHttpActionResult PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingId }, booking);
        }

        /// <summary>
        /// Deletes a booking from the database.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the deletion operation.
        /// </returns>
        /// <example>
        /// POST: api/BookingData/DeleteBooking/5
        /// </example>

        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/DeleteBooking/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingId == id) > 0;
        }
    }
}