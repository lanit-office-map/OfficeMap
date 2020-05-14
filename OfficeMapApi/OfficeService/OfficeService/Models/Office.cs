using System;

namespace OfficeService.Models
{

	public class Office
	{
        internal Guid Guid { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string PhoneNumber { get; set; }        
    }
}
