using System.Collections.Generic;
using System.Text;

namespace OfficeService.Repository.Filters
{
    public class OfficeFilter
    {
        public int OfficeId { get; }
        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public string Building { get; }
        public string PhoneNumber { get; }

    
        public OfficeFilter(
            int officeId,
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
        }

        public override string ToString()
        {
            var result = new StringBuilder($"(OfficeId: {OfficeId}");
            if (City != null)
            {
                result.Append($", City: {City}");
            }
            if (Street != null)
            {
                result.Append($", Street: {Street}");
            }
            if (House != null)
            {
                result.Append($", House: {House}");
            }
            if (Building != null)
            {
                result.Append($", Building: {Building}");
            }

            return result.ToString();
        }




    }
}

