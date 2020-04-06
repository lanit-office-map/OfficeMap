using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Mappers.Interfaces;
using OfficeService.Models;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;

namespace OfficeService.Services
{
  public class OfficesService : IOfficeService
  {
    #region private fields
    private readonly IOfficeRepository officeRepository;
    private readonly IOfficeMapper officeMapper;
    #endregion

    #region public methods
    public OfficesService(
      [FromServices] IOfficeRepository officeRepository,
      [FromServices] IOfficeMapper officeMapper)
    {
      this.officeRepository = officeRepository;
      this.officeMapper = officeMapper;
    }

    public Task<IEnumerable<Office>> GetOfficesAsync()
    {
      var result = officeRepository.GetOfficesAsync().Result;

      return  Task.FromResult(result.Select(officeMapper.Map));
    }
    #endregion

  }
}
