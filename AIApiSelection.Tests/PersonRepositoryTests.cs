using AIApiSelection.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AIApiSelection.Tests;

public class PersonRepositoryTests
{
    private readonly PersonRepository _repository;

    public PersonRepositoryTests()
    {
        var config = new ConfigurationBuilder().Build();
        _repository = new PersonRepository(config);
    }

    [Fact]
    public void GetAllPersons_ReturnsPersonsList()
    {
        var persons = _repository.GetAllPersons();
        Assert.NotNull(persons);
        Assert.NotEmpty(persons);
    }

    [Fact]
    public void GetPersonById_ExistingId_ReturnsPerson()
    {
        var person = _repository.GetPersonById(1);
        Assert.NotNull(person);
        Assert.Equal(1, person.BusinessEntityID);
        Assert.NotEmpty(person.FirstName);
    }

    [Fact]
    public void GetAddressById_ReturnsAddress()
    {
        var address = _repository.GetAddressById(1);
        Assert.NotNull(address);
        Assert.Equal(1, address.AddressID);
        Assert.NotEmpty(address.AddressLine1);
    }

    [Fact]
    public void GetAllContactTypes_ReturnsList()
    {
        var types = _repository.GetAllContactTypes();
        Assert.NotNull(types);
        Assert.NotEmpty(types);
    }

    [Fact]
    public void GetContactTypeById_ReturnsContactType()
    {
        var type = _repository.GetContactTypeById(1);
        Assert.NotNull(type);
        Assert.Equal(1, type.ContactTypeID);
    }

    [Fact]
    public void GetAllCountryRegions_ReturnsList()
    {
        var list = _repository.GetAllCountryRegions();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public void GetCountryRegionByCode_ReturnsCountryRegion()
    {
        var cr = _repository.GetCountryRegionByCode("US");
        if (cr != null)
        {
            Assert.Equal("US", cr.CountryRegionCode);
        }
    }

    [Fact]
    public void GetPasswordByBusinessEntityId_ReturnsPassword()
    {
        var password = _repository.GetPasswordByBusinessEntityId(1);
        Assert.NotNull(password);
        Assert.Equal(1, password.BusinessEntityID);
        Assert.NotEmpty(password.PasswordHash);
    }

    [Fact]
    public void GetAllStateProvinces_ReturnsList()
    {
        var list = _repository.GetAllStateProvinces();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public void GetStateProvinceById_ReturnsStateProvince()
    {
        var sp = _repository.GetStateProvinceById(79);
        if (sp != null)
        {
            Assert.Equal(79, sp.StateProvinceID);
            Assert.NotEmpty(sp.StateProvinceCode);
        }
    }
}
