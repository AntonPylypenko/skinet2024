using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(StoreContext context) :  base(context) { }
}
