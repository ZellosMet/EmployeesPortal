using EmployeePortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;

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
                TempData["Message"] = $"Сотрудник с номером {employee.Id} и именем {employee.FullName} обнавлён.";
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
            TempData["Message"] = $"Сотрудник с номером {id} и именем {employee.FullName} удалён.";
            return RedirectToAction("List");
        }

        [HttpGet]
       // public JsonResult GetPositions(Department department)
        public JsonResult GetPositions(string department)
        {
            //var positions = new Dictionary<Department, List<string>>
            var positions = new Dictionary<string, List<string>>
            {
                //{Department.Отдел_ИТ, new List<string> {"Разработка ПО", "Администрирование", "Сетевого обеспечения"}},
                //{Department.Менеджер, new List<string> {"HR Специалист", "HR Менеджер", "Координатор по подбору персонала"}},
                //{Department.Продажи, new List<string> {"Директор по продажам", "Менеджер по продажам", "Менеджер по работе с клиентами"}},
                //{Department.Администрация, new List<string> {"Офис менеджер", "Помощник руководителя", "Секретарь"}}

                {"Отдел_ИТ", new List<string> {"Разработка ПО", "Администрирование", "Сетевого обеспечения"}},
                {"Менеджер", new List<string> {"HR Специалист", "HR Менеджер", "Координатор по подбору персонала"}},
                {"Продажи", new List<string> {"Директор по продажам", "Менеджер по продажам", "Менеджер по работе с клиентами"}},
                {"Администрация", new List<string> {"Офис менеджер", "Помощник руководителя", "Секретарь"}}
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
                    return RedirectToAction("List", "Employee");
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

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
           if (ModelState.IsValid)
           {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {                    
                    return RedirectToAction("List", "Employee");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
           }
           return View(model);           
        }
    }
}