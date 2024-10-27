using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class StudentClassEnrollment : Entity
{
    public Guid AcademicClassId { get; }

    public Guid StudentId { get; }

    public StudentClassEnrollment(Guid academicClassId, Guid studentId)
    {
        DomainValidationHelper.IsNotEmpty(academicClassId, StudentClassEnrollmentDomainErrorsConstant.INVALID_ACADEMIC_CLASS_ID);
        DomainValidationHelper.IsNotEmpty(studentId, StudentClassEnrollmentDomainErrorsConstant.INVALID_ACADEMIC_CLASS_STUDENT_ID);

        AcademicClassId = academicClassId;
        StudentId = studentId;
    }
}