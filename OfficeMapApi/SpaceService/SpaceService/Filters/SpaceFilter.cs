namespace SpaceService.Filters
{
    public class SpaceFilter
    {
        public int OfficeId { get; set; }

        public SpaceFilter(int officeId)
          {
            OfficeId = officeId;
        }
    }
}
