using Edutrack.Interfaces;
using Edutrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TeacherController : Controller
{
    private readonly ITeacherInterface _teacherInterface;
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ITeacherInterface teacherInterface, ILogger<TeacherController> logger)
    {
        _teacherInterface = teacherInterface;
        _logger = logger;
    }

    [HttpGet("GetTeacher")]
    public async Task<IActionResult> GetTeacher()
    {
        var userID = HttpContext.Session.GetInt32("UserId");
        if (userID == null)
        {
            return Json(new { success = false, message = "User not logged in." });
        }

        var result = await _teacherInterface.GetTeacher(userID.Value);
        // var result = await _teacherInterface.GetTeacher(15);
        // System.Console.WriteLine(userID.Value + "userid from GetTeacher");

        if (result != null)
        {
            return Json(new { success = true, message = "Teacher found", data = result });
        }
        else
        {
            return Json(new { success = false, message = "Teacher not found" });
        }
    }

    [HttpPut("UpdateTeacher")]
    public async Task<IActionResult> UpdateTeacher([FromBody] t_UpdateTeacher teacher)
    {

        var userID = HttpContext.Session.GetInt32("UserId");

        Console.WriteLine(userID + "userid from UpdateTeacher");

        if (userID == null)
        {
            return Json(new { success = false, message = "User not logged in." });
        }

        Console.WriteLine(userID);

        var result = await _teacherInterface.UpdateTeacher(teacher, userID.Value);

        if (result == 1)
        {
            return Json(new { success = true, message = "Updated Successfully" });
        }
        else
        {
            return Json(new { success = false, message = "Error updating teacher" });
        }
    }


}
