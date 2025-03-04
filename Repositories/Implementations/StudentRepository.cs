using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Model;

namespace Repositories.Implementations
{
    public class StudentRepository : IStudentInterface
    {
        private readonly NpgsqlConnection _conn;
        public StudentRepository(NpgsqlConnection conn)
        {
            _conn = conn;
        }
        public async Task<t_Student> GetStudent(int id)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"SELECT * FROM t_student WHERE c_student_id = @id";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new t_Student
                            {
                                StudentId = reader.GetInt32(reader.GetOrdinal("c_student_id")),
                                FullName = reader.GetString(reader.GetOrdinal("c_full_name")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("c_date_of_birth")),
                                Gender = reader.GetString(reader.GetOrdinal("c_gender")),
                                ClassId = reader.GetInt32(reader.GetOrdinal("c_class_id")),
                                SectionId = reader.GetInt32(reader.GetOrdinal("c_section_id")),
                                GuardianDetails = reader.GetString(reader.GetOrdinal("c_guardian_details")),
                                EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("c_enrollment_date")),
                                Image = reader.IsDBNull(reader.GetOrdinal("c_image")) ? null : reader.GetString(reader.GetOrdinal("c_image")),
                                UserId = reader.GetInt32(reader.GetOrdinal("c_user_id")),
                                Password = reader.GetString(reader.GetOrdinal("c_password"))

                            };
                        }
                        else
                        {
                            return new t_Student();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Asseset Get Exception : " + ex.Message);
                return new t_Student();
            }
        }

        public async Task<int> AddPayment(t_payment payment)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"INSERT INTO t_payment (c_student_id, c_amount, c_paymentDate, c_paymentStatus)
                             VALUES (@studentId, @amount, @paymentDate, @paymentStatus)";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("studentId", payment.StudentId);
                    cmd.Parameters.AddWithValue("amount", payment.Amount);
                    cmd.Parameters.AddWithValue("paymentDate", payment.PaymentDate);
                    cmd.Parameters.AddWithValue("paymentStatus", payment.PaymentStatus.ToString());
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Payment Add Exception : " + ex.Message);
                return -1;
            }
        }
        public async Task<t_payment> GetPayment(int id)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"SELECT * FROM t_payment WHERE c_payment_id = @id";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new t_payment
                            {
                                PaymentId = reader.GetInt32(reader.GetOrdinal("c_payment_id")),
                                StudentId = reader.GetInt32(reader.GetOrdinal("c_student_id")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("c_amount")),
                                PaymentDate = reader.GetDateTime(reader.GetOrdinal("c_paymentDate")),
                                PaymentStatus = reader.GetString(reader.GetOrdinal("c_paymentStatus"))
                            };
                        }
                        else
                        {
                            return null; // No payment found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPayment Exception: " + ex.Message);
                return null;
            }
        }

        public async Task<t_notification> GetNotification()
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }

            try
            {
                string query = "SELECT * FROM t_notifications WHERE c_receiver = 'Student'";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {

                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new t_notification
                            {
                                c_title_name = reader.GetString(reader.GetOrdinal("c_title_name")),
                                c_title_description = reader.GetString(reader.GetOrdinal("c_title_description")),
                                c_receiver = reader.GetString(reader.GetOrdinal("c_receiver"))

                            };
                        }
                        else
                        {
                            return null; // No payment found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPayment Exception: " + ex.Message);
                return null;
            }
        }
        public async Task<int> GetStudentIdByUserId(int userId)
        {
            System.Console.WriteLine(userId + "usedid from function");
            int studentId = 0;
            string query = @"SELECT c_student_id FROM t_student WHERE c_user_id = @id";
            await _conn.OpenAsync();
            using (var cmd = new NpgsqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    studentId = Convert.ToInt32(result);
                }

            }
            await _conn.CloseAsync();
            System.Console.WriteLine(studentId + "student id from function");
            return studentId;
        }

        public async Task<List<t_SyllabusTracking>> GetSyllabusTrackingByStudent(int studentId)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"SELECT * FROM t_syllabustracking WHERE c_student_id = @studentId";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("studentId", studentId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        List<t_SyllabusTracking> syllabusList = new List<t_SyllabusTracking>();
                        while (await reader.ReadAsync())
                        {
                            syllabusList.Add(new t_SyllabusTracking
                            {
                                SyllabusId = reader.GetInt32(reader.GetOrdinal("c_syllabusid")),
                                StudentId = reader.GetInt32(reader.GetOrdinal("c_student_id")),
                                SubjectId = reader.GetInt32(reader.GetOrdinal("c_subject_id")),
                                Topic = reader.GetString(reader.GetOrdinal("c_topic")),
                                CompletionPercentage = reader.GetInt32(reader.GetOrdinal("c_completionpercentage")),
                                CompletionDate = reader.GetDateTime(reader.GetOrdinal("c_completiondate"))
                            });
                        }
                        return syllabusList;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSyllabusTrackingByStudent Exception: " + ex.Message);
                return new List<t_SyllabusTracking>(); // Return an empty list instead of null
            }
        }
        public async Task<int> teacherFeedback(t_teacherFeedback feedback)
        {
            try
            {

                await using var cm = new NpgsqlCommand(@"
        INSERT INTO t_teacherFeedback 
        (c_teacher_id, c_student_id, c_rating, c_feedback_text, c_created_date, c_teacher_name) 
        VALUES 
        (@c_teacher_id, @c_student_id, @c_rating, @c_feedback_text, @c_created_date, 
        (SELECT u.c_userName FROM t_teacher t 
         JOIN t_user u ON t.c_user_id = u.c_user_id 
         WHERE t.c_teacher_id = @c_teacher_id))", _conn);

                cm.Parameters.AddWithValue("@c_teacher_id", feedback.C_Teacher_Id);
                cm.Parameters.AddWithValue("@c_student_id", feedback.C_Student_Id);
                cm.Parameters.AddWithValue("@c_rating", feedback.C_Rating);
                cm.Parameters.AddWithValue("@c_feedback_text", feedback.C_Feedback_Text);
                cm.Parameters.AddWithValue("@c_created_date", feedback.C_Created_Date);

                await _conn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return 0;
            }
        }
        public async Task<List<vm_subjectimetable>> GetTaskByClass(string classid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM t_subjectimetable", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();
            if (datar.HasRows)
            {
                dt.Load(datar);
            }
            List<vm_subjectimetable> taskList = new List<vm_subjectimetable>();
            taskList = (from DataRow dr in dt.Rows
                        where dr["c_class_id"].ToString() == classid
                        select new vm_subjectimetable()
                        {
                            c_Subject_Id = Convert.ToInt32(dr["c_subject_id"]),
                            c_Teacher_Id = Convert.ToInt32(dr["c_teacher_id"]),
                            c_Task_End_Time = (DateTime)(dr["c_task_end_time"] != DBNull.Value ? Convert.ToDateTime(dr["c_task_end_time"]) : (DateTime?)null),
                            c_Task_Start_Time = (DateTime)(dr["c_task_start_time"] != DBNull.Value ? Convert.ToDateTime(dr["c_task_start_time"]) : (DateTime?)null),
                            c_Task_Description = dr["c_task_description"].ToString()

                        }).ToList();
            _conn.Close();
            return taskList;
        }

        public async Task<(bool success, string message, List<TeachingMaterial>)> GetMaterialsByTeacherId(int id)
        {
            try
            {

                List<TeachingMaterial> materials = new List<TeachingMaterial>();
                string query = @"
                SELECT 
                    tm.c_material_id, 
                    tm.c_file_name, 
                    tm.c_file_type, 
                    tm.c_upload_date, 
                    tm.c_subject_id, 
                    tm.c_teacher_id, 
                    tm.c_file_path, 
                    u.c_userName AS teacher_name,
                    s.c_subject_name 
                FROM t_teaching_material tm
                LEFT JOIN t_teacher t ON tm.c_teacher_id = t.c_teacher_id
                LEFT JOIN t_user u ON t.c_user_id = u.c_user_id
                LEFT JOIN t_subject s ON tm.c_subject_id = s.c_subject_id
                WHERE tm.c_teacher_id = @TeacherId";
                await _conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherId", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            materials.Add(new TeachingMaterial
                            {
                                C_Material_Id = reader.GetInt32(0),
                                C_File_Name = reader.GetString(1),
                                C_File_Type = reader.GetString(2),
                                C_Upload_Date = reader.GetDateTime(3),
                                C_Subject_Id = reader.GetInt32(4),
                                C_Teacher_Id = reader.GetInt32(5),
                                C_File_Path = reader.GetString(6),
                                c_Teacher_Name = reader.IsDBNull(7) ? null : reader.GetString(7),
                                C_Subject_Name = reader.IsDBNull(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
                if (materials.Count > 0)
                {
                    return (true, "Materials retrieved successfully", materials);
                }
                return (false, "No materials found for the given teacher", new List<TeachingMaterial>());

            }
            catch (Exception ex)
            {
                return (false, "Error retrieving materials: " + ex.Message, new List<TeachingMaterial>());
            }
            finally
            {
                await _conn.CloseAsync();
            }

        }




    }




}
