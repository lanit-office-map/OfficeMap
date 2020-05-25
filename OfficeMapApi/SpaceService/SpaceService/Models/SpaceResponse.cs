using SpaceService.Database.Entities;
using System;
using System.Collections.Generic;
using Common.RabbitMQ.Models;

namespace SpaceService.Models
{
  public class SpaceResponse
  {
    #region internal properies
    internal int SpaceId { get; set; }
    #endregion

    #region public properties
    public Guid SpaceGuid { get; set; }
    public ICollection<SpaceResponse> Spaces { get; set; }
    public MapResponse Map { get; set; }
    public SpaceTypeResponse SpaceType { get; set; }
    public IEnumerable<WorkplaceResponse> Workplaces { get; set; }
    public string SpaceName { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; }
    public int Floor { get; set; }
    #endregion
  }
}
