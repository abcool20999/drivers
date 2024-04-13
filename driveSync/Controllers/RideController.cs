using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using driveSync.Models;
using System.Web.Script.Serialization;
using System.Net.NetworkInformation;
using System.Data.Entity.Migrations.Model;
using System.Security.Policy;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace driveSync.Controllers
{
    public class RideController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RideController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44332/api/RideData/");
        }
        // GET: Ride/List
        public ActionResult List(Ride ride)
        {
            try
            {
                // Establish url connection endpoint
                string url = "ListRides/"+ride.DriverId;

                // Send request to API to retrieve list of rides
                var response = client.GetAsync(url).Result;

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON response into a list of RideDTO objects
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<RideDTO> rides = jss.Deserialize<IEnumerable<RideDTO>>(responseData);

                    // Debug info
                    Debug.WriteLine("Number of rides received: " + rides.Count());

                    // Return the view with the list of rides
                    return View(rides);
                }
                else
                {
                    Debug.WriteLine("API request failed with status code: " + response.StatusCode);
                    // Handle unsuccessful response (e.g., return an error view)
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
                // Handle any exceptions (e.g., return an error view)
                return View("Error");
            }
        }
    


// GET: Ride/Details/5
public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ride/Add
        //[Route("/Add/:driverId")]
        public ActionResult Add(int id)
        {
            HttpResponseMessage response = client.GetAsync("GetDriver/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                var driver=JsonConvert.DeserializeObject<Driver>(response.Content.ReadAsStringAsync().Result);
                return View("Add", driver);
            }
            else
            {
                return RedirectToAction("Errors");
            }
            
           

        }

        // POST: Ride/AddRide
        [HttpPost]
        public ActionResult AddRide(Ride ride)
        {
            Debug.WriteLine("the inputted trip name is :");
            Debug.WriteLine(ride.Price);
            //objective: add a new trip into our system using the API
            //curl -H "Content-Type:application/json" -d @trip.json  https://localhost:44354/api/RideData/AddRide

            string url = "AddRide";

            var rideentity = new Ride
            {
                DriverId = ride.DriverId,
                startLocation = ride.startLocation,
                endLocation = ride.endLocation,
                price = ride.price,
                Time = ride.Time,
                dayOftheweek = ride.dayOftheweek,
                BagQuantity = ride.BagQuantity,
                BagSize = ride.BagSize,
                BagWeight = ride.BagWeight,
                LuggageQuantity = ride.LuggageQuantity,
                LuggageSize = ride.LuggageSize,
                LuggageWeight = ride.LuggageWeight,

            };

            //var inventoryentity = new List<Inventory>()
            //{
            //    new Inventory
            //    {
            //        ItemName = "Bag",
            //        Quantity = ride.BagQuantity,
            //        Size = ride.BagSize,
            //        Weight = ride.BagWeight
            //    },

            //    new Inventory
            //    {
            //        ItemName = "Luggage",
            //        Quantity = ride.LuggageQuantity,
            //        Size = ride.LuggageSize,
            //        Weight = ride.LuggageWeight
            //    }
            //};
            
            //convert ride object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(rideentity);

            Debug.WriteLine(jsonpayload);

            //send the json payload to the url through the use of our client
            //setup the postdata as HttpContent variable content
            HttpContent content = new StringContent(jsonpayload);

            //configure a header for our client to specify the content type of app for post 
            content.Headers.ContentType.MediaType = "application/json";

            //check if you can access information from our postasync request, get an httpresponse request and result of the request

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //if (response.IsSuccessStatusCode)
            //{
                //var rid = JsonConvert.DeserializeObject<Ride>(response.Content.ReadAsStringAsync().Result);

                return RedirectToAction("List", rideentity);
            //}
            //else
            //{
            //    return RedirectToAction("Errors");
            //}
        }

        // GET: Ride/Edit/5
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Edit(int id)
        {
            // Retrieve the ride from the database
            Ride ride = db.Rides.Find(id);

            // Check if the ride exists
            if (ride == null)
            {
                return HttpNotFound(); // Return 404 if ride is not found
            }

            // Map Ride model to RideDTO
            RideDTO rideDTO = new RideDTO
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

            // Pass the RideDTO to the view
            return View(rideDTO);
        }

        // POST: Ride/Update/1
        [HttpPost]
        public ActionResult Update(int id, RideDTO rideDTO)
        {
            // Set the driver ID to match the ID in the route
            rideDTO.RideId = id;

            // Construct the URL to update the ride with the given ID
            string url = "UpdateRide/" + id;

            // Serialize the ride object into JSON payload
            string jsonpayload = jss.Serialize(rideDTO);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the ride information
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Log the content of the request
            Debug.WriteLine(content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Redirect to the List action if the update was successful
                return RedirectToAction("List");
            }
            else
            {
                // Redirect to the Error action if there was an error during the update
                return RedirectToAction("Error");
            }
        }

        // GET: Ride/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ride/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
