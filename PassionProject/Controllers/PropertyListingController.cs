using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Text;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PassionProject.Controllers
{
    public class PropertyListingController : Controller
    {
        private static HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        public PropertyListingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: PropertyListing
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            //string url = "user/property-listings";

            //TODO: Due to authuentication error getting all list
            string url = "property-listings";

            var token = HttpContext.Request.Params.GetValues("__RequestVerificationToken").FirstOrDefault();
            client.DefaultRequestHeaders.Add("RequestVerificationToken", token); 
            
            List<PropertyListingDto> list = new List<PropertyListingDto>();
            HttpResponseMessage response = await client.GetAsync(url);


            list = await response.Content.ReadAsAsync<List<PropertyListingDto>>();
            //IEnumerable<PropertyListingDto> list = response.Content.ReadAsAsync<IEnumerable<PropertyListingDto>>().Result;

            return View(list);
        }


        // POST: /PropertyListing/Create
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            PropertyListingDto property = new PropertyListingDto();
            return View(property);
        }



        // POST: /PropertyListing/Create
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PropertyListingDto model)
        {
            if (ModelState.IsValid)
            {

                string url = "property-listings";
                List<PropertyListingDto> list = new List<PropertyListingDto>();

                string jsonModel = jss.Serialize(model);

                //HttpContent content = new StringContent(jsonModel);
                //content.Headers.ContentType.MediaType = "application/json";
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /PropertyListing/Update/1
        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(string slug)
        {
            //property-listings/{slug}
            string url = "property-listings/" + slug;
            PropertyListingDto propertyDto = new PropertyListingDto();
            HttpResponseMessage response = await client.GetAsync(url);


            propertyDto = await response.Content.ReadAsAsync<PropertyListingDto>();

            //var data = JsonConvert.DeserializeObject<PropertyListingDto>((string)resultResponse.data);
            //if (resultResponse.data != null)
            //    propertyDto = (PropertyListingDto) resultResponse.data;


            return View(propertyDto);
        }


        // POST: /PropertyListing/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(PropertyListingDto model)
        {
            var token = HttpContext.Request.Params.GetValues("__RequestVerificationToken").FirstOrDefault();
            client.DefaultRequestHeaders.Add("RequestVerificationToken", token);

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                string url = "property-listings/" + model.Id;
                PropertyListingDto list = new PropertyListingDto();

                string jsonModel = jss.Serialize(model);

                //HttpContent content = new StringContent(jsonModel);
                //content.Headers.ContentType.MediaType = "application/json";
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // POST: /PropertyListing/Delete
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            string url = "property-listings/" + id;
            PropertyListingDto list = new PropertyListingDto();

            //string jsonModel = jss.Serialize(model);

            ////HttpContent content = new StringContent(jsonModel);
            ////content.Headers.ContentType.MediaType = "application/json";
            //var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View();
        }

    }
}

public class ResultResponse
{
    public string message { get; set; }
    public bool success { get; set; }
    public object data { get; set; }
    public object errors { get; set; }
}