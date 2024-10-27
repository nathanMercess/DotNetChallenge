using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Infra.Contracts.Repositories;

public interface IStudentRepository
{
    public Task<Student[]> GetAllActiveStudentsAsync();

    public Task<Student> GetByIdAsync(Guid studentId);

    public Task<bool> InsertStudentAsync(Student student);

    public Task<bool> UpdateStudentAsync(Student student);
    
    public Task<bool> ExcludedStudentAsync(Student student);
    
    public Task<Student> GetByIdWithJoinsAsync(Guid studentId);
    
    public Task<Student[]> GetAllActiveStudentsNotInCourseAsync(Guid courseId);
}
