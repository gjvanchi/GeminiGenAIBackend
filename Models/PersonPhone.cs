namespace AIApiSelection.Models;

public record PersonPhone(
    int BusinessEntityID,
    string PhoneNumber,
    int PhoneNumberTypeID,
    DateTime ModifiedDate
);
