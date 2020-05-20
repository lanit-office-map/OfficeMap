using System;
using System.Runtime.Serialization;

namespace OfficeService.Models
{
    public class OfficeResponse
    {
        public Guid Guid { get; set; }
        public int OfficeId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string PhoneNumber { get; set; }
    }
}
