using Eshopping.API.Configuration.SeedData.Products.Models;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace Eshopping.API.Configuration.SeedData;

public static partial class SeedData
{
    public static void SeedProductsAndCategories(IConfiguration configuration, EshoppingContext context)
    {
        List<(Guid Id, string Name)> productCategories = GetExistingOrNewProductCategories(configuration, context);
        CreateProductsIfNotExist(configuration, context, productCategories);
    }

    #region section private methods

    private static List<(Guid Id, string Name)> GetExistingOrNewProductCategories(IConfiguration configuration, EshoppingContext context)
    {
        List<ProductCategorySeedData> productCategorySeedData =
                configuration.GetSection("SeedData:ProductCategories").Get<List<ProductCategorySeedData>>() ?? [];

        List<ProductCategory> productCategories = [.. context.ProductCategories];

        List<string> listToAdd = productCategorySeedData.Where(pc => !productCategories.Exists(p => p.Name == pc.Name)).Select(pc => pc.Name).ToList();

        if (listToAdd.Count > 0)
        {
            productCategories.AddRange(listToAdd.Select(name => new ProductCategory(name)));
            context.ProductCategories.AddRange(productCategories);
            context.SaveChanges();
        }

        return productCategories.Select(pc => (pc.ID, pc.Name)).ToList();
    }

    private static void CreateProductsIfNotExist(IConfiguration configuration, EshoppingContext context, List<(Guid Id, string Name)> productCategories)
    {
        List<ProductSeedData> productSeedData = configuration.GetSection("SeedData:Products").Get<List<ProductSeedData>>() ?? [];

        List<Product> products = [.. context.Products];

        productSeedData = productSeedData.Where(pc => !products.Exists(p => p.ReferenceNumber == pc.ReferenceNumber)).ToList();

        if (productSeedData.Count == 0) return;

        List<Product> productsToAdd = productSeedData.Select(pro => new Product(
            productCategories.FirstOrDefault(pc => pc.Name == pro.CategoryName).Id,
            pro.ReferenceNumber,
            pro.Name,
            pro.Description,
            pro.Price,
            pro.StockQuantity
        )).ToList();

        context.Products.AddRange(productsToAdd);
        context.SaveChanges();
    }

    #endregion
}