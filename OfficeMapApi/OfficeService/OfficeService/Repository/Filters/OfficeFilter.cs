using System;

namespace OfficeService.Repository.Filters
{
    public class OfficeFilter
    {
        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public string Building { get; }
        public string PhoneNumber { get; }
        public Guid OfficeGuid { get; }

        public OfficeFilter(
            Guid officeGuid,
            string city = null,
            string street = null,
            string house = null,
            string building = null,
            string phoneNumber = null)
        {
            City = city;
            Street = street;
            House = house;
            Building = building;
            PhoneNumber = phoneNumber;
            OfficeGuid = officeGuid;
        }




    }
}
