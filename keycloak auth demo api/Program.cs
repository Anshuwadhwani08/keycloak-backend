using keycloak_auth_demo_api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
//using keycloak_auth_demo_api.Controllers.Authentication;

var builder = WebApplication.CreateBuilder(args);
//private readonly IWebHostEnvironment currentEnvironment;
// Add services to the container.
builder.Services.ConfigureJWT(builder.Environment.IsDevelopment(),
    "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArUAFD46d/Rg7usnOH6wqJLqeoD+4QGqcuE8i9xlIx2XOv2mN2Y/y9VUbe8piJEyN3AQtKeH9xct3fRs9ICdnUNDj+1YEfWqLsoEXdpjphfVYbyhGNQTjiMrof4RrNItck/YF+ae4AqEFuYD4LWR8bHA7zZbBFfw2elvGKZ9DKgjEuH4TEVgTuTgU5DMmG8fMPLEosbsLYKRKxuo6muqcSTCkfLOzLkLCHlgS5iOXDLYk9oHF4xC+KdR77zLIdHT6H8tKaVFajN+aRhv6urTDLQb66uroqdj9wJmLGpUZSioq+oGzfMYoWa+J7tFDv4hkn0qZI+IypXupBXa9lZpfHwIDAQAB");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApi", Version = "v1" });

            //First we define the security scheme
            c.AddSecurityDefinition("Bearer", //Name the security scheme
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                    Scheme = JwtBearerDefaults.AuthenticationScheme //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
            });
        });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
