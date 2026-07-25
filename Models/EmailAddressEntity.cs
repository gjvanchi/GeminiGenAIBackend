namespace AIApiSelection.Models;

public record EmailAddressEntity(
    int BusinessEntityID,
    int EmailAddressID,
    string? EmailAddress,
    Guid Rowguid,
    DateTime ModifiedDate
);
