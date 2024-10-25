using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class StudentClassEnrollment : Entity
{
    public Guid AcademicClassId { get; }

    public Guid StutantId { get; }

    public StudentClassEnrollment(Guid academicClassId, Guid stutantId)
    {
        DomainValidationHelper.IsNotEmpty(academicClassId, StudentClassEnrollmentDomainErrorsConstant.INVALID_ACADEMIC_CLASS_ID);
        DomainValidationHelper.IsNotEmpty(stutantId, StudentClassEnrollmentDomainErrorsConstant.INVALID_ACADEMIC_CLASS_STUDANT_ID);

        AcademicClassId = academicClassId;
        StutantId = stutantId;
    }
}