using Softhouse.Application;
using Softhouse.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//services cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddScoped(typeof(IJsonPlaceholderService), typeof(JsonPlaceholderService));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Application).Assembly));
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "Google";
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.Map("/auth/google/callback", callbackApp =>
{
    callbackApp.Run(async context =>
    {
        var authenticateResult = await context.AuthenticateAsync();

        if (authenticateResult.Succeeded)
        {
            // Authentication succeeded, perform the desired action.
            // For example, you can retrieve the user's information and store it in a session or database.
            var user = authenticateResult.Principal; // Get the authenticated user's information
            // Store the user information as needed

            // Send a response back to the client indicating successful authentication.
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Authentication successful");
        }
        else
        {
            // Authentication failed, handle the error or redirect to an error page.
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authentication failed");
        }
    });
});

app.MapControllers();

app.Run();
