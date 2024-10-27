namespace DotNetChallenge.Crosscutting.Dtos;

public sealed class StudentDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string User { get; set; }

    public string Password { get; set; }

    public AcademicClassDto[] AcademicClasses { get; set; } = [];

    public StudentDto() { }

    public StudentDto(Guid id, string name, string user, AcademicClassDto[] academicClasses)
    {
        Id = id;
        Name = name;
        User = user;
        AcademicClasses = academicClasses;
    }
}