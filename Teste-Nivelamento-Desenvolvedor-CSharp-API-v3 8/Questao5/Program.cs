using MediatR;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;
using Questao5.Application.Commands;
using Questao5.Application.Queries.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig
    { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.MapPost("/api/conta/movimentacao", async (IMediator mediator, MovimentacaoCommand command) =>
{
    var result = await mediator.Send(command);
    return result ? Results.Ok("Movimentação realizada com sucesso.") : Results.BadRequest("Erro na movimentação.");
});

app.MapGet("/api/conta/saldo/{numeroConta}", async (IMediator mediator, int numeroConta) =>
{
    var saldo = await mediator.Send(new ConsultaSaldoQuery(numeroConta));
    return Results.Ok(new { Saldo = saldo });
});
app.Run();

// Informa��es �teis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html