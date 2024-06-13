using BankAccount.Extension;
using BankAccount.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient("MyClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30); // Set your desired timeout value here
});
builder.Services.RegisterRepositories();
builder.Services.RegisterApiInvoker();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
