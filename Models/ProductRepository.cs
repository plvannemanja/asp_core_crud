using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace InitProject.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context ??
            throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetPage(int page, int perPage = 10)
        {
            // Calculate skip count based on page number and page size
            int skipCount = (page - 1) * perPage;

            // Query your data source and apply pagination
            var query = await (_context.Products
                                .OrderByDescending(m => m.ProductId) // Assuming you want to order by CreatedDate
                                .Skip(skipCount)
                                .Take(perPage)
                                .ToListAsync());
            return query;
        }

        public async Task<Product> Add(Product product)
        {
            product.CreatedDate = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Find(int productId) {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<Product> Update(Product product)
        {
            product.ModifiedDate = DateTime.Now;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public void Remove(int productId)
        {
            var product = _context.Products.First(p => p.ProductId == productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

    }
}
