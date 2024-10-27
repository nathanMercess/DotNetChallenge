using Dapper;
using DotNetChallenge.Domain.Entities;
using DotNetChallenge.Infra.Contracts.Repositories;
using System.Data;

namespace DotNetChallenge.Infra.Implementations.Repositories;

public sealed class AcademicClassRepository : IAcademicClassRepository
{
    private readonly IDbConnection _dbConnection;

    public AcademicClassRepository(IDbConnection dbConnection) => _dbConnection = dbConnection;

    public async Task<AcademicClass[]> GetAllActiveClassesAsync()
    {
        IEnumerable<AcademicClass> classes = await _dbConnection.QueryAsync<AcademicClass>(
            "SELECT * FROM AcademicClass WHERE Excluded = 0"
        );

        return classes.ToArray();
    }

    public async Task<AcademicClass> GetByIdAsync(Guid classId)
    {
        return await _dbConnection.QuerySingleOrDefaultAsync<AcademicClass>(
            "SELECT * FROM AcademicClass WHERE Id = @Id",
            new { Id = classId }
        );
    }

    public async Task<AcademicClass> GetByIdWithJoinsAsync(Guid classId)
    {
        string sql = @"
            SELECT 
                ac.[Id], 
                ac.[CourseId], 
                ac.[ClassName], 
                ac.[Year],
                s.[Id], 
                s.[Name], 
                s.[User],
                s.[Excluded]
            FROM 
                [AcademicClass] ac
            LEFT JOIN 
                [StudentClassEnrollment] sce ON ac.[Id] = sce.[AcademicClassId]
            LEFT JOIN 
                [Student] s ON sce.[StudentId] = s.[Id]
            WHERE 
                ac.[Id] = @Id";

        Dictionary<Guid, AcademicClass> classDictionary = new Dictionary<Guid, AcademicClass>();

        IEnumerable<AcademicClass> classes = await _dbConnection.QueryAsync<AcademicClass, Student, AcademicClass>(
            sql,
            (academicClass, student) =>
            {
                if (!classDictionary.TryGetValue(classId, out var currentClass))
                {
                    currentClass = academicClass;
                    classDictionary.Add(classId, currentClass);
                }

                if (student != null && !student.Excluded)
                {
                    currentClass.AddStudent(student);
                }

                return currentClass;
            },
            new { Id = classId },
            splitOn: "Id"
        );

        return classes.FirstOrDefault();
    }

    public async Task<bool> InsertClassAsync(AcademicClass academicClass)
    {
        string sql = "INSERT INTO AcademicClass ([Id], [CourseId], [ClassName], [Year], [Excluded]) VALUES (@Id, @CourseId, @ClassName, @Year, @Excluded)";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, academicClass);

        return rowsAffected > 0;
    }

    public async Task<AcademicClass> ExistsByClassNameAsync(string className)
    {
        string sql = "SELECT * FROM AcademicClass WHERE ClassName = @ClassName AND Excluded = 0";
        return await _dbConnection.QuerySingleOrDefaultAsync<AcademicClass>(sql, new { ClassName = className });
    }

    public async Task<bool> UpdateClassAsync(AcademicClass academicClass)
    {
        string sql = @"UPDATE AcademicClass SET [CourseId] = @CourseId, [ClassName] = @ClassName, [Year] = @Year WHERE Id = @Id";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, academicClass);

        return rowsAffected > 0;
    }

    public async Task<bool> ExcludeClassAsync(Guid classId)
    {
        string sql = @"UPDATE AcademicClass SET [Excluded] = 1 WHERE Id = @Id";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = classId });

        return rowsAffected > 0;
    }
}

