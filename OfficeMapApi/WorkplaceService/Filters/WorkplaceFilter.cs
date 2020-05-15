namespace WorkplaceService.Filters
{
    public class WorkplaceFilter
    {
        public int SpaceId { get; }

        public WorkplaceFilter(int spaceId)
        {
            SpaceId = spaceId;
        }
    }
}
