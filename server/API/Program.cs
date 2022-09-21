using Flashcards.Data;
using Flashcards.Environment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Flashcards_Environment env = new Flashcards_Environment();
string connectionString = env.AZURE_CONNECTION_STRING;

builder.Services.AddSingleton<IRepository>(sp => new Repository(connectionString, sp.GetRequiredService<ILogger<Repository>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

