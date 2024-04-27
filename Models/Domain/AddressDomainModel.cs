namespace StudentPortal.API.Models.Domain
{
    public class AddressDomainModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public Guid StudentId { get; set; }
        public StudentDomainModel Student {  get; set; }
    }
}
