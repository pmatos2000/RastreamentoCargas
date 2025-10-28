using RastreamentoCargas.API.Extensions; 


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabaseAndIdentity(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices()
    .AddSwaggerServices()
    .AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyPendingMigrations();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();