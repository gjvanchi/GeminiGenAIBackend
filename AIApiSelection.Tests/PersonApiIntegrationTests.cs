using System.Net;
using System.Net.Http.Json;
using AIApiSelection.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AIApiSelection.Tests;

public class PersonApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PersonApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPersonById_ReturnsOk_WhenPersonExists()
    {
        var response = await _client.GetAsync("/api/persons/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var person = await response.Content.ReadFromJsonAsync<Person>();
        Assert.NotNull(person);
        Assert.Equal(1, person.BusinessEntityID);
        Assert.NotEmpty(person.FirstName);
    }

    [Fact]
    public async Task GetAddressById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/addresses/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var address = await response.Content.ReadFromJsonAsync<Address>();
        Assert.NotNull(address);
        Assert.Equal(1, address.AddressID);
        Assert.NotEmpty(address.AddressLine1);
    }

    [Fact]
    public async Task GetAllContactTypes_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/contacttypes");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<List<ContactType>>();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public async Task GetContactTypeById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/contacttypes/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var type = await response.Content.ReadFromJsonAsync<ContactType>();
        Assert.NotNull(type);
        Assert.Equal(1, type.ContactTypeID);
    }

    [Fact]
    public async Task GetAllCountryRegions_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/countryregions");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<List<CountryRegion>>();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public async Task GetCountryRegionByCode_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/countryregions/US");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var cr = await response.Content.ReadFromJsonAsync<CountryRegion>();
        Assert.NotNull(cr);
        Assert.Equal("US", cr.CountryRegionCode);
    }

    [Fact]
    public async Task GetPasswordById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/passwords/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var password = await response.Content.ReadFromJsonAsync<PasswordEntity>();
        Assert.NotNull(password);
        Assert.Equal(1, password.BusinessEntityID);
        Assert.NotEmpty(password.PasswordHash);
    }

    [Fact]
    public async Task GetAllStateProvinces_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/stateprovinces");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<List<StateProvince>>();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public async Task GetStateProvinceById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/stateprovinces/79");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var sp = await response.Content.ReadFromJsonAsync<StateProvince>();
        Assert.NotNull(sp);
        Assert.Equal(79, sp.StateProvinceID);
        Assert.NotEmpty(sp.StateProvinceCode);
    }
}
