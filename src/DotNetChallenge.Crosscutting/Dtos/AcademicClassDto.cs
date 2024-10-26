namespace DotNetChallenge.Crosscutting.Dtos;

public sealed class AcademicClassDto
{
    public Guid Id { get; }

    public Guid CourseId { get; set; }

    public string ClassName { get; set; }

    public int Year { get; set; }

    public AcademicClassDto(Guid id, Guid courseId, string className, int year)
    {
        Id = id;
        CourseId = courseId;
        ClassName = className;
        Year = year;
    }
}