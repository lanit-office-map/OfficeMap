using System;

namespace WorkplaceService.Models
{
  public class Workplace
  {
    #region internal properties
    internal int SpaceId { get; set; }

    internal Guid WorkspaceGuid { get; set; }

    internal int EmployeeId { get; set; }
    #endregion

    #region public properties
    public Guid UserGuid { get; set; }
    public Map Map { get; set; }
    #endregion

  }
}
