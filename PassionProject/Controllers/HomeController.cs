using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PassionProject.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient client;

        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        // GET: Visitor
        public async Task<ActionResult> Index()
        {
            string url = "property-listings";
            List<PropertyListingDto> list = new List<PropertyListingDto>();
            HttpResponseMessage response = await client.GetAsync(url);


            list = await response.Content.ReadAsAsync<List<PropertyListingDto>>();
            //IEnumerable<PropertyListingDto> list = response.Content.ReadAsAsync<IEnumerable<PropertyListingDto>>().Result;

            return View(list);
        }


        public async Task<ActionResult> Details(string slug)
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}