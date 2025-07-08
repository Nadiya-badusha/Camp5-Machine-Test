using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Record_Management_System.Models;
using Student_Record_Management_System.Service;

namespace Student_Record_Management_System.Controllers
{
    [Authorize(Roles = "Invigilator")]
    public class InvigilatorController : Controller
    {

        private readonly IStudentService _service;
        public InvigilatorController(IStudentService service) => _service = service;


        [HttpGet]
        public IActionResult Dashboard(int? rollNumber)
        {
            if (rollNumber.HasValue)
            {
                var student = _service.GetStudentByRoll(rollNumber.Value);
                if (student != null)
                    return View(new List<StudentRecord> { student });

                TempData["Message"] = "No student found with Roll Number: " + rollNumber;
                return RedirectToAction("Dashboard");
            }

            return View(_service.GetAllStudents());
        }


        //[HttpPost]
        //public IActionResult Edit(StudentRecord s)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["Message"] = "Invalid data during update.";
        //        return RedirectToAction("Dashboard");
        //    }

        //    _service.UpdateStudent(s);
        //    return RedirectToAction("Dashboard");
        //}

        [HttpPost]
        public IActionResult Edit(StudentRecord s)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                TempData["Message"] = "Failed to update: " + string.Join(", ", errors);
                return RedirectToAction("Dashboard");
            }

            _service.UpdateStudent(s);
            TempData["Message"] = "Student updated successfully.";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Create(StudentRecord s)
        {
            _service.AddStudent(s);
            TempData["Message"] = "Student created successfully.";
            return RedirectToAction("Dashboard");
        }

    
        public IActionResult Delete(int rollNumber)
        {
            _service.DeleteStudent(rollNumber);
            return RedirectToAction("Dashboard");
        }

    }
}
