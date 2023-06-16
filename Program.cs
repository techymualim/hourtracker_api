using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Workhourtrack;
using Workhourtrack.Entities;



var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add the necessary configurations for your application

builder.Services.AddEntityFrameworkMySQL()
                .AddDbContext<HourtrackContext>(options =>
                {
                    options.UseMySQL("server=workhour-mysql.mysql.database.azure.com;user=workhouse;password=karachi-123;database=hourtrack");
                });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowOrigin"); // Apply CORS middleware here



// ... your other route handlers ...


app.MapGet("/employees", async (HourtrackContext dbContext) =>
{
    var employees = await dbContext.Employees.ToListAsync();
    return employees;
});

app.MapGet("/employees/{id}", async (int id, HourtrackContext dbContext) =>
    await dbContext.Employees.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.MapPut("/employees/{id}", async (int id, Employee inputEmployee, HourtrackContext dbContext) =>
{
    var employee = await dbContext.Employees.FindAsync(id);

    if (employee is null) return Results.NotFound();


    employee.Id = inputEmployee.Id;
    employee.Name = inputEmployee.Name;


    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});


app.MapGet("/projects", async (HourtrackContext dbContext) =>
{
    var projects = await dbContext.Projects.ToListAsync();
    return projects;
});

app.MapGet("/projects/{id}", async (int id, HourtrackContext dbContext) =>
    await dbContext.Projects.FindAsync(id)
        is Project project
            ? Results.Ok(project)
            : Results.NotFound());

app.MapPut("/projects/{id}", async (int id, Project inputProject, HourtrackContext dbContext) =>
{
    var project = await dbContext.Projects.FindAsync(id);

    if (project is null) return Results.NotFound();


    project.Id = inputProject.Id;
    project.Name = inputProject.Name;


    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/workhour", async (HourtrackContext dbContext) =>
{
    var workhours = await dbContext.Workhours.ToListAsync();
    return workhours;
});

app.MapGet("/workhour/{id}", async (int id, HourtrackContext dbContext) =>
    await dbContext.Workhours.FindAsync(id)
        is Workhour Workhours
            ? Results.Ok(Workhours)
            : Results.NotFound());

app.MapPut("/workhour/{id}", async (int id, Project inputProject, HourtrackContext dbContext) =>
{
    var project = await dbContext.Projects.FindAsync(id);

    if (project is null) return Results.NotFound();


    project.Id = inputProject.Id;
    project.Name = inputProject.Name;


    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/workhour/transactions", async (int employeeId, int projectId, DateTime startTime, DateTime endTime, HourtrackContext dbContext) =>
{
    var transactions = await dbContext.Workhours
        .Where(w => w.EmployeeId == employeeId && w.ProjectId == projectId && w.StartTime >= startTime && w.EndTime <= endTime)
        .Select(w => new
        {
            w.EmployeeId,
            w.ProjectId,
            w.StartTime,
            w.EndTime,
            w.WorkDescription
        })
        .ToListAsync();

    return transactions;
});

app.MapPost("/workhour", async (Workhour workhour, HourtrackContext dbContext) =>
{
    dbContext.Workhours.Add(workhour);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/todoitems/{workhour.Id}", workhour);
});



app.UseCors(MyAllowSpecificOrigins);
app.Run();
