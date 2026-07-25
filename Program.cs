using AIApiSelection.Models;
using AIApiSelection.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register dependencies
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();

// Add OpenAPI services
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Redirect root to API documentation
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
   .ExcludeFromDescription();

// 1. Person API Group
var personsGroup = app.MapGroup("/api/persons")
                      .WithTags("Person API");

personsGroup.MapGet("/", (IPersonRepository repo) => Results.Ok(repo.GetAllPersons()))
    .WithName("GetAllPersons")
    .WithSummary("Retrieve a list of all persons")
    .Produces<IEnumerable<Person>>(StatusCodes.Status200OK);

personsGroup.MapGet("/{id:int}", (int id, IPersonRepository repo) =>
{
    var person = repo.GetPersonById(id);
    return person is not null ? Results.Ok(person) : Results.NotFound(new { Message = $"Person with ID {id} not found." });
})
.WithName("GetPersonById")
.WithSummary("Retrieve basic person information (name, type, demographics)")
.Produces<Person>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

personsGroup.MapGet("/{id:int}/contact", (int id, IPersonRepository repo) =>
{
    var contact = repo.GetPersonContactByPersonId(id);
    return contact is not null ? Results.Ok(contact) : Results.NotFound(new { Message = $"Contact details for person ID {id} not found." });
})
.WithName("GetPersonContactById")
.WithSummary("Retrieve address and email address for a person by ID")
.Produces<PersonContact>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 2. Address API Group
var addressGroup = app.MapGroup("/api/addresses")
                      .WithTags("Address API");

addressGroup.MapGet("/{id:int}", (int id, IPersonRepository repo) =>
{
    var address = repo.GetAddressById(id);
    return address is not null ? Results.Ok(address) : Results.NotFound(new { Message = $"Address with ID {id} not found." });
})
.WithName("GetAddressById")
.WithSummary("Retrieve physical address details by ID")
.Produces<Address>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 3. ContactType API Group
var contactTypeGroup = app.MapGroup("/api/contacttypes")
                          .WithTags("Contact Type API");

contactTypeGroup.MapGet("/", (IPersonRepository repo) => Results.Ok(repo.GetAllContactTypes()))
    .WithName("GetAllContactTypes")
    .WithSummary("Retrieve all contact types")
    .Produces<IEnumerable<ContactType>>(StatusCodes.Status200OK);

contactTypeGroup.MapGet("/{id:int}", (int id, IPersonRepository repo) =>
{
    var type = repo.GetContactTypeById(id);
    return type is not null ? Results.Ok(type) : Results.NotFound(new { Message = $"Contact type with ID {id} not found." });
})
.WithName("GetContactTypeById")
.WithSummary("Retrieve contact type by ID")
.Produces<ContactType>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 4. CountryRegion API Group
var countryRegionGroup = app.MapGroup("/api/countryregions")
                              .WithTags("Country Region API");

countryRegionGroup.MapGet("/", (IPersonRepository repo) => Results.Ok(repo.GetAllCountryRegions()))
    .WithName("GetAllCountryRegions")
    .WithSummary("Retrieve all country regions")
    .Produces<IEnumerable<CountryRegion>>(StatusCodes.Status200OK);

countryRegionGroup.MapGet("/{code}", (string code, IPersonRepository repo) =>
{
    var cr = repo.GetCountryRegionByCode(code);
    return cr is not null ? Results.Ok(cr) : Results.NotFound(new { Message = $"Country region with code {code} not found." });
})
.WithName("GetCountryRegionByCode")
.WithSummary("Retrieve country region details by code")
.Produces<CountryRegion>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 5. Password API Group
var passwordGroup = app.MapGroup("/api/passwords")
                       .WithTags("Password API");

passwordGroup.MapGet("/{id:int}", (int id, IPersonRepository repo) =>
{
    var password = repo.GetPasswordByBusinessEntityId(id);
    return password is not null ? Results.Ok(password) : Results.NotFound(new { Message = $"Password details for BusinessEntityID {id} not found." });
})
.WithName("GetPasswordById")
.WithSummary("Retrieve password details (hash and salt info) by BusinessEntityID")
.Produces<PasswordEntity>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 6. StateProvince API Group
var stateProvinceGroup = app.MapGroup("/api/stateprovinces")
                              .WithTags("State Province API");

stateProvinceGroup.MapGet("/", (IPersonRepository repo) => Results.Ok(repo.GetAllStateProvinces()))
    .WithName("GetAllStateProvinces")
    .WithSummary("Retrieve all state provinces")
    .Produces<IEnumerable<StateProvince>>(StatusCodes.Status200OK);

stateProvinceGroup.MapGet("/{id:int}", (int id, IPersonRepository repo) =>
{
    var sp = repo.GetStateProvinceById(id);
    return sp is not null ? Results.Ok(sp) : Results.NotFound(new { Message = $"State/province with ID {id} not found." });
})
.WithName("GetStateProvinceById")
.WithSummary("Retrieve state/province details by ID")
.Produces<StateProvince>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.Run();

public partial class Program { }
