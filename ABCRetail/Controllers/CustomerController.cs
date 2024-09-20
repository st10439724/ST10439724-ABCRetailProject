using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;



namespace ABCRetail.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AzureTableService _service;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(AzureTableService service, ILogger<CustomerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: Customer/Add CRUD operation
        public IActionResult Add()
        {
            return View();
        }

        // POST: Customer/Add CRUD operations
        [HttpPost]
        public async Task<IActionResult> Add(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                
                customer.PartitionKey = "Customers";
               
                customer.RowKey = Guid.NewGuid().ToString();
                
                _logger.LogInformation($"Attempting to add customer with PartitionKey: {customer.PartitionKey}, RowKey: {customer.RowKey}");

                await _service.AddCustomerAsync(customer);

                TempData["SuccessMessage"] = "Customer added successfully!";
                //_logger.LogInformation($"Successfully added customer: {customer.FirstName}"); //we want to display some pop up to confirm a customer is added 
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage); // we want a Log of any validation errors
                }
            }
            return View(customer);
        }

        // GET: Customer/Edit/{id} Crud
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _service.GetCustomersAsync(); // get the customer by id
            return View(customer);
        }

        // POST: Customer/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(string id, CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                // Update customer logic here (you may need to implement a method in the service)
                await _service.UpdateCustomerAsync(customer); // Implement this method
          
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customer/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _service.GetCustomersAsync(); // Modify to get the customer by id
            return View(customer);
        }

        // POST: Customer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Delete customer logic here (you may need to implement a method in the service)
            await _service.DeleteCustomerAsync(id); // Implement this method
            
            return RedirectToAction("Index");
        }

        // GET: Customer/Index
        public async Task<IActionResult> Index()
        {
            var customers = await _service.GetCustomersAsync();
            return View(customers);
        }
    }
}
