namespace StudentPortal.API.Models.Domain
{
    public class StudentDomainModel
    {
        public Guid RollNumber { get; set; }
        public string Name { get; set; }
        public int Class {  get; set; }
        public List<AddressDomainModel> Addresses { get; set; }
    }
}
