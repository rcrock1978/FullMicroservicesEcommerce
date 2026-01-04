namespace ProductService.Contracts.DTOs;

public record UpdateStockDto(
    int Quantity,
    string Operation // "Add" or "Remove"
);
