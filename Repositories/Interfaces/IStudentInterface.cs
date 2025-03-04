using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Model;

namespace Repositories.Interfaces
{
    public interface IStudentInterface
    {
        Task<t_Student> GetStudent(int id);
        Task<t_payment> GetPayment(int id);

        Task<int> AddPayment(t_payment payment);

        Task<t_notification> GetNotification();
        Task<int> GetStudentIdByUserId(int userId);
        Task<List<t_SyllabusTracking>> GetSyllabusTrackingByStudent(int studentId);

        Task<int> teacherFeedback(t_teacherFeedback feedback);
        Task<List<vm_subjectimetable>> GetTaskByClass(string classid);
        Task<(bool success, string message, List<TeachingMaterial>)> GetMaterialsByTeacherId(int id);

    }
}