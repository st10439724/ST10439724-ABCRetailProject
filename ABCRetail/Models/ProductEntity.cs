
using Azure;
using Azure.Data.Tables;
using System;

namespace ABCRetail.Models
{
    public class ProductEntity : ITableEntity
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Product-specific properties as we hadd prod inform
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }

       
    }
}
