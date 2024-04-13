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



namespace ridesnShare.Controllers
{
    public class RideDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of trips from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of TripDTO objects representing the list of trips.
        /// </returns>
        /// <example>
        /// GET: api/TripData/ListTrips
        /// </example>
        //[ResponseType(typeof(List<Ride>))]
        [HttpGet]
        [Route("api/RideData/ListRides/{id}")]
        public IEnumerable<RideDTO> Rides(int id)
        {
            IEnumerable<Ride> Rides = db.Rides.Where(r=>r.DriverId==id);
            IEnumerable<RideDTO> rideDTOs = Rides.Select(r => new RideDTO()
            {
                //RideId = r.RideId,
                DriverId = r.DriverId,
                StartLocation = r.startLocation,
                EndLocation = r.endLocation,
                Price = r.price,
                Time = r.Time,
                DayOftheweek = r.dayOftheweek,
                LuggageQuantity = r.LuggageQuantity,
                LuggageWeight = r.LuggageWeight,
                LuggageSize = r.LuggageSize,
                BagQuantity = r.BagQuantity,
                BagSize = r.BagSize,
                BagWeight = r.BagWeight
            });
           
            return rideDTOs;

        }
        /// <summary>
        /// Adds a new ride to the database.
        /// </summary>
        /// <param name="ride">The ride object containing information about the new ride.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/RideData/AddRide/5
        /// </example>
        [HttpGet]
        [Route("api/RideData/GetDriver/{id}")]

        public Driver GetDriver(int id)
        {
            return db.Drivers.FirstOrDefault(d=> d.DriverId == id);
        }    
        [ResponseType(typeof(Ride))]
        [HttpPost]
        public Ride AddRide(Ride ride)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            // Find the driver associated with the ride (assuming RideId is actually DriverId)
            Driver driver = db.Drivers.Find(ride.DriverId);

            if (driver == null)
            {
                Debug.WriteLine("Driver doesn't exist");
                //return BadRequest("Driver not found");
                return null;
            }

            // Create an instance of Inventory
            //Inventory inventory = new Inventory
            //{
            //    ItemName = ride.ItemName,
            //    Weight = ride.Weight,
            //    Size = ride.Size,
            //    Quantity = ride.Quantity
            //};

            //// Add the inventory to the database
            //db.Inventories.Add(inventory);
            //db.SaveChanges();

            // Create an instance of Ride
            //Ride newRide = new Ride
            //{
            //    StartLocation = ride.StartLocation,
            //    EndLocation = ride.EndLocation,
            //    Price = ride.Price,
            //    Time = ride.Time,
            //    DayOfTheWeek = ride.DayOfTheWeek,
            //};

            // Add the ride to the database
            db.Rides.Add(ride);
            db.SaveChanges();

            return ride;
        }

        private bool RideExists(int id)
        {
            return db.Rides.Count(r => r.RideId == id) > 0;
        }

        /// <summary>
        /// Retrieves information about a specific ride from the database.
        /// </summary>
        /// <param name="id">The ID of the ride to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the ride.
        /// </returns>
        /// <example>
        /// GET: api/RideData/FindRide/{id}
        /// </example>

        [ResponseType(typeof(Ride))]
        [HttpGet]
        [Route("api/RideData/FindRide/{id}")]
        public IHttpActionResult FindRide(int id)
        {
            Ride ride = db.Rides.Find(id);
            RideDTO rideDTO = new RideDTO()
            {
                DriverId = ride.DriverId,
                StartLocation = ride.startLocation,
                EndLocation = ride.endLocation,
                Price = ride.price,
                Time = ride.Time,
                DayOftheweek = ride.dayOftheweek,
                LuggageQuantity = ride.LuggageQuantity,
                LuggageWeight = ride.LuggageWeight,
                LuggageSize = ride.LuggageSize,
                BagQuantity = ride.BagQuantity,
                BagSize = ride.BagSize,
                BagWeight = ride.BagWeight
            };

            if (ride == null)
            {
                return NotFound();
            }

            return Ok(rideDTO);
        }
        /// <summary>
        /// Updates information about a specific passenger in the database.
        /// </summary>
        /// <param name="id">The ID of the passenger to update.</param>
        /// <param name="passenger">The updated information of the passenger.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation.
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/UpdatePassenger/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/RideData/UpdateRide/{id}")]
        public IHttpActionResult UpdateRide(int id, RideDTO rideDTO)
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
                Debug.WriteLine("POST parameter" + rideDTO.StartLocation);
                Debug.WriteLine("POST parameter" + rideDTO.EndLocation);
                Debug.WriteLine("POST parameter" + rideDTO.Price);
                return BadRequest();
            }

            db.Entry(rideDTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideExists(id))
                {
                    Debug.WriteLine("Ride not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
