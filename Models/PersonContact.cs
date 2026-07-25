namespace AIApiSelection.Models;

public record PersonContact(
    int BusinessEntityID,
    string? EmailAddress,
    string? PhoneNumber,
    int? PhoneNumberTypeID,
    int? AddressID,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    int? StateProvinceID,
    string? PostalCode
);
