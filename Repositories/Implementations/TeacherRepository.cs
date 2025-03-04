using Edutrack.Interfaces;
using Edutrack.Models;
using Npgsql;
namespace Edutrack.Repositories.Implementations;

public class TeacherRepository : ITeacherInterface
{
    private readonly NpgsqlConnection _con;
    public TeacherRepository(NpgsqlConnection con)
    {
        _con = con;
    }

    public async Task<t_UpdateTeacher> GetTeacher(int userID)
    {

        try
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("SELECT c_phone_number,c_date_of_birth,c_qualification,c_experience,c_subject_expertise FROM t_teacher WHERE c_user_id = @userID", _con))
            {
                cmd.Parameters.AddWithValue("userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new t_UpdateTeacher
                        {
                            C_phone = reader["c_phone_number"].ToString(),
                            C_date = DateOnly.FromDateTime(Convert.ToDateTime(reader["c_date_of_birth"])),
                            C_qualification = reader["c_qualification"].ToString(),
                            C_experience = Convert.ToInt32(reader["c_experience"]),
                            C_subjectExpertise = reader["c_subject_expertise"].ToString()
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (_con.State == System.Data.ConnectionState.Open)
            {
                await _con.CloseAsync();
            }
        }

        return null;
    }


    public async Task<int> UpdateTeacher(t_UpdateTeacher teacher, int userID)
    {
        try
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("UPDATE t_teacher SET c_phone_number=@phone, c_date_of_birth = @date,c_qualification = @qualification,c_experience = @experience ,c_subject_expertise = @subject_expertise WHERE c_user_id = @userID", _con))
            {
                cmd.Parameters.AddWithValue("phone", teacher.C_phone);
                cmd.Parameters.AddWithValue("date", teacher.C_date);
                cmd.Parameters.AddWithValue("qualification", teacher.C_qualification);
                cmd.Parameters.AddWithValue("experience", teacher.C_experience);
                cmd.Parameters.AddWithValue("subject_expertise", teacher.C_subjectExpertise);
                cmd.Parameters.AddWithValue("userID", userID);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error: " + ex.Message);
            return 0;
        }

        finally
        {
            if (_con.State == System.Data.ConnectionState.Open)
            {
                await _con.CloseAsync();
            }
        }


    }

}

