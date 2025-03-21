using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
    {
        var spec = new ProductSpecification(productSpecParams);

        return await CreatePageResult(productRepository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.Add(product);

        if (await productRepository.SaveAllAsync())
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

        productRepository.Update(product);

        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating this product.");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product == null) return NotFound();

        productRepository.Remove(product);

        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting this product.");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await productRepository.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await productRepository.ListAsync(spec));
    }

    private bool ProductExist(int id) 
    {
        return productRepository.Exist(id);
    } 
}
