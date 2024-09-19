using EmployeePortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService ES;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
       public EmployeeController(EmployeeService employee_service, UserManager<User> userManager, SignInManager<User> signInManager)
       {
            ES = employee_service;
            _userManager = userManager;
            _signInManager = signInManager;
       }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string SearchTerm, [FromQuery] string SelectedDepartment, [FromQuery] string SelectedType, [FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {           
            var (employees, totalCount) = await ES.GetEmployees(SearchTerm, SelectedDepartment, SelectedType, PageNumber, PageSize);
            var viewModel = new EmployeeListViewModel
            {
                Employees = employees,
                TotalPages = (int)Math.Ceiling((double)totalCount / PageSize),
                SearchTerm = SearchTerm,
                SelectedDepartment = SelectedDepartment,
                SelectedType = SelectedType,
                PageSize = PageSize,
                PageNumber = PageNumber
            };

            GetSelectLists();
            ViewBag.PageSizeOptions = new SelectList(new List<int> { 3, 5, 10, 15, 20, 25 }, PageSize);
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
       // public JsonResult GetPositions(Department department)
        public JsonResult GetPositions(string department)
        {
            //var positions = new Dictionary<Department, List<string>>
            var positions = new Dictionary<string, List<string>>
            {
                //{Department.�����_��, new List<string> {"���������� ��", "�����������������", "�������� �����������"}},
                //{Department.��������, new List<string> {"HR ����������", "HR ��������", "����������� �� ������� ���������"}},
                //{Department.�������, new List<string> {"�������� �� ��������", "�������� �� ��������", "�������� �� ������ � ���������"}},
                //{Department.�������������, new List<string> {"���� ��������", "�������� ������������", "���������"}}

                {"�����_��", new List<string> {"���������� ��", "�����������������", "�������� �����������"}},
                {"��������", new List<string> {"HR ����������", "HR ��������", "����������� �� ������� ���������"}},
                {"�������", new List<string> {"�������� �� ��������", "�������� �� ��������", "�������� �� ������ � ���������"}},
                {"�������������", new List<string> {"���� ��������", "�������� ������������", "���������"}}
            };

            var result = positions.ContainsKey(department) ? positions[department] : new List<string>();
            return Json(result);
        }
        private void GetSelectLists()
        {
            ViewBag.DepartmentOptions = new SelectList(Enum.GetValues(typeof(Department)).Cast<Department>());
            ViewBag.EmployeeTypeOptions = new SelectList(Enum.GetValues(typeof(EmployeeType)).Cast<EmployeeType>());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Employee", "List");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}