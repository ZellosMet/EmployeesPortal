using EmployeePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService ES;
        public EmployeeController(EmployeeService employee_service)
        {
            ES = employee_service;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string search_term, [FromQuery] string selected_department, [FromQuery] string selected_type, [FromQuery] int page_number = 1, [FromQuery] int page_size = 5)
        {           
            var (employees, totalCount) = await ES.GetEmployees(search_term, selected_department, selected_type, page_number, page_size);
            var viewModel = new EmployeeListViewModel
            {
                Employees = employees,
                TotalPages = (int)Math.Ceiling((double)totalCount / page_size),
                SearchTerm = search_term,
                SelectedDepartment = selected_department,
                SelectedType = selected_type,
                PageSize = page_size,
                PageNumber = page_number
            };

            GetSelectLists();
            ViewBag.PageSizeOptions = new SelectList(new List<int> { 3, 5, 10, 15, 20, 25 }, page_size);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            GetSelectLists();
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                ES.CreateEmployee(employee);
                return RedirectToAction("Success", new { id = employee.Id });
            }

            GetSelectLists();
            return View(employee);
        }

        public IActionResult Success([FromRoute] int id)
        {
            var employee = ES.GetEmployeeById(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        public IActionResult Details([FromRoute] int id)
        {
            var employee = ES.GetEmployeeById(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Update([FromRoute] int id)
        {
            var employee = ES.GetEmployeeById(id);
            if (employee == null) return NotFound();

            GetSelectLists();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Update([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                ES.UpdateEmployee(employee);
                TempData["Message"] = $"��������� � ������� {employee.Id} � ������ {employee.FullName} �������.";
                return RedirectToAction("List");
            }

            GetSelectLists();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            var employee = ES.GetEmployeeById(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            var employee = ES.GetEmployeeById(id);
            if (employee == null) return NotFound();

            ES.DeleteEmployee(id);
            TempData["Message"] = $"��������� � ������� {id} � ������ {employee.FullName} �����.";
            return RedirectToAction("List");
        }

        [HttpGet]
        public JsonResult GetPositions(Department department)
        {
            var positions = new Dictionary<Department, List<string>>
            {
                {Department.�����_��, new List<string> {"���������� ��", "�����������������", "�������� �����������"}},
                {Department.����������_����������, new List<string> {"HR ����������", "HR ��������", "����������� �� ������� ���������"}},
                {Department.�������, new List<string> {"�������� �� ��������", "�������� �� ��������", "�������� �� ������ � ���������"}},
                {Department.�������������, new List<string> {"���� ��������", "�������� ������������", "���������"}}
            };

            var result = positions.ContainsKey(department) ? positions[department] : new List<string>();
            return Json(result);
        }

        private void GetSelectLists()
        {
            ViewBag.DepartmentOptions = new SelectList(Enum.GetValues(typeof(Department)).Cast<Department>());
            ViewBag.EmployeeTypeOptions = new SelectList(Enum.GetValues(typeof(EmployeeType)).Cast<EmployeeType>());
        }
    }
}