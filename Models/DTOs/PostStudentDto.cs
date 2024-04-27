namespace StudentPortal.API.Models.DTOs
{
    public class PostStudentDto
    {
        public string Name { get; set; }
        public int Class { get; set; }
        public List<string> Addresses { get; set; }
    }
}
