using System;

namespace Common.RabbitMQ.Models
{
  public class GetWorkplaceResponse
  {
    public Guid WorkplaceGuid { get; set; }
    public string Name { get; set; }

    public GetMapFileResponse MapFile { get; set; }
  }
}
