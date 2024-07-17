using GameStore.Api.Data;
using GameStore.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);// DB connection String

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndPoints();

await app.MigrateDbAsync();
app.Run();
