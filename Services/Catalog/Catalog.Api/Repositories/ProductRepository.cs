using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Constractor
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        }
        #endregion
        #region Product Repo
        public async Task CreateProduct(Product product)
        {
            _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter =
                 Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string Id)
        {
            return await _context.Products.Find(p=>p.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = 
                Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            FilterDefinition<Product> filter =
                Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatResult = await _context.Products.
                ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updatResult.IsAcknowledged && updatResult.ModifiedCount > 0;
        }
        #endregion
    }
}
