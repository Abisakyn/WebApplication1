using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HSport.App.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Product = HSport.App.Model.Product;

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
        public async Task<ActionResult>GetAllProducts()
        {
            return Ok(await _context.Products.ToArrayAsync());
        }
        [HttpGet("{id}")]
        public async Task <ActionResult> GetProducts(int id)
        {
            var product =await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);


        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAvailableProducts()
        {
            var availableProducts = await _context.Products.Where(p => p.IsAvailable)
                                                   .ToArrayAsync();

            if (availableProducts == null)
            {
                return NotFound();
            }

            return Ok(availableProducts);
        }

        [HttpPost]
        public async Task<ActionResult>PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProducts),
                new { id = product.Id }, product);
        }
        [HttpPut("{id}")]
        public async Task <ActionResult>PutProduct(int id, [FromBody] Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State =EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Products.Any(p=>p.Id == id)){
                    return NotFound();
                }else
                {
                    throw; 
                }
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task <ActionResult> DeleteProduct (int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteMultipleProducts([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach(var id in ids)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                products.Add(product);
            }
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return Ok(products);

        }


    }
}
