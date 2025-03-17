using Core.Entities;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams productSpecParams) : base(x => 
        (productSpecParams.Brands.Count == 0  || productSpecParams.Brands.Contains(x.Brand)) &&
        (productSpecParams.Types.Count == 0  || productSpecParams.Types.Contains(x.Type)) 
    )
    {
        switch (productSpecParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;

            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;

            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
