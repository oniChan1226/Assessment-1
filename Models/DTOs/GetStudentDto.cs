namespace StudentPortal.API.Models.DTOs
{
    public class GetStudentDto
    {
        public Guid RollNumber { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        public List<string> Addresses { get; set; }
    }
}
