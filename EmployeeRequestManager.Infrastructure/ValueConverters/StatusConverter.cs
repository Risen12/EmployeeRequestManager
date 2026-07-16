using System.Linq.Expressions;
using EmployeeRequestManager.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmployeeRequestManager.Infrastructure.ValueConverters;

public sealed class StatusConverter : ValueConverter<RequestStatus, string>
{
    public StatusConverter()
        : base(
            status => status == RequestStatus.New ? "Новая"
                : status == RequestStatus.InProgress ? "В работе"
                : "Завершена",
            status => status == "Новая" ? RequestStatus.New
                : status == "В работе" ? RequestStatus.InProgress
                : RequestStatus.Completed)
    { }
}