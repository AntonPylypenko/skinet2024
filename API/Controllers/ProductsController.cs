using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await repo.GetProductsAsync()); 
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
      
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExist(id)) 
            return BadRequest($"Cannot update this product #{id}, id of given product is:{product.Id}");

        repo.UpdateProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating this product.");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting this product.");
    }

    private bool ProductExist(int id) 
    {
        return repo.ProductExist(id);
    } 
}
