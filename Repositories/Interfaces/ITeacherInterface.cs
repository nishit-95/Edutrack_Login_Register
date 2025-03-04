using Edutrack.Models;
namespace Edutrack.Interfaces;

public interface ITeacherInterface
{
    Task<t_UpdateTeacher> GetTeacher(int userID);
    Task<int> UpdateTeacher(t_UpdateTeacher teacher, int userID);
}