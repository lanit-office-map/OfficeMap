using System;
using System.Runtime.Serialization;

namespace UserService.Models
{
  public class User
  {
    #region internal properties
    internal Guid UserGuid { get; set; }
    #endregion
    public Employee Employee { get; set; }
  }
}