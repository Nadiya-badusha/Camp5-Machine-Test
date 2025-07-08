using Microsoft.AspNetCore.Mvc;
using Student_Record_Management_System.Service;

namespace Student_Record_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service) => _service = service;

        public IActionResult MyRecord()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var record = _service.GetStudentByUserId(userId);
            return View(record);
        }

    }
}
