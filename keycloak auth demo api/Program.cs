//using keycloak_auth_demo_api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using keycloak_auth_demo_api.Controllers.Authentication;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = "http://localhost:8080/realms/MyRealm/";
    o.Audience = "MyApp";

    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudiences = new string[] { "master-realm", "account", "MyApp" }
    };
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();
            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            return c.Response.WriteAsync(c.Exception.ToString());
        }
    };

    o.RequireHttpsMetadata= false;
    o.SaveToken= true;
    o.Validate();
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Admin", policy => policy.RequireClaim("Admin","[Admin]"));
    o.AddPolicy("superadmin", policy => policy.RequireRole("[superadmin]"));
});
       

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApi v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
