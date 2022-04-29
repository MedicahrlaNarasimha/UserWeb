using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserWebApp.Models;

namespace UserWebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// List of Users
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var _client = new HttpClient();
                var content = Task.Run(() => _client.GetAsync("http://localhost:45674/api/User")).Result;
                if (content.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<List<User>>(content.Content.ReadAsStringAsync().Result);
                    return View(response);
                }
                else
                    return View("No Data Found...");
            }
            catch (Exception ex)
            {
                return Content("No Data Found...");
            }
           
        }
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Adding New User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var _client = new HttpClient();
            var request = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(request, UnicodeEncoding.UTF8, "application/json");
            var response = Task.Run(() => _client.PostAsync("http://localhost:45674/api/User", stringContent)).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
