namespace WorkplaceService.Models
{
    public class Workplace
    {
        public int? EmployeeId { get; set; }
        public int? SpaceId { get; set; }
        public int? MapId { get; set; }

        public Map Map { get; set; }
    }
}
