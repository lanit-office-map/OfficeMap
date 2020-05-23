using System;

namespace Common.RabbitMQ.Models
{
  public class GetMapFileResponse
  {
    public Guid MapFileGuid { get; set; }
    public string Content { get; set; }
  }
}
