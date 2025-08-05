namespace Eshopping.API.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int QuantityBackup { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
}