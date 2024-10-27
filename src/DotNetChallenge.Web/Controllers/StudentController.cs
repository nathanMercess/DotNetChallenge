using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Services.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetChallenge.Web.Controllers;

[ApiVersion("1.0")]
public sealed class StudentController : Controller
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService) => _studentService = studentService;

    public async Task<IActionResult> Index()
        => View(await _studentService.GetAllActiveStudentsAsync());


    [HttpGet("GetActiveStudentsNotInCourse/{courseId:guid}")]
    public async Task<IActionResult> GetActiveStudentsNotInCourse([FromRoute] Guid courseId)
        => Ok(await _studentService.GetActiveStudentsNotInCourseAsync(courseId));

    [HttpGet("StudentDetails/{studentId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid studentId)
        => View("~/Views/StudentDetails/Index.cshtml", await _studentService.GetByIdWithJoinsAsync(studentId));

    [HttpPost(nameof(CreateStudent))]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDto student)
        => Ok(await _studentService.CreateStudentAsync(student));

    [HttpPut(nameof(UpdateStudent))]
    public async Task<IActionResult> UpdateStudent([FromBody] StudentDto student)
        => Ok(await _studentService.UpdateStudentAsync(student));

    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> ExcludedStudent([FromRoute] Guid studentId)
        => Ok(await _studentService.ExcludedStudentAsync(studentId));

}
