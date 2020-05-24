using System;

namespace Common.RabbitMQ.Models
{
  public class GetMapFileResponse
  {
    public Guid MapGuid { get; set; }
    public string Content { get; set; }
  }
}
