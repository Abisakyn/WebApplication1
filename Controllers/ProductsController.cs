using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HSport.App.Model;

namespace HSport.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            return Ok(_context.Products.ToArray());
        }
        [HttpGet("{id}")]
        public ActionResult GetProducts(int id)
        {
            var product = _context.Products.Find(id);

            return Ok(product);


        }
    }
}
