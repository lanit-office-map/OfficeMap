namespace WorkplaceService.Filters
{
    public class WorkplaceFilter
    {
        public int? SpaceId { get; }

        public int? EmployeeId { get; }

        public WorkplaceFilter(
          int? spaceId,
          int? employeeId = null)
        {
            SpaceId = spaceId;
            EmployeeId = employeeId;
        }
    }
}
