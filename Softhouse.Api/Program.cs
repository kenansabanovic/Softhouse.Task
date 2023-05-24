using Softhouse.Application;
using Softhouse.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .WithExposedHeaders("Authorization");
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

app.UseCors("MyCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/auth/google/callback", async callbackApp =>
{

    var authenticateResult = await callbackApp.AuthenticateAsync();

    if (authenticateResult.Succeeded)
    {
        var user = authenticateResult.Principal;

        callbackApp.Response.StatusCode = 200;
        await callbackApp.Response.WriteAsync("Authentication successful");
    }
    else
    {
        callbackApp.Response.StatusCode = 401;
        await callbackApp.Response.WriteAsync("Authentication failed");
    }
});
app.MapControllers();

app.Run();
