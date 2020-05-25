using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ.Models
{
  public class GetUserResponse
  {
    public int UserId { get; set; }

    public GetEmployeeResponse Employee { get; set; }
  }
}
