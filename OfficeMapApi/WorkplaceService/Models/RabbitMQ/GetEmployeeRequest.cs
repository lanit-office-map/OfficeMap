using System;

namespace WorkplaceService.Models.RabbitMQ
{
    public class GetEmployeeRequest
    {
        public Guid EmployeeGuid { get; set; }
        public int EmployeeId { get; set; }
    }
}
