using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.API.Data;
using StudentPortal.API.Models.Domain;
using StudentPortal.API.Models.DTOs;
using System.Text.RegularExpressions;

namespace StudentPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext dbContext;
        public StudentController(StudentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private static bool IsValid(string name)
        {
            string pattern = @"[\d~`!@#$%^&*()+=\[\]{}\\|;:'"",.<>?\/]";
            if(name == null || Regex.IsMatch(name, pattern))
            {
                return false;
            }
            return true;
        }
        private static bool IsValid(int Class)
        {
            if(Class <= 0 || Class > 20)
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = dbContext.Students.Include(s => s.Addresses).ToList();
            var studentsDto = students.Select(student => new GetStudentDto
            {
                RollNumber = student.RollNumber,
                Name = student.Name,
                Class = student.Class,
                Addresses = student.Addresses != null ? student.Addresses.Select(address => address.Address).ToList(): new List<string>()
            }).ToList();
            return Ok(studentsDto);
        }

        [HttpGet("{RollNumber:Guid}")]
        public IActionResult GetStudentByRollNumber(Guid RollNumber) {
            var student = dbContext.Students.Include(s => s.Addresses).FirstOrDefault(x  => x.RollNumber == RollNumber);
            if(student == null)
            {
                return NotFound();
            }
            var studentDto = new GetStudentDto { 
                RollNumber = student.RollNumber,
                Name = student.Name,
                Class = student.Class,
                Addresses = student.Addresses != null ? student.Addresses.Select(address => address.Address).ToList() : new List<string>()

            };
            return Ok(studentDto);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] PostStudentDto studentDto)
        {
            if(!(IsValid(studentDto.Name) || IsValid(studentDto.Class)))
            {
                return BadRequest("Invalid Name");
            }
            var studentDomainModel = new StudentDomainModel
            {
                Name = studentDto.Name,
                Class = studentDto.Class,
                Addresses = studentDto.Addresses.Select(address => new AddressDomainModel { Address = address }).ToList(),
            };
            dbContext.Students.Add(studentDomainModel);
            dbContext.SaveChanges();
            var NewStudent = new PostStudentDto
            {
                Name = studentDomainModel.Name,
                Class = studentDomainModel.Class,
                Addresses = studentDomainModel.Addresses.Select(address => address.Address).ToList(),
            };
            return CreatedAtAction(nameof(GetStudentByRollNumber), new {RollNumber = studentDomainModel.RollNumber}, NewStudent);
        }

        [HttpPut("{RollNumber:Guid}")]
        public IActionResult UpdateStudent(Guid RollNumber, [FromBody] PostStudentDto updateStudent)
        {
            var student = dbContext.Students.Include(s => s.Addresses).FirstOrDefault(x => x.RollNumber == RollNumber);
            if(student == null)
            {
                return NotFound();
            }
            else if(!(IsValid(updateStudent.Name) || IsValid(updateStudent.Class)))
            {
                return BadRequest("Invalid");
            }
            student.Name = updateStudent.Name;
            student.Class = updateStudent.Class;
            student.Addresses.Clear();
            student.Addresses.AddRange(updateStudent.Addresses.Select(address => new AddressDomainModel { Address = address}));
            dbContext.SaveChanges();
            var updatedStudentDto = new GetStudentDto
            {
                Name = student.Name,
                Class = student.Class,
                Addresses = student.Addresses != null ? student.Addresses.Select(address => address.Address).ToList() : new List<string>()
            };
            return Ok(updatedStudentDto);

        }

        [HttpDelete("{RollNumber:Guid}")]
        public IActionResult DeleteStudent(Guid RollNumber)
        {
            var student = dbContext.Students.Include(s => s.Addresses).FirstOrDefault(x => x.RollNumber == RollNumber);
            if(student == null) { return NotFound();}
            dbContext.Students.Remove(student);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}
