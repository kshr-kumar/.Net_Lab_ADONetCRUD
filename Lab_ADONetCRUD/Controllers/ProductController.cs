using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab_ADONetCRUD.Controllers
{
    public class ProductController : Controller
    {
        IConfiguration _configuration; 
        ProductRepository _productRepository;
        CategoryRepository _categoryRepository; 
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration; 
            _productRepository = new ProductRepository(_configuration.GetConnectionString("DbConnection"));
            _categoryRepository = new CategoryRepository(_configuration.GetConnectionString("DbConnection"));
        }
        public IActionResult Index()
        {
            var products = _productRepository.GetProducts();
            return View(products); 
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.GetCategories(); 
            return View(); 
        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            ModelState.Remove("ProductId"); //It is an optional field for creation, so removed it
                                            
            if (ModelState.IsValid) 
            {
                _productRepository.AddProduct(model); 
                return RedirectToAction("Index"); // product Home page
                                                  
            }
            ViewBag.Categories = _categoryRepository.GetCategories();
            return View();
        }
        public IActionResult Edit(int id) 
        {
            ViewBag.Categories = _categoryRepository.GetCategories();
            Product model = _productRepository.GetProductById(id); 
            return View("Create", model); //Call Create View and pass model
                                          
        }
        [HttpPost]
        public IActionResult Edit(Product model) 
        { if (ModelState.IsValid) 
            {
                _productRepository.UpdateProduct(model); 
                return RedirectToAction("Index"); // product Home page
                                                  
            }
            ViewBag.Categories = _categoryRepository.GetCategories();
            return View(); 
        }
        public IActionResult Delete(int id) 
        {
            _productRepository.DeleteProduct(id); 
            return RedirectToAction("Index"); // Go to the Listing Page
                                              
        }
    }
}
