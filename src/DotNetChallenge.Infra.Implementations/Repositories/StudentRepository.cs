using Dapper;
using DotNetChallenge.Domain.Entities;
using DotNetChallenge.Infra.Contracts.Repositories;
using System.Data;

namespace DotNetChallenge.Infra.Implementations.Repositories;

public sealed class StudentRepository : IStudentRepository
{
    private readonly IDbConnection _dbConnection;

    public StudentRepository(IDbConnection dbConnection) => _dbConnection = dbConnection;

    public async Task<Student[]> GetAllActiveStudentsAsync()
    {
        IEnumerable<Student> students = await _dbConnection.QueryAsync<Student>("SELECT * FROM Student where Excluded = 0");

        return students.ToArray();
    }

    public async Task<Student> GetByIdAsync(Guid studentId)
        => await _dbConnection.QuerySingleOrDefaultAsync<Student>("SELECT * FROM Student WHERE Id = @Id", new { Id = studentId });

    public async Task<Student> GetByIdWithJoinsAsync(Guid studentId)
    {
        string sql = @" SELECT 
                 [Student].[Id],
                 [Name],
                 [User],
                 [Password],
                 [AcademicClass].[Id],
                 [CourseId],
                 [ClassName],
                 [Year]
            FROM 
                Student
            LEFT JOIN 
                StudentClassEnrollment ON student.id = StudentClassEnrollment.StudentId
            LEFT JOIN 
                AcademicClass ON StudentClassEnrollment.AcademicClassId = AcademicClass.Id
            WHERE 
                Student.id = @Id";

        var studentDictionary = new Dictionary<Guid, Student>();

        IEnumerable<Student> student = await _dbConnection.QueryAsync<Student, AcademicClass, Student>(
            sql,
            (student, academicClass) =>
            {
                if (!studentDictionary.TryGetValue(studentId, out var currentStudent))
                {
                    currentStudent = student;
                    studentDictionary.Add(studentId, currentStudent);
                }

                if (academicClass != null)
                {
                    currentStudent.AddAcademicClass(academicClass);
                }

                return currentStudent;
            },
            new { Id = studentId },
            splitOn: "Id"
        );

        return student.FirstOrDefault();
    }

    public async Task<bool> InsertStudentAsync(Student student)
    {
        string sql = "INSERT INTO Student ([Id], [Name], [User], [Password], [Excluded]) VALUES (@Id, @Name, @User, @Password, @Excluded)";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, student);

        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        string sql = @"UPDATE Student SET [Name] = @Name, [User] = @User WHERE Id = @Id";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, student);

        return rowsAffected > 0;
    }

    public async Task<bool> ExcludedStudentAsync(Student student)
    {
        string sql = @"UPDATE Student SET [Excluded] = @Excluded WHERE Id = @Id";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, student);

        return rowsAffected > 0;
    }
}