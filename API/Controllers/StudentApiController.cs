using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentApiController : ControllerBase
    {
        private readonly IStudentInterface _student;


        public StudentApiController(IStudentInterface studentInterface)
        {
            _student = studentInterface;
        }


        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _student.GetStudent(id);
            if (student != null)
            {
                return Ok(new { success = true, student });
            }
            else
            {
                return BadRequest(new { success = false, message = "Student not found" });
            }
        }

        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] t_payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            var status = await _student.AddPayment(payment);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Payment Added" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Payment not Added" });
            }
        }


        [HttpGet("GetPayment")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _student.GetPayment(id);
            if (payment != null)
            {
                return Ok(new { success = true, payment });
            }
            else
            {
                return NotFound(new { success = false, message = "Payment not found" });
            }
        }
        [HttpGet("GetNotification")]
        public async Task<IActionResult> GetNotification()
        {
            var notification = await _student.GetNotification();
            if (notification != null)
            {
                return Ok(new { success = true, notification });
            }
            else
            {
                return NotFound(new { success = false, message = "Notification not found" });
            }
        }


        [HttpGet("GetByStudent/{studentId}")]
        public async Task<IActionResult> GetSyllabusTrackingByStudent(int studentId)
        {
            try
            {
                var result = await _student.GetSyllabusTrackingByStudent(studentId);

                // Prevents null reference exception and handles empty result set
                if (result == null)
                {
                    return NotFound(new { message = "No syllabus tracking records found for this student." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the data.", error = ex.Message });
            }
        }
        [HttpPost]
        [Route("AddFeedback")]
        public async Task<IActionResult> AddFeedback([FromBody] t_teacherFeedback feedback)
        {
            Console.WriteLine("hello ");
            // if (!ModelState.IsValid)
            // {                
            //     return BadRequest(ModelState);
            // }           
            var status = await _student.teacherFeedback(feedback);

            if (status == 1)
            {
                return Ok(new { success = true, message = "Feedback Insterted Successfully!!!!!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while adding the Feedback" });
            }
        }
        [HttpGet]
        [Route("List")]
        public async Task<ActionResult> List()
        {
            List<vm_subjectimetable> contacts = await
            _student.GetTaskByClass((6).ToString());
            return Ok(contacts);
        }


    }


}
