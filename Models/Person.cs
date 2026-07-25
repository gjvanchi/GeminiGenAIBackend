namespace AIApiSelection.Models;

public record Person(
    int BusinessEntityID,
    string PersonType,
    bool NameStyle,
    string? Title,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Suffix,
    int EmailPromotion,
    string? AdditionalContactInfo,
    string? Demographics,
    Guid Rowguid,
    DateTime ModifiedDate
);
