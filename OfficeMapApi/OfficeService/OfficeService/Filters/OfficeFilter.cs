using System;

namespace OfficeService.Filters
{
    public class OfficeFilter
    {
        public int OfficeId { get; }
        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public string Building { get; }
        public string PhoneNumber { get; }
        public Guid OfficeGuid { get; }

        public OfficeFilter(
            int officeId,
            Guid officeGuid,
            string city = null,
            string street = null,
            string house = null,
            string building = null,
            string phoneNumber = null)
        {
            OfficeId = officeId;
            City = city;
            Street = street;
            House = house;
            Building = building;
            PhoneNumber = phoneNumber;
            OfficeGuid = officeGuid;
        }
    }
}
