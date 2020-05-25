using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ.Models
{
  public class GetUserRequest
  {
    public Guid UserGuid { get; set; }
  }
}
