using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;
using OfficeService.Database.Entities;
using OfficeService.Models;

namespace OfficeService.Mappers
{
  public class OfficeModelsProfile : Profile
  {
    public OfficeModelsProfile()
    {
      CreateMap<DbOffice, Office>();

      CreateMap<Office, DbOffice>()
        .ForMember(dboffice => dboffice.Obsolete, opt => opt.Ignore())
        .ForMember(dboffice => dboffice.OfficeId, opt => opt.Ignore());

      CreateMap<DbOffice, OfficeResponse>();
      CreateMap<OfficeResponse, DbOffice>();

      CreateMap<Response<OfficeResponse>, Response<GetOfficeResponse>>();
      CreateMap<OfficeResponse, GetOfficeResponse>();

    }
  }
}
