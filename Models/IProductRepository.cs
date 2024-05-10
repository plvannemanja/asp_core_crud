namespace InitProject.Models
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetPage(int pageIndex, int perPage = 10);
        Task<Product?> Find(int productId);
        Task<Product> Update(Product product);
        void Remove(int productId); 
    }
}
