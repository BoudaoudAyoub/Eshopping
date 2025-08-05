namespace Eshopping.API.Configuration.SeedData.Products.Models;
public class ProductSeedData
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0.0m;
    public int StockQuantity { get; set; } = 0;
    public string CategoryName { get; set; } = string.Empty;
}