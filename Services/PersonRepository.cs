namespace AIApiSelection.Services;

using System.Data;
using AIApiSelection.Models;
using Microsoft.Data.SqlClient;

public interface IPersonRepository
{
    IEnumerable<Person> GetAllPersons();
    Person? GetPersonById(int businessEntityId);
    PersonContact? GetPersonContactByPersonId(int businessEntityId);

    // New methods
    Address? GetAddressById(int addressId);
    IEnumerable<ContactType> GetAllContactTypes();
    ContactType? GetContactTypeById(int contactTypeId);
    IEnumerable<CountryRegion> GetAllCountryRegions();
    CountryRegion? GetCountryRegionByCode(string countryRegionCode);
    PasswordEntity? GetPasswordByBusinessEntityId(int businessEntityId);
    IEnumerable<StateProvince> GetAllStateProvinces();
    StateProvince? GetStateProvinceById(int stateProvinceId);
}

public class PersonRepository : IPersonRepository
{
    private readonly string _connectionString;

    public PersonRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AdventureWorks")
            ?? "Server=localhost;Database=AdventureWorks2022;Integrated Security=True;TrustServerCertificate=True;";
    }

    public IEnumerable<Person> GetAllPersons()
    {
        var persons = new List<Person>();
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT TOP 50 
                    BusinessEntityID, PersonType, NameStyle, Title, FirstName, 
                    MiddleName, LastName, Suffix, EmailPromotion, 
                    CAST(AdditionalContactInfo AS nvarchar(max)) AS AdditionalContactInfo, 
                    CAST(Demographics AS nvarchar(max)) AS Demographics, 
                    rowguid, ModifiedDate 
                FROM Person.Person 
                ORDER BY BusinessEntityID";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                persons.Add(MapPerson(reader));
            }
        }
        catch
        {
            return GetFallbackPersons();
        }
        return persons;
    }

    public Person? GetPersonById(int businessEntityId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    BusinessEntityID, PersonType, NameStyle, Title, FirstName, 
                    MiddleName, LastName, Suffix, EmailPromotion, 
                    CAST(AdditionalContactInfo AS nvarchar(max)) AS AdditionalContactInfo, 
                    CAST(Demographics AS nvarchar(max)) AS Demographics, 
                    rowguid, ModifiedDate 
                FROM Person.Person 
                WHERE BusinessEntityID = @id";
            cmd.Parameters.AddWithValue("@id", businessEntityId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapPerson(reader);
            }
        }
        catch
        {
            return GetFallbackPersons().FirstOrDefault(p => p.BusinessEntityID == businessEntityId);
        }
        return null;
    }

    public PersonContact? GetPersonContactByPersonId(int businessEntityId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT TOP 1
                    p.BusinessEntityID,
                    e.EmailAddress,
                    ph.PhoneNumber,
                    ph.PhoneNumberTypeID,
                    a.AddressID,
                    a.AddressLine1,
                    a.AddressLine2,
                    a.City,
                    a.StateProvinceID,
                    a.PostalCode
                FROM Person.Person p
                LEFT JOIN Person.EmailAddress e ON p.BusinessEntityID = e.BusinessEntityID
                LEFT JOIN Person.PersonPhone ph ON p.BusinessEntityID = ph.BusinessEntityID
                LEFT JOIN Person.BusinessEntityAddress bea ON p.BusinessEntityID = bea.BusinessEntityID
                LEFT JOIN Person.Address a ON bea.AddressID = a.AddressID
                WHERE p.BusinessEntityID = @id";
            cmd.Parameters.AddWithValue("@id", businessEntityId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PersonContact(
                    BusinessEntityID: reader.GetInt32(0),
                    EmailAddress: reader.IsDBNull(1) ? null : reader.GetString(1),
                    PhoneNumber: reader.IsDBNull(2) ? null : reader.GetString(2),
                    PhoneNumberTypeID: reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    AddressID: reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                    AddressLine1: reader.IsDBNull(5) ? null : reader.GetString(5),
                    AddressLine2: reader.IsDBNull(6) ? null : reader.GetString(6),
                    City: reader.IsDBNull(7) ? null : reader.GetString(7),
                    StateProvinceID: reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                    PostalCode: reader.IsDBNull(9) ? null : reader.GetString(9)
                );
            }
        }
        catch
        {
            return GetFallbackContacts().FirstOrDefault(c => c.BusinessEntityID == businessEntityId);
        }
        return null;
    }

    public Address? GetAddressById(int addressId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT AddressID, AddressLine1, AddressLine2, City, StateProvinceID, PostalCode, rowguid, ModifiedDate 
                FROM Person.Address 
                WHERE AddressID = @id";
            cmd.Parameters.AddWithValue("@id", addressId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Address(
                    AddressID: reader.GetInt32(0),
                    AddressLine1: reader.GetString(1),
                    AddressLine2: reader.IsDBNull(2) ? null : reader.GetString(2),
                    City: reader.GetString(3),
                    StateProvinceID: reader.GetInt32(4),
                    PostalCode: reader.GetString(5),
                    Rowguid: reader.GetGuid(6),
                    ModifiedDate: reader.GetDateTime(7)
                );
            }
        }
        catch
        {
            return GetFallbackAddresses().FirstOrDefault(a => a.AddressID == addressId);
        }
        return null;
    }

    public IEnumerable<ContactType> GetAllContactTypes()
    {
        var types = new List<ContactType>();
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ContactTypeID, Name, ModifiedDate FROM Person.ContactType ORDER BY ContactTypeID";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                types.Add(new ContactType(
                    ContactTypeID: reader.GetInt32(0),
                    Name: reader.GetString(1),
                    ModifiedDate: reader.GetDateTime(2)
                ));
            }
        }
        catch
        {
            return GetFallbackContactTypes();
        }
        return types;
    }

    public ContactType? GetContactTypeById(int contactTypeId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ContactTypeID, Name, ModifiedDate FROM Person.ContactType WHERE ContactTypeID = @id";
            cmd.Parameters.AddWithValue("@id", contactTypeId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ContactType(
                    ContactTypeID: reader.GetInt32(0),
                    Name: reader.GetString(1),
                    ModifiedDate: reader.GetDateTime(2)
                );
            }
        }
        catch
        {
            return GetFallbackContactTypes().FirstOrDefault(c => c.ContactTypeID == contactTypeId);
        }
        return null;
    }

    public IEnumerable<CountryRegion> GetAllCountryRegions()
    {
        var list = new List<CountryRegion>();
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 50 CountryRegionCode, Name, ModifiedDate FROM Person.CountryRegion ORDER BY Name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CountryRegion(
                    CountryRegionCode: reader.GetString(0),
                    Name: reader.GetString(1),
                    ModifiedDate: reader.GetDateTime(2)
                ));
            }
        }
        catch
        {
            return GetFallbackCountryRegions();
        }
        return list;
    }

    public CountryRegion? GetCountryRegionByCode(string countryRegionCode)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CountryRegionCode, Name, ModifiedDate FROM Person.CountryRegion WHERE CountryRegionCode = @code";
            cmd.Parameters.AddWithValue("@code", countryRegionCode);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new CountryRegion(
                    CountryRegionCode: reader.GetString(0),
                    Name: reader.GetString(1),
                    ModifiedDate: reader.GetDateTime(2)
                );
            }
        }
        catch
        {
            return GetFallbackCountryRegions().FirstOrDefault(cr => cr.CountryRegionCode == countryRegionCode);
        }
        return null;
    }

    public PasswordEntity? GetPasswordByBusinessEntityId(int businessEntityId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT BusinessEntityID, PasswordHash, PasswordSalt, rowguid, ModifiedDate FROM Person.Password WHERE BusinessEntityID = @id";
            cmd.Parameters.AddWithValue("@id", businessEntityId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PasswordEntity(
                    BusinessEntityID: reader.GetInt32(0),
                    PasswordHash: reader.GetString(1),
                    PasswordSalt: reader.GetString(2),
                    Rowguid: reader.GetGuid(3),
                    ModifiedDate: reader.GetDateTime(4)
                );
            }
        }
        catch
        {
            return GetFallbackPasswords().FirstOrDefault(p => p.BusinessEntityID == businessEntityId);
        }
        return null;
    }

    public IEnumerable<StateProvince> GetAllStateProvinces()
    {
        var list = new List<StateProvince>();
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 50 StateProvinceID, StateProvinceCode, CountryRegionCode, IsOnlyStateProvinceFlag, Name, TerritoryID, rowguid, ModifiedDate FROM Person.StateProvince ORDER BY Name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapStateProvince(reader));
            }
        }
        catch
        {
            return GetFallbackStateProvinces();
        }
        return list;
    }

    public StateProvince? GetStateProvinceById(int stateProvinceId)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT StateProvinceID, StateProvinceCode, CountryRegionCode, IsOnlyStateProvinceFlag, Name, TerritoryID, rowguid, ModifiedDate FROM Person.StateProvince WHERE StateProvinceID = @id";
            cmd.Parameters.AddWithValue("@id", stateProvinceId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapStateProvince(reader);
            }
        }
        catch
        {
            return GetFallbackStateProvinces().FirstOrDefault(sp => sp.StateProvinceID == stateProvinceId);
        }
        return null;
    }

    private static StateProvince MapStateProvince(SqlDataReader reader)
    {
        return new StateProvince(
            StateProvinceID: reader.GetInt32(0),
            StateProvinceCode: reader.GetString(1).Trim(),
            CountryRegionCode: reader.GetString(2),
            IsOnlyStateProvinceFlag: reader.GetBoolean(3),
            Name: reader.GetString(4),
            TerritoryID: reader.GetInt32(5),
            Rowguid: reader.GetGuid(6),
            ModifiedDate: reader.GetDateTime(7)
        );
    }

    private static Person MapPerson(SqlDataReader reader)
    {
        return new Person(
            BusinessEntityID: reader.GetInt32(0),
            PersonType: reader.GetString(1),
            NameStyle: reader.GetBoolean(2),
            Title: reader.IsDBNull(3) ? null : reader.GetString(3),
            FirstName: reader.GetString(4),
            MiddleName: reader.IsDBNull(5) ? null : reader.GetString(5),
            LastName: reader.GetString(6),
            Suffix: reader.IsDBNull(7) ? null : reader.GetString(7),
            EmailPromotion: reader.GetInt32(8),
            AdditionalContactInfo: reader.IsDBNull(9) ? null : reader.GetString(9),
            Demographics: reader.IsDBNull(10) ? null : reader.GetString(10),
            Rowguid: reader.GetGuid(11),
            ModifiedDate: reader.GetDateTime(12)
        );
    }

    // --- Fallback lists for offline support & unit tests ---

    private static List<Person> GetFallbackPersons() => new()
    {
        new Person(1, "EM", false, null, "Ken", "J", "Sánchez", null, 0, null, null, Guid.NewGuid(), DateTime.Now),
        new Person(2, "EM", false, null, "Terri", "Lee", "Duffy", null, 1, null, null, Guid.NewGuid(), DateTime.Now),
        new Person(3, "EM", false, null, "Roberto", "Tamburello", "Tamburello", null, 0, null, null, Guid.NewGuid(), DateTime.Now)
    };

    private static List<PersonContact> GetFallbackContacts() => new()
    {
        new PersonContact(1, "ken0@adventure-works.com", "697-555-0142", 1, 1, "4350 El Camino Real", null, "Bellingham", 79, "98225"),
        new PersonContact(2, "terri0@adventure-works.com", "819-555-0175", 1, 2, "1848 Birchwood Ave.", null, "Bellingham", 79, "98225"),
        new PersonContact(3, "roberto0@adventure-works.com", "212-555-0187", 1, 3, "6387 Scenic Avenue", null, "Bellingham", 79, "98225")
    };

    private static List<Address> GetFallbackAddresses() => new()
    {
        new Address(1, "4350 El Camino Real", null, "Bellingham", 79, "98225", Guid.NewGuid(), DateTime.Now),
        new Address(2, "1848 Birchwood Ave.", null, "Bellingham", 79, "98225", Guid.NewGuid(), DateTime.Now)
    };

    private static List<ContactType> GetFallbackContactTypes() => new()
    {
        new ContactType(1, "Accounting Manager", DateTime.Now),
        new ContactType(2, "Assistant Sales Agent", DateTime.Now),
        new ContactType(3, "Assistant Sales Representative", DateTime.Now)
    };

    private static List<CountryRegion> GetFallbackCountryRegions() => new()
    {
        new CountryRegion("US", "United States", DateTime.Now),
        new CountryRegion("CA", "Canada", DateTime.Now),
        new CountryRegion("FR", "France", DateTime.Now)
    };

    private static List<PasswordEntity> GetFallbackPasswords() => new()
    {
        new PasswordEntity(1, "d5T8t0JvBw==", "1g==", Guid.NewGuid(), DateTime.Now),
        new PasswordEntity(2, "k8T2h0NvFw==", "2g==", Guid.NewGuid(), DateTime.Now)
    };

    private static List<StateProvince> GetFallbackStateProvinces() => new()
    {
        new StateProvince(79, "WA", "US", false, "Washington", 1, Guid.NewGuid(), DateTime.Now),
        new StateProvince(80, "CA", "US", false, "California", 1, Guid.NewGuid(), DateTime.Now)
    };
}
