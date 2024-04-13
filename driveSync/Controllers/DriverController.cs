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

namespace driveSync.Controllers
{
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44332/api/DriverData/");
        }

        public ActionResult DriverLoginSubmit(Driver driver)
        {
            Debug.WriteLine(driver.username);
            Debug.WriteLine(driver.password);

            string url = "Validate";
            string jsonpayload = jss.Serialize(driver);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                //if (response.IsSuccessStatusCode)
                {
                    Driver resUser = response.Content.ReadAsAsync<Driver>().Result;

                    //Session.Clear();
                    //Session["userId"] = resUser;
                    //var action = $"PassengerProfile";
                    //return RedirectToAction(action, "Passenger");

                    return RedirectToAction("DriverProfile", "Driver", resUser);

                }
                //else
                //{
                //    Debug.WriteLine("Unsuccessful login attempt.");
                //    return RedirectToAction("Index", "Home"); // Redirect to home page if login fails
                //}
            }
            catch (Exception ex)
            {
                // Log the exception details
                Debug.WriteLine("An error occurred during login: " + ex.Message);
                // Redirect to an error page or handle the error appropriately
                return RedirectToAction("Error", "Driver");
            }
        }

        public ActionResult DriverProfile(Driver driver)
        {


            // Pass user object to the view
            return View(driver);
        }

        //GET: Driver/List
        public ActionResult List()
        {
            try
            {
                // Establish url connection endpoint
                string url = "ListDrivers";

                // Send request to API to retrieve list of drivers
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON response into a list of DriverDTO objects
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<DriverDTO> drivers = jss.Deserialize<IEnumerable<DriverDTO>>(responseData);

                    // Debug info
                    Debug.WriteLine("Number of rides received: " + drivers.Count());

                    // Return the view with the list of drivers
                    return View(drivers);
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

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            //objective is to communicate with my Driver data api to retrieve one driver.
            //curl https://localhost:44332/api/DriverData/FindDriver/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables me see if my httpclient is communicating with the data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type driver.
            DriverDTO selecteddriver = response.Content.ReadAsAsync<DriverDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("driver received");
            Debug.WriteLine(selecteddriver.firstName);
            //this shows the channel of comm btwn our webserver in our driver controller and the actual driver data controller api as we are communicating through an http request

            return View(selecteddriver);
        }

        // GET: Driver/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Driver/Create
        [HttpPost]
        public ActionResult AddDriver(Driver driver)
        {
            Debug.WriteLine("the inputted driver name is :");
            Debug.WriteLine(driver.firstName);
            //objective: add a new driver into our system using the API
            //curl -H "Content-Type:application/json" -d @trip.json  https://localhost:44354/api/DriverData/AddDriver

            string url = "AddDriver";

            //convert driver object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(driver);

            Debug.WriteLine(jsonpayload);

            //send the json payload to the url through the use of our client
            //setup the postdata as HttpContent variable content
            HttpContent content = new StringContent(jsonpayload);

            //configure a header for our client to specify the content type of app for post 
            content.Headers.ContentType.MediaType = "application/json";

            //check if you can access information from our postasync request, get an httpresponse request and result of the request

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }

        }
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieve the driver from the database
            Driver driver = db.Drivers.Find(id);

            // Check if the driver exists
            if (driver == null)
            {
                return HttpNotFound(); // Return 404 if driver is not found
            }

            // Map Driver model to DriverDTO
            DriverDTO driverDTO = new DriverDTO
            {
                DriverId = driver.DriverId,
                username = driver.username,
                firstName = driver.firstName,
                lastName = driver.lastName,
                email = driver.email,
                Age = driver.Age,
                CarType = driver.CarType
            };

            // Pass the DeiverDTO to the view
            return View(driverDTO);
        }

        // POST: Driver/Update/1
        [HttpPost]
        public ActionResult Update(int id, DriverDTO driverDTO)
        {
            // Set the driver ID to match the ID in the route
            driverDTO.DriverId = id;

            // Construct the URL to update the driver with the given ID
            string url = "UpdateDriver/" + id;

            // Serialize the driver object into JSON payload
            string jsonpayload = jss.Serialize(driverDTO);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the driver information
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

        // GET: Driver/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDTO selecteddriver = response.Content.ReadAsAsync<DriverDTO>().Result;
            return View(selecteddriver);

        }

        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteDriver/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // POST: Passenger/Login
        [HttpGet]
        public ActionResult DriverLogin()
        {

            return View("DriverLogin");

        }

    }
}
