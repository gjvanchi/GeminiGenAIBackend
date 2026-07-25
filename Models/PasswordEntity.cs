namespace AIApiSelection.Models;

public record PasswordEntity(
    int BusinessEntityID,
    string PasswordHash,
    string PasswordSalt,
    Guid Rowguid,
    DateTime ModifiedDate
);
