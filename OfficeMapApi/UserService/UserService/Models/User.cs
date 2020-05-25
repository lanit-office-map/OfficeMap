using System;
using System.Runtime.Serialization;

namespace UserService.Models
{
    public class User
    {
        [IgnoreDataMember]
        public Guid UserGuid { get; set; }
        [IgnoreDataMember]
        public string Email { get; set; }

        public Employee Employee { get; set; }
    }
}