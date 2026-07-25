namespace AIApiSelection.Models;

public record StateProvince(
    int StateProvinceID,
    string StateProvinceCode,
    string CountryRegionCode,
    bool IsOnlyStateProvinceFlag,
    string Name,
    int TerritoryID,
    Guid Rowguid,
    DateTime ModifiedDate
);
