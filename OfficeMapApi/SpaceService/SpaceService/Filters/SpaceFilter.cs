using System;

namespace SpaceService.Filters
{
  public class SpaceFilter
  {
    public int OfficeId { get; }

    public SpaceFilter(int officeId)
    {
      OfficeId = officeId;
    }
  }
}
