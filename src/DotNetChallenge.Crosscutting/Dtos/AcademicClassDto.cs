namespace DotNetChallenge.Crosscutting.Dtos;

public sealed class AcademicClassDto
{

    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string ClassName { get; set; }

    public int Year { get; set; }

    public StudentDto[] Students { get; set; } = [];

    public AcademicClassDto() { }

    public AcademicClassDto(Guid id, Guid courseId, string className, int year, StudentDto[] students)
    {
        Id = id;
        CourseId = courseId;
        ClassName = className;
        Year = year;
        Students = students;
    }
}