﻿using System;
using OfficeService.Models;
using Common.Interfaces;

namespace OfficeService.Services.Interface
{
    public interface IOfficeService :
          IGet<Office, Guid>,
          IDelete<Guid>,
          IFindAll<Office>,
          ICreate<Office>,
          IUpdate<Office>
    {
    }
}