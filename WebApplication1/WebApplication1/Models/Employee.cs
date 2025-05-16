namespace WebApplication1.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string  Surname { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string XUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        //relations

        public int PositionId { get; set; }
        public Position Positions { get; set; }
    }
}
