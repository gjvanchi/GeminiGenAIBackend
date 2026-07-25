namespace AIApiSelection.Models;

public record Address(
    int AddressID,
    string AddressLine1,
    string? AddressLine2,
    string City,
    int StateProvinceID,
    string PostalCode,
    Guid Rowguid,
    DateTime ModifiedDate
);
