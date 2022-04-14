using CodeExercise.Api;
using CodeExercise.LocationRepository;
using CodeExercise.LocationService;
using CodeExercise.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;

var builder = WebApplication.CreateBuilder(args);

// Add controllers, setting default serializer
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
}); 

// Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(settings =>
{
    settings.SchemaType = SchemaType.OpenApi3;
    settings.Title = "Code Exercise Public API";
    settings.SchemaNameGenerator = new SchemaNameGenerator();
});

// Add dependency injection
builder.Services.AddSingleton<ILocationRepository, Repository>();
builder.Services.AddSingleton<ILocationSearchService, LocationSearchService>();

// Build application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    //app.UseReDoc();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
