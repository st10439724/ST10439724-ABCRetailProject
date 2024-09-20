using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;
using System.Threading.Tasks;


namespace ABCRetail.Controllers
{

    public class ProductController : Controller
    {
        private readonly AzureTableService _service;
        private readonly BlobStorageService _blobStorageService;


        public ProductController(AzureTableService service, BlobStorageService blobStorageService)
        {
            _service = service;
            _blobStorageService = blobStorageService;
        }

        // GET: Product/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Product/Add
        [HttpPost]
        public async Task<IActionResult> Add(ProductEntity product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var stream = imageFile.OpenReadStream())
                    {
                        await _blobStorageService.UploadBlobAsync("product-images", imageFile.FileName, stream, imageFile.ContentType);
                    }
                    // Store the image URL in the product entity
                    product.ImageUrl = _blobStorageService.GetBlobUri("product-images", imageFile.FileName);
                }

                await _service.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _service.GetProductsAsync(); // Modify to get the product by id
            return View(product);
        }

        // POST: Product/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProductEntity product)
        {
            if (ModelState.IsValid)
            {
                // Update product logic here (you may need to implement a method in the service)
                await _service.UpdateProductAsync(product); // Implement this method
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _service.GetProductsAsync(); // Modify to get the product by id
            return View(product);
        }

        // POST: Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Delete product logic here (you may need to implement a method in the service)
            await _service.DeleteProductAsync(id); // Implement this method
            return RedirectToAction("Index");
        }

        // GET: Product/Index
        public async Task<IActionResult> Index()
        {
            var products = await _service.GetProductsAsync();
            return View(products);
        }
    }

}
