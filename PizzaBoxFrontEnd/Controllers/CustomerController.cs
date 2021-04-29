using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PizzaBox.Domain.Entities;
using PizzaBoxFrontEnd.Models;

namespace PizzaBoxFrontEnd.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View(new Customer());
        }
        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Customer";
            string customerJson = JsonConvert.SerializeObject(customer);
            HttpContent httpContent = new StringContent(customerJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Customer was added successfully";
            }
            else
            {
                ViewBag.Message = "ERROR: Please try again!";
            }
            return AddCustomer();
        }
        private List<Store> GetStores()
        {
            List<Store> stores = new List<Store>();
            var client = new HttpClient();
            string url = "http://localhost:5002/Store";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                stores = JsonConvert.DeserializeObject<List<Store>>(responsebody);
            }
            return stores;

        }
        private List<Order> GetStoreOrders(int storeId)
        {
            List<Order> orders = new List<Order>(); //Call API to get the list
            using (var client = new HttpClient())
            {
                string url = $"http://localhost:5002/StoreOrder?id={storeId}";
                var response = client.GetAsync(url);

                if (response.Result.IsSuccessStatusCode)
                {
                    var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                    orders = JsonConvert.DeserializeObject<List<Order>>(responsebody);
                }
            }
            return orders;
        }

        [HttpGet]
        public IActionResult ViewCustomerOrders()
        {
            List<Customer> customers = GetCustomers();
            ViewBag.Customers = customers.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.CustomerName
            }).ToList();
            ViewCustomerOrdersViewModel viewCustomerOrdersViewModel = new ViewCustomerOrdersViewModel();
            if (ViewBag.CustomerId != null)
            {
                viewCustomerOrdersViewModel.CustomerId = ViewBag.CustomerId.ToString();
            }
            return View(viewCustomerOrdersViewModel);
        }

        private List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            var client = new HttpClient();
            string url = "http://localhost:5002/Customer";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                customers = JsonConvert.DeserializeObject<List<Customer>>(responsebody);
            }
            return customers;
        }

        [HttpPost]
        public IActionResult ViewCustomerOrders(ViewCustomerOrdersViewModel viewCustomerOrdersViewModel)
        {
            int customerId = int.Parse(viewCustomerOrdersViewModel.CustomerId);
            var orders = GetCustomerOrders(customerId);
            ViewBag.CustomerId = customerId;
            ViewBag.CustomerOrders = orders;
            return ViewCustomerOrders();
        }

        private List<Order> GetCustomerOrders(int customerId)
        {
            List<Order> orders = new List<Order>();
            var client = new HttpClient();
            string url = $"http://localhost:5002/CustomerOrder?id={customerId}";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<Order>>(responsebody);
            }
            return orders;
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
