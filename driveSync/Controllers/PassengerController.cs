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
using Newtonsoft.Json;

namespace driveSync.Controllers
{
    public class PassengerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PassengerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44332/api/PassengerData/");
        }

        public ActionResult PassengerLoginSubmit(Passenger passenger)
        {
            Debug.WriteLine(passenger.username);
            Debug.WriteLine(passenger.password);

            string url = "Validate";
            string jsonpayload = jss.Serialize(passenger);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                //if (response.IsSuccessStatusCode)
                {
                    Passenger resUser = response.Content.ReadAsAsync<Passenger>().Result;

                    //Session.Clear();
                    //Session["userId"] = resUser;
                    //var action = $"PassengerProfile";
                    //return RedirectToAction(action, "Passenger");

                    return RedirectToAction("PassengerProfile", "Passenger");

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
                return RedirectToAction("Error", "Passenger");
            }
        }

        public ActionResult List()
        {
            try
            {
                // Establish url connection endpoint
                string url = "ListPassengers";

                // Send request to API to retrieve list of passengers
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON response into a list of PassengerDTO objects
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<PassengerDTO> passengers = jss.Deserialize<IEnumerable<PassengerDTO>>(responseData);

                    // Debug info
                    Debug.WriteLine("Number of rides received: " + passengers.Count());

                    // Return the view with the list of rides
                    return View(passengers);
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

        public ActionResult PassengerProfile()
        {


            // Pass user object to the view
            return View();
        }


        // GET: Passenger/Details/5
        public ActionResult Details(int id)
        {
            //objective is to communicate with my Passenger data api to retrieve one passenger.
            //curl https://localhost:44332/api/PassengerData/FindPassenger/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindPassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables me see if my httpclient is communicating with the data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type passenger.
            PassengerDTO selectedpassenger = response.Content.ReadAsAsync<PassengerDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("passenger received");
            Debug.WriteLine(selectedpassenger.firstName);
            //this shows the channel of comm btwn our webserver in our passenger controller and the actual passenger data controller api as we are communicating through an http request

            return View(selectedpassenger);
        }

        // GET: Passenger/Add
        public ActionResult Add()
        {
              return View();     
           
        }

        // POST: Passenger/Create
        [HttpPost]

        public ActionResult AddPassenger(Passenger passenger)
        {
            Debug.WriteLine("the inputted passenger name is :");
            Debug.WriteLine(passenger.firstName);
            //objective: add a new passenger into our system using the API
            //curl -H "Content-Type:application/json" -d @trip.json  https://localhost:44354/api/PassengerData/AddPassenger

            string url = "AddPassenger";

            //convert passenger object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(passenger);

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
        // GET: Passenger/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieve the passenger from the database
            Passenger passenger = db.Passengers.Find(id);

            // Check if the passenger exists
            if (passenger == null)
            {
                return HttpNotFound(); // Return 404 if passenger is not found
            }

            // Map Passenger model to PassengerDTO
            PassengerDTO passengerDTO = new PassengerDTO
            {
                PassengerId = passenger.PassengerId,
                firstName = passenger.firstName,
                lastName = passenger.lastName,
                email = passenger.email
            };

            // Pass the PassengerDTO to the view
            return View(passengerDTO);
        }

        // POST: Passenger/Update/1
        [HttpPost]
        public ActionResult Update(int id, PassengerDTO passengerDTO)
        {
            // Set the passenger ID to match the ID in the route
            passengerDTO.PassengerId = id;

            // Construct the URL to update the passenger with the given ID
            string url = "UpdatePassenger/" + id;

            // Serialize the passenger object into JSON payload
            string jsonpayload = jss.Serialize(passengerDTO);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the passenger information
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

        // GET: Passenger/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindPassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassengerDTO selectedpassenger = response.Content.ReadAsAsync<PassengerDTO>().Result;
            return View(selectedpassenger);

        }

        // POST: Passenger/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeletePassenger/" + id;
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
        public ActionResult PassengerLogin()
        {
            
           return View("PassengerLogin");
          
        }

  
    }
}
