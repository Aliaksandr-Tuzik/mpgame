using Microsoft.AspNetCore.ResponseCompression;
using MPGame.Api.Hubs;
using MPGame.Api.Services;

const string AllowRequestsFromClientPolicy = "AllowRequestsFromClientPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LobbiesService>();
builder.Services.AddSingleton<LobbiesController.ILobbiesService>((provider) => provider.GetService<LobbiesService>());
builder.Services.AddSingleton<GameHub.ILobbiesService>((provider) => provider.GetService<LobbiesService>());

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: AllowRequestsFromClientPolicy,
        policy  =>
        {
            var clientBaseUrl = builder.Configuration["clientBaseUrl"];
            
            Console.WriteLine($"Client base URL: {clientBaseUrl}");

            policy.WithOrigins(clientBaseUrl)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowRequestsFromClientPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/gamehub");

app.Run();
