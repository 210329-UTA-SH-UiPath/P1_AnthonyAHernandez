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
using PizzaBox.Client.Singletons;
using PizzaBox.Domain.Entities;
using PizzaBoxFrontEnd.Models;

namespace PizzaBoxFrontEnd.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult ViewStoreOrders()
        {
            List<Store> stores = GetStores();
            ViewBag.Stores = stores.Select(s => new SelectListItem()
            {
                Value=s.Id.ToString(),
                Text=s.Name
            }).ToList();
            ViewStoreOrdersViewModel viewStoreOrdersViewModel = new ViewStoreOrdersViewModel();
            if (ViewBag.StoreId != null)
            {
                viewStoreOrdersViewModel.StoreId = ViewBag.StoreId.ToString();
            }
            return View(viewStoreOrdersViewModel);
        }
        [HttpPost] 
        public IActionResult ViewStoreOrders(ViewStoreOrdersViewModel viewStoreOrdersViewModel)
        {
            int storeId = int.Parse(viewStoreOrdersViewModel.StoreId);
            var orders = GetStoreOrders(storeId);
            ViewBag.StoreId = storeId;
            ViewBag.StoreOrders = orders;
            return ViewStoreOrders();
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
        [HttpGet]

        public IActionResult UpdatePizza(int id)
        {
            var pizza = GetPizza(id);
            var sizes = GetSize();
            ViewBag.Sizes = sizes.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            var crusts = GetCrust();
            ViewBag.Crusts = crusts.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            return View(pizza);
        }

        [HttpPost]

        public IActionResult UpdatePizza(Pizza pizza)
        {
            if (pizza.Quantity <= 0)
            {
                ViewBag.ErrorMessage = "ERROR Please select at least 1 pizza for the order.";
                return UpdatePizza(pizza.Id);
            }
            var existingPizza = GetPizza(pizza.Id);
            pizza.PizzaToppings = existingPizza.PizzaToppings;
            if(pizza.PizzaToppings.Count < 2 || pizza.PizzaToppings.Count > 5)
            {
                ViewBag.ErrorMessage = "ERROR Please select at least 2 and no more than 5 toppings.";
                return UpdatePizza(pizza.Id);
            }

            UpdatePizzaApi(pizza);
            return RedirectToAction("ConfirmOrder", new { id = pizza.OrderId });
        }

        private Pizza UpdatePizzaApi(Pizza pizza)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Pizza";
            string pizzaJson = JsonConvert.SerializeObject(pizza);
            HttpContent httpContent = new StringContent(pizzaJson, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Pizza>(responsebody);
            }
            else
            {
                return null;
            }
        }

        private List<Size> GetSize()
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Size";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Size>>(responsebody);
            }
            else
            {
                return null;
            }
        }

        private List<Crust> GetCrust()
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Crust";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Crust>>(responsebody);
            }
            else
            {
                return null;
            }
        }

        private Pizza GetPizza(int id)
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Pizza?id={id}";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Pizza>(responsebody);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]

        public IActionResult AddPizzaTopping(int id)
        {
            var pizzaTopping = new PizzaTopping();
            pizzaTopping.PizzaId = id;
            var toppings = GetToppings();
            ViewBag.toppings = toppings.Select(j => new SelectListItem()
            {
                Value = j.Id.ToString(),
                Text = j.Name
            });

           
            return View(pizzaTopping);
        }

        [HttpGet]

        public IActionResult DeletePizzaTopping(int id)
        {
            PizzaTopping pizzaTopping = DeletePizzaToppingApi(id);
            return RedirectToAction("UpdatePizza", new { id = pizzaTopping.PizzaId });
        }

        private PizzaTopping DeletePizzaToppingApi(int id)
        {
          

                var client = new HttpClient();
                string url = $"http://localhost:5002/PizzaTopping?id={id}";
                var response = client.DeleteAsync(url);
                if (response.Result.IsSuccessStatusCode)
                {
                    var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<PizzaTopping>(responsebody);
                }
                else
                {
                    return null;
                }

            
        }

        [HttpPost]

        public IActionResult AddPizzaTopping(PizzaTopping pizzaTopping)
        {
            InsertPizzaTopping(pizzaTopping);
            return RedirectToAction("UpdatePizza", new { id = pizzaTopping.PizzaId });
        }
        private List<Topping> GetToppings()
        {
            
                var client = new HttpClient();
                string url = $"http://localhost:5002/Topping";
                var response = client.GetAsync(url);
                if (response.Result.IsSuccessStatusCode)
                {
                    var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Topping>>(responsebody);
                }
                else
                {
                    return null;
                }
            
        }

        [HttpGet]

        public IActionResult DeletePizza(int id)
        {
            Pizza pizza = DeletePizzaApi(id);
            return RedirectToAction("ConfirmOrder", new { id = pizza.OrderId });
        }

        private Pizza DeletePizzaApi(int id)
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Pizza?id={id}";
            var response = client.DeleteAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Pizza>(responsebody);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]

        public IActionResult SelectPizza(int id)
        {          
            var pizzas = PizzaSingleton.Instance.Pizzas;
            var pizzasList = new List<SelectListItem>();
            for (int i = 0; i < pizzas.Count; i++)
            {
                pizzasList.Add(new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = pizzas[i].Name
                });
            }
            ViewBag.Pizzas = pizzasList;
            var viewModel = new AddPizzaViewModel();
            viewModel.OrderId = id;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SelectPizza(AddPizzaViewModel addPizzaViewModel)
        {
            var pizzas = PizzaSingleton.Instance.Pizzas;
            var selectedPizza = pizzas[addPizzaViewModel.PizzaIndex];
            var defaultToppings = selectedPizza.PizzaToppings;
            selectedPizza.OrderId = addPizzaViewModel.OrderId;
            selectedPizza = AddPizza(selectedPizza);
            if(defaultToppings != null)
            {
                foreach (var pizzaTopping in defaultToppings)
                {
                    pizzaTopping.PizzaId = selectedPizza.Id;
                    InsertPizzaTopping(pizzaTopping);
                }
            }
            return RedirectToAction("UpdatePizza", new { id = selectedPizza.Id });
        }

        private PizzaTopping InsertPizzaTopping(PizzaTopping pizzaTopping)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/PizzaTopping";
            string pizzaToppingJson = JsonConvert.SerializeObject(pizzaTopping);
            HttpContent httpContent = new StringContent(pizzaToppingJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<PizzaTopping>(responsebody);
            }
            else
            {
                return null;
            }
        }

        private Pizza AddPizza(Pizza pizza)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Pizza";
            string pizzaJson = JsonConvert.SerializeObject(pizza);
            HttpContent httpContent = new StringContent(pizzaJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Pizza>(responsebody);
            }
            else
            {
                return null;
            }
        }

        private void UpdateOrder(Order order)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Order";
            string orderJson = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Order was updated successfully";
            }
            else
            {
                ViewBag.Message = "ERROR: Please try again!";
            }
        }

        private Order CreateOrder(Order order)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Order";
            string orderJson = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return  JsonConvert.DeserializeObject<Order>(responsebody);
            }
            else
            {
                return null;
            }
        }
        [HttpPost]

        public IActionResult InitOrder(PlaceOrderViewModel placeOrderViewModel)
        {
            int customerId = int.Parse(placeOrderViewModel.CustomerId);
            int storeId = int.Parse(placeOrderViewModel.StoreId);
            var existingOrders = GetCustomerOrders(customerId);
            if (existingOrders != null && existingOrders.Any()) 
            {
                var latestOrderDate = existingOrders.Select(i => i.OrderDate).Max();
                var hours = (DateTime.Now - latestOrderDate).TotalHours;
                if (hours < 2)
                {
                    ViewBag.ErrorMessage = "ERROR You can only make one order every two hours.";
                    return InitOrder(customerId, storeId);
                }

                var existingOrdersInStore = existingOrders.Where(i => i.StoreId == storeId).ToList();
                if (existingOrdersInStore != null && existingOrdersInStore.Any())
                {
                    var latestOrderDateInStore = existingOrdersInStore.Select(i => i.OrderDate).Max();
                    var hoursStore = (DateTime.Now - latestOrderDateInStore).TotalHours;
                    if (hoursStore < 24)
                    {
                        ViewBag.ErrorMessage = "ERROR You can only make one order from one location every 24 hours.";
                        return InitOrder(customerId, storeId);
                    }
                }
                
            }
     
            
            var order = new Order()
            {
                Id = placeOrderViewModel.OrderId,
                CustomerId = int.Parse(placeOrderViewModel.CustomerId),
                StoreId = int.Parse(placeOrderViewModel.StoreId)
            };
            order = CreateOrder(order);
            return RedirectToAction("SelectPizza", new { id = order.Id });
        }

        [HttpPost]

        public IActionResult ViewSalesReportMonth(ViewSalesReportMonthViewModel viewSalesReportMonthViewModel)
        {
            List<Order> orders = GetOrdersApi();
            int month = int.Parse(viewSalesReportMonthViewModel.Month);
            int year = DateTime.Today.Year;
            var monthlyOrders = orders.Where(i => i.OrderDate.Month == month && i.OrderDate.Year == year).ToList();
            decimal totalRevenue = calculateRevenue(monthlyOrders);

            ViewBag.totalRevenue = totalRevenue.ToString("c2");
            List<PizzaNameCount> pizzaNameCounts = GetPizzaNameCount(monthlyOrders);

            ViewBag.pizzaNameCounts = pizzaNameCounts;
            return ViewSalesReportMonth(viewSalesReportMonthViewModel.Month);
        }

        [HttpPost]

        public IActionResult ViewSalesReportWeek(ViewSalesReportWeekViewModel viewSalesReportWeekViewModel)
        {
            List<Order> orders = GetOrdersApi();
            string week = viewSalesReportWeekViewModel.Week;
            var sections = week.Split('-');
            DateTime start = DateTime.Parse(sections[0]);
            DateTime end = DateTime.Parse(sections[1]);
            var weeklyOrders = orders.Where(i => i.OrderDate >= start && i.OrderDate <= end).ToList();
            decimal totalRevenue = calculateRevenue(weeklyOrders);

            ViewBag.totalRevenue = totalRevenue.ToString("c2");
            List<PizzaNameCount> pizzaNameCounts = GetPizzaNameCount(weeklyOrders);

            ViewBag.pizzaNameCounts = pizzaNameCounts;
            return ViewSalesReportWeek(viewSalesReportWeekViewModel.Week);
        }

            private List<PizzaNameCount> GetPizzaNameCount(List<Order> orders)
        {
            List<PizzaNameCount> pizzaNameCounts = new List<PizzaNameCount>();
            foreach( var order in orders)
            {
                foreach(var pizza in order.Pizzas)
                {
                    var pizzaName = pizzaNameCounts.Where(i => i.PizzaName == pizza.Name).FirstOrDefault();
                    if (pizzaName == null)
                    {
                        pizzaNameCounts.Add(new PizzaNameCount { PizzaName = pizza.Name, Count = pizza.Quantity });
                    } else
                    {
                        pizzaName.Count += pizza.Quantity;
                    }
                }

               
            }
            return pizzaNameCounts;
        }

        private decimal calculateRevenue(List<Order> orders)
        {
            decimal totalRevenue = 0m;
            foreach(var order in orders)
            {
                totalRevenue += order.GetTotal();
            }
            return totalRevenue;
        }

        private List<Order> GetOrdersApi()
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Orders";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Order>>(responsebody);
            }
            return null;
        
    }
        [HttpGet]

        public IActionResult ViewSalesReportWeek(string week = "")
        {
            var ViewModel = new ViewSalesReportWeekViewModel();
            ViewModel.Week = week;
            var weeks = GetWeeks();
            ViewBag.weeks = weeks;
            return View(ViewModel);

        }

        private List<SelectListItem> GetWeeks()
        {
            DateTime latestSunday = GetLatestSunday();
            DateTime latestMonday = latestSunday.AddDays(-6);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            string week = $"{latestMonday.ToShortDateString()}-{latestSunday.ToShortDateString()}";
            selectListItems.Add(new SelectListItem
            {
                Text = week,
                Value = week
            });
            for (int i = 0; i < 7; i++)
            {
                latestSunday = latestSunday.AddDays(-7);
                latestMonday = latestMonday.AddDays(-7);
                week = $"{latestMonday.ToShortDateString()}-{latestSunday.ToShortDateString()}";
                selectListItems.Add(new SelectListItem
                {
                    Text = week,
                    Value = week
                });

            }

            return selectListItems;
        }

        private DateTime GetLatestSunday()
        {
            var day = DateTime.Today;
            while (day.DayOfWeek != DayOfWeek.Sunday)
            {
                day = day.AddDays(-1);

            }
            return day;
        }

        [HttpGet]

        public IActionResult ViewSalesReportMonth(string month = "")
        {
            var ViewModel = new ViewSalesReportMonthViewModel();
            ViewModel.Month = month;
            var months = new List<SelectListItem>();
            months.Add(new SelectListItem
            {
                Value = "1",
                Text = "January"
            });
            months.Add(new SelectListItem
            {
                Value = "2",
                Text = "February"
            }); 
            months.Add(new SelectListItem
            {
                Value = "3",
                Text = "March"
            });
            months.Add(new SelectListItem
            {
                Value = "4",
                Text = "April"
            });
            months.Add(new SelectListItem
            {
                Value = "5",
                Text = "May"
            });
            months.Add(new SelectListItem
            {
                Value = "6",
                Text = "June"
            });
            months.Add(new SelectListItem
            {
                Value = "7",
                Text = "July"
            });
            months.Add(new SelectListItem
            {
                Value = "8",
                Text = "August"
            });
            months.Add(new SelectListItem
            {
                Value = "9",
                Text = "September"
            });
            months.Add(new SelectListItem
            {
                Value = "10",
                Text = "Spooky Month"
            });
            months.Add(new SelectListItem
            {
                Value = "11",
                Text = "November"
            });
            months.Add(new SelectListItem
            {
                Value = "12",
                Text = "December"
            });

            ViewBag.months = months;
            return View(ViewModel);
        }

        [HttpGet]
        public IActionResult InitOrder(int customerid = 0, int storeid = 0)
        {
            List<Customer> customers = GetCustomers();
            ViewBag.Customers = customers.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.CustomerName
            }).ToList();

            List<Store> stores = GetStores();
            ViewBag.Stores = stores.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            PlaceOrderViewModel orderViewModel = new PlaceOrderViewModel();
            orderViewModel.CustomerId = customerid.ToString();
            orderViewModel.StoreId = storeid.ToString();
            return View(orderViewModel);
        }
        [HttpGet]
        public IActionResult ConfirmOrder(int id)
        {
            List<Customer> customers = GetCustomers();
            ViewBag.Customers = customers.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.CustomerName
            }).ToList();

            List<Store> stores = GetStores();
            ViewBag.Stores = stores.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            Order order = GetOrderById(id);

            return View(order);
        }

        private Order UpdateOrderApi(Order order)
        {
            var client = new HttpClient();
            string url = "http://localhost:5002/Order";
            string orderJson = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, httpContent);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Order>(responsebody);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]

        public IActionResult ConfirmOrder(Order order)
        {
            var existingOrder = GetOrderById(order.Id);
            order.Pizzas = existingOrder.Pizzas;
            if(order.Pizzas == null || !order.Pizzas.Any())
            {
                ViewBag.ErrorMessage = "Please select at least 1 pizza.";
                return ConfirmOrder(order.Id);
            }
            var total = order.GetTotal();
            if (total > 250)
            {
                ViewBag.ErrorMessage = "Your order cannot be more than $250.";

                return ConfirmOrder(order.Id);
            }

            var quantity = order.GetQuantity();
            if (quantity > 50)
            {
                ViewBag.ErrorMessage = "You cannot order more than 50 pizzas.";

                return ConfirmOrder(order.Id);
            }

            order.OrderStatus = "Completed";
            UpdateOrderApi(order);
            TempData["SuccessMessage"] = "You have successfully placed an order!"; 
            return RedirectToAction("Index", "Home");
        }

        private Order GetOrderById(int id)
        {
            var client = new HttpClient();
            string url = $"http://localhost:5002/Order?id={id}";
            var response = client.GetAsync(url);
            if (response.Result.IsSuccessStatusCode)
            {
                var responsebody = response.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Order>(responsebody);
            }
            return null;
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
