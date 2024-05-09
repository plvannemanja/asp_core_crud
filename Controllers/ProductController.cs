using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InitProject.Models;

namespace InitProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ??
                throw new ArgumentNullException(nameof(productRepository));
        }

        // GET: api/producs?page=1&perpage=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int page = 1, int perPage = 10)
        {
            return Ok(await _productRepository.GetPage(page, perPage));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity
            };

            await _productRepository.Add(product);

            return CreatedAtAction(nameof(PostProduct), new { productId = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO productDTO)
        {
            var product = await _productRepository.Find(id);
            if(product == null)
            {
                return NotFound();
            }

            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.Quantity = productDTO.Quantity;

            product = await _productRepository.Update(product);

            return Ok(product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Remove(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}