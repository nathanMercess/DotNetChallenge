using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Services.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetChallenge.Web.Controllers
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public sealed class AcademicClassController : Controller
    {
        private readonly IAcademicClassService _academicClassService;

        public AcademicClassController(IAcademicClassService academicClassService)
            => _academicClassService = academicClassService;

        public async Task<IActionResult> Index()
            => View(await _academicClassService.GetAllActiveClassesAsync());

        [HttpGet("AcademicClassDetails/{classId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid classId)
            => View("~/Views/AcademicClass/AcademicClassDetails.cshtml", await _academicClassService.GetByIdWithJoinsAsync(classId));

        [HttpPost(nameof(CreateAcademicClass))]
        public async Task<IActionResult> CreateAcademicClass([FromBody] AcademicClassDto academicClass)
            => Ok(await _academicClassService.CreateClassAsync(academicClass));

        [HttpPut(nameof(UpdateAcademicClass))]
        public async Task<IActionResult> UpdateAcademicClass([FromBody] AcademicClassDto academicClass)
            => Ok(await _academicClassService.UpdateClassAsync(academicClass));

        [HttpDelete("{classId:guid}")]
        public async Task<IActionResult> ExcludeAcademicClass([FromRoute] Guid classId)
            => Ok(await _academicClassService.ExcludeClassAsync(classId));

        [HttpPost("AddStudentToClass/{classId:guid}/{studentId:guid}")]
        public async Task<IActionResult> AddStudentToClass(Guid classId, Guid studentId)
            => Ok(await _academicClassService.AddStudentToClassAsync(classId, studentId));

        [HttpDelete("RemoveStudentFromClass/{classId:guid}/{studentId:guid}")]
        public async Task<IActionResult> RemoveStudentFromClass(Guid classId, Guid studentId)
            => Ok(await _academicClassService.RemoveStudentFromClassAsync(classId, studentId));
    }
}
