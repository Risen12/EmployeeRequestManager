using Bogus;
using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRequestManager.Infrastructure.Seeding;

public class DataBaseSeeder
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Employees.AnyAsync() || await context.EmployeeRequests.AnyAsync())
        {
            Console.WriteLine("База данных уже заполнена. Пропуск заполнения.");
            return;
        }
        
        var employeeFaker = new Faker<Employee>()
            .RuleFor(e => e.FullName, f => f.Name.FullName())
            .RuleFor(e => e.Department, f => f.Commerce.Department())
            .RuleFor(e => e.Post, f => f.Name.JobTitle());
        
        var employees = employeeFaker.Generate(1000);
        await context.Employees.AddRangeAsync(employees);
        await context.SaveChangesAsync();

        var requestFaker = new Faker<EmployeeRequest>()
            .RuleFor(r => r.RequestCreationDate, f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(r => r.Description, f => f.PickRandom("Забрать документы", "Починить Wi-Fi", "Написать feature", "Провести собеседование с кандидатом", "Провести совещание", "Сделать отчёт", "Починить принтер"))
            .RuleFor(r => r.RequestExpirationDate, f => f.Date.Future(1).ToUniversalTime())
            .RuleFor(r => r.Status,
                f => f.PickRandom(RequestStatus.InProgress, RequestStatus.Completed, RequestStatus.New))
            .RuleFor(r => r.AuthorId, f => f.PickRandom(employees).Id)
            .RuleFor(r => r.ExecutorId, f => f.PickRandom(employees).Id);
        
        int totalRequests = 1000000;
        int batchSize = 50000; 

        for (int i = 0; i < totalRequests; i += batchSize)
        {
            var batch = requestFaker.Generate(batchSize);
            await context.EmployeeRequests.AddRangeAsync(batch);
            await context.SaveChangesAsync();
        }

        Console.WriteLine("Заполнение базы данных успешно завершено!");
    }
}