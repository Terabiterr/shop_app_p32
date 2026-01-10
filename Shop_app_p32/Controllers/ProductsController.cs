using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_app_p32.Models;
using Shop_app_p32.Services;

namespace Shop_app_p32.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public ProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        // GET: http://localhost:[port]/products
        // Displays the list of products
        public async Task<ViewResult> Read()
        {
            var products = await _serviceProduct.GetAsync(); // Retrieve all products asynchronously
            return View(products); // Return the products to the view
        }

        // GET: http://localhost:[port]/products/details/{id}
        // Displays details of a single product by ID
        [HttpGet]
        public async Task<ViewResult> Details(int id)
        {
            var product = await _serviceProduct.GetByIdAsync(id); // Retrieve product by ID asynchronously
            return View(product); // Return the product details to the view
        }

        [HttpGet]
        // GET: http://localhost:[port]/products/create
        // Display the product creation form
        public ViewResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken] // Validate the anti-forgery token for security
        // POST: http://localhost:[port]/products/create
        // Handle product creation form submission
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid) // Check if the form data is valid
            {
                await _serviceProduct.CreateAsync(product); // Create the product asynchronously
                return RedirectToAction(nameof(Read)); // Redirect to the product list
            }
            return View(product); // If validation fails, return to the form with the entered data
        }

        [HttpGet]
        public async Task<ViewResult> Update(int id)
        {
            var product = await _serviceProduct.GetByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _serviceProduct.UpdateAsync(product.Id, product);
                return RedirectToAction(nameof(Read));
            }
            return NotFound();
        }


        [HttpGet]
        // GET: http://localhost:[port]/products/delete
        // Display the product delete confirmation form
        public ViewResult Delete() => View();

        [HttpPost]
        // POST: http://localhost:[port]/products/delete/{id}
        // Handle product deletion
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceProduct.DeleteAsync(id); // Delete the product asynchronously
            return RedirectToAction(nameof(Read)); // Redirect to the product list
        }

    }
}
