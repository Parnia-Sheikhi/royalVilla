using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla.DTO;
using RoyalVilla_API.Services;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwSettings")["Secret"]);

// adding Authentication to our middleware
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters      // tell the parameters if the token is valid or not
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,   // check whether validate token has expired or not
        ClockSkew = TimeSpan.Zero
    };
});

// configure versioning here
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddVersionedApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV";
    option.SubstituteApiVersionInUrl = true;
});

// accessing the other project like Web to have access to our API
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});  // we have to baste the options to show we use sql server and connection string


builder.Services.AddControllers();

var buildProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>(); // we would be able to get all the versions

    foreach (var description in buildProvider.ApiVersionDescriptions)
    {
        var versionName = description.GroupName;
        var versionNumber = description.ApiVersion.ToString();
        var displayName = $"Demo API -- {versionNumber}";

    // for differentiate that we are adding 2 open api
    builder.Services.AddOpenApi(versionName, options =>
    {
        options.AddDocumentTransformer((document, context, cancelationToken) =>
        {
            document.Info = new OpenApiInfo
            {
                Title = "Demo Royal API",
                Version = versionName,
                Description = displayName,
                Contact = new OpenApiContact
                {
                    Name = "Parnia",
                    Email = "parnia.sh100@gmail.com"
                }
            };

            document.Components ??= new();  // if it's not exist create a new of that
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter JWT Bearer token"
                }
            };
            // to have access globally 
            document.Security =
            [
                new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
            }
            ];
            return Task.CompletedTask;
        });
    });
}


// allows us to have maintain multiple version in separate documentation each document can describe different endpoint or version
//builder.Services.AddOpenApi("v1");
//builder.Services.AddOpenApi("v2");

// adding auto mapper for mapping the DTO to our model
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<Villa, VillaCreateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaDTO>().ReverseMap();
    o.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
    o.CreateMap<User, UserDTO>().ReverseMap();

    o.CreateMap<VillaAmenities, VillaAmenitiesCreateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesUpdateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesDTO>().
    ForMember(dest => dest.VillaName, opt => opt.MapFrom(src => src.Villa != null ? src.Villa.Name : null));

    o.CreateMap<VillaAmenitiesDTO, VillaAmenities>();
    // all the prop names are the same and we don't need configuration
}); // above we have to mention the source and the destination
    // ReverseMap would both the mapping villa to VillaCreateDTO and opposite


builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");

    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>(); // we would be able to get all the versions

    // before doing this by running the app we could just see the v1 version but we want to see v2 too
    app.MapScalarApiReference(option =>
    {
        option.Title = "Demo - Royal Villa API";

        var sortedVersion = provider.ApiVersionDescriptions.OrderBy(v => v.ApiVersion).ToList();

        foreach (var description in sortedVersion)
        {
            var versionName = description.GroupName;
            var versionNumber = description.ApiVersion.ToString();
            var displayName = $"Demo API -- {versionNumber}";

            var isDefault = description.ApiVersion.Equals(new ApiVersion(2, 0));

            option.AddDocument(versionName, displayName, $"/openapi/{versionName}.json", isDefault);
        }
        
        // with this we tell the scalar that we have two document for you to register
        //.AddDocument("v2", "Demo API v2", "/openapi/v2.json");
    });
}

// we are not restrict any users to access our api
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// automatically migrate things to our db it's not neccessary to write update-database
static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}
