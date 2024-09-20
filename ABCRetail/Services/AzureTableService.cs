using Azure.Data.Tables;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ABCRetail.Models;
using Microsoft.Extensions.Logging;
using Azure;
using System.Linq.Expressions;


namespace ABCRetail.Services
{
    public class AzureTableService
    {
        private readonly TableServiceClient _serviceClient;
        private readonly ILogger<AzureTableService> _logger;

        public AzureTableService(IConfiguration configuration, ILogger<AzureTableService> logger)

        {
            _logger = logger;
            _logger.LogInformation("Setting up AzureTableService with connection string.");
            string connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString")!;
            _serviceClient = new TableServiceClient(connectionString);

            _logger.LogInformation("Setting up AzureTableService with connection string.");
            //var connectionString = "DefaultEndpointsProtocol=https;AccountName=cldv7112project1;AccountKey=yy+T9vzG7frxPNI/iUH8zPvGyrgVH6XsFlQUU0KAao/qC95DDomAP14BUjuyywSWhwazQrVLIRUa+AStj0p+MA==;EndpointSuffix=core.windows.net";
            //_serviceClient = new TableServiceClient(connectionString);
        }

        // Initialize and get the TableClient for a specific table (Customers or Products)
        private TableClient GetTableClient(string tableName)
        {
            var tableClient = _serviceClient.GetTableClient(tableName);
            tableClient.CreateIfNotExists(); // We want to make Ensure the table exists
            return tableClient;
        }

        // -------------------- CUSTOMER METHODS --------------------

        // We use the below to ADd a customer to the Customers table
        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            var tableClient = GetTableClient("Customers");
            try
            {
                customer.PartitionKey = "Customers";
                customer.RowKey = Guid.NewGuid().ToString();

                _logger.LogInformation($"Adding customer with PartitionKey: {customer.PartitionKey}, RowKey: {customer.RowKey}");


                await tableClient.AddEntityAsync(customer);
                _logger.LogInformation($"Added customer!: {customer.FirstName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add customer: {ex.Message}");
                throw;
            }

        }



        // Get a list of customers from the Customers table
        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            var tableClient = GetTableClient("Customers");
            var customers = new List<CustomerEntity>();
            await foreach (var customer in tableClient.QueryAsync<CustomerEntity>())
            {
                customers.Add(customer);
            }
            return customers;
        }
        // Update a customer
        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            var tableClient = GetTableClient("Customers");
            await tableClient.UpdateEntityAsync(customer, ETag.All);
        }

        // Delete a customer
        public async Task DeleteCustomerAsync(string id)
        {
            var tableClient = GetTableClient("Customers");
            await tableClient.DeleteEntityAsync("Customers", id);
        }



        // -------------------- PRODUCT METHODS --------------------

        // Add a product to the Products table
        public async Task AddProductAsync(ProductEntity product)
        {

            var tableClient = GetTableClient("Products");
            try
            {
                product.PartitionKey = "Products";
                product.RowKey = Guid.NewGuid().ToString();
                await tableClient.AddEntityAsync(product);
                Console.WriteLine($"Added Product: {product.ProductName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add Product: {ex.Message}");
                throw;
            }

        }

        // Get a list of products from the Products table (not working / why??)
        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var tableClient = GetTableClient("Products");
            var products = new List<ProductEntity>();
            await foreach (var product in tableClient.QueryAsync<ProductEntity>())
            {
                products.Add(product);
            }
            return products;
        }

        // Update a product
        public async Task UpdateProductAsync(ProductEntity product)
        {
            var tableClient = GetTableClient("Products");
            await tableClient.UpdateEntityAsync(product, ETag.All);
        }

        // Delete a product
        public async Task DeleteProductAsync(string id)
        {
            var tableClient = GetTableClient("Products");
            await tableClient.DeleteEntityAsync("Products", id);
        }

    }

}
