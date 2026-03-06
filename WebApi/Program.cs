using CRM.Application.useCases.Student.CreateStudent;
using DemoCRM.Infrastructure.BackgroundServices;
using DemoCRM.Infrastructure.Messaging;
using DemoCRM.Persistance.Context;
using DemoCRM.Persistance.Seed;
using Microsoft.EntityFrameworkCore;
using WebApi.Graph.Mutations;
using WebApi.Graph.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CrmContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("CrmDb"));
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateStudentRequest).Assembly));

builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddType<StudentQueries>()
    .AddType<CourseQueries>()
    .AddType<StudentMutations>()
    .AddType<TeacherMutations>()
    .AddType<TeacherQueries>()
    .AddType<CourseMutations>()
    .AddType<DateOnly>();

// EMAIL SETTINGS
builder.Services.Configure<EmailSettings>( builder.Configuration.GetSection("EmailSettings"));

// INFRASTRUCTURE SERVICES
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RabbitMqPublisher>();

// BACKGROUND WORKER
builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();


// SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CrmContext>();
    await SeedData.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();