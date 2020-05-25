using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpaceService.Models
{
  public class Space
  {
    #region internal properies
    internal int OfficeId { get; set; }

    internal Guid SpaceGuid { get; set; }
    #endregion

    #region public properties
    public ICollection<Space> Spaces { get; set; }
    public Map Map { get; set; }
    public Guid SpaceTypeGuid { get; set; }
    public string SpaceName { get; set; }
    public string Description { get; set; }
    public int? Capacity { get; set; }
    public int? Floor { get; set; }

    #endregion

  }
}