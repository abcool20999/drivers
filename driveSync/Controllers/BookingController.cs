using driveSync.Controllers;
using driveSync.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace driveSync.Controllers
{
    public class BookingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/BookingData/");
        }
        private readonly BookingDataController _bookingdatacontroller;
        public BookingController()
        {
            _bookingdatacontroller = new BookingDataController();
        }
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }
        // GET: Booking/List
        public ActionResult List()
        {
            //objective is to communicate with my Booking data api to retrieve a list of bookings.
            //curl https://localhost:44354/api/BookingData/ListBookings


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "ListBookings";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables us see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type booking.
            IEnumerable<BookingDTO> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDTO>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of bookings received");
            Debug.WriteLine(bookings.Count());
            //this shows the channel of comm btwn our webserver in our passenger controller and the actual passenger data controller api as we are communicating through an http request

            return View(bookings);
        }

        // GET: Booking/Details/1
        public ActionResult Details(int id)
        {
            //objective is to communicate with my Booking data api to retrieve one passenger booking.
            //curl https://localhost:44354/api/BookingData/FindBooking/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables me see if my httpclient is communicating with the data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type booking.
            BookingDTO selectedbooking = response.Content.ReadAsAsync<BookingDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("booking received");
            Debug.WriteLine(selectedbooking.passengerFirstName);
            //this shows the channel of comm btwn our webserver in our booking controller and the actual booking data controller api as we are communicating through an http request

            return View(selectedbooking);
        }


        // GET: Booking/Edit/1
        public ActionResult Edit(int id)
        {
            // Construct the URL to find the passengerbooking with the given ID
            string url = "FindBooking/" + id;

            // Send a GET request to retrieve the passenger booking information from the API
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the response content into a BookingDTO object
            BookingDTO selectedbooking = response.Content.ReadAsAsync<BookingDTO>().Result;

            // Return the View with the selected booking data
            return View("EditBooking", selectedbooking);
        }


        // GET: Booking/Edit/1
        public ActionResult EditBooking(BookingDTO bookingDTO)
        {


            // Return the View with the selected booking data
            return View(bookingDTO);
        }

        // POST: Booking/Update/1
        [HttpPost]
        public ActionResult Update(BookingDTO bookingDTO)
        {
            int id = bookingDTO.BookingId;
            // Set the booking ID to match the ID in the route


            // Construct the URL to update the passenger booking with the given ID
            string url = "UpdateBooking/" + id;

            // Serialize the passenger booking object into JSON payload
            string jsonpayload = jss.Serialize(bookingDTO);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the booking information
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

        [HttpPost]
        public ActionResult CreateBooking(Booking booking)
        {
            try
            {
                // TODO: Add insert logic here
                var response = _bookingdatacontroller.PostBooking(booking);

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }
        // GET: Booking/Edit/5
        public ActionResult Edits(int id)
        {
            return View();
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Booking booking)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDTO selectedbooking = response.Content.ReadAsAsync<BookingDTO>().Result;
            return View(selectedbooking);

        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteBooking/" + id;
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
    }

}