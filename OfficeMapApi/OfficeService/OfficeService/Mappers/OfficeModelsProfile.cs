using AutoMapper;
using OfficeService.Database.Entities;
using OfficeService.Models;

namespace OfficeService.Mappers
{
    public class OfficeModelsProfile : Profile
    {
        public OfficeModelsProfile()
        {
            #region Office
            CreateMap<DbOffice, Office>();

            CreateMap<Office, DbOffice>()
                .ForMember(dboffice => dboffice.Spaces, opt => opt.Ignore())
                .ForMember(dboffice => dboffice.Obsolete, opt => opt.Ignore())
                .ForMember(dboffice => dboffice.OfficeId, opt => opt.Ignore());
            #endregion
        }
    }
}
