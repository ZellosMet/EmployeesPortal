using EmployeesPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static EmployeesPortal.Models.EmployeeEnums;

namespace EmployeesPortal.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly EmployeeService ES;
		public EmployeeController(EmployeeService employee_service)
		{
			ES = employee_service;
		}

		[HttpGet]
		public async Task<IActionResult> List([FromQuery] string SearchTerm, [FromQuery] string SelectedDepartament, [FromQuery] string SelectType, [FromQuery] int PageNimber = 1, [FromQuery] int PageSize = 5)
		{
			var (employees, totalCount) = await ES.GetEmployee(SearchTerm, SelectedDepartament, SelectType, PageNimber, PageSize);
			var viewModel = new EmployeeListViewModel
			{
				Employees = employees,
				TotalPages = (int)Math.Ceiling((double)totalCount / PageSize),
				SearchTerm = SearchTerm,
				SelectedDepartament = SelectedDepartament,
				SelectedType = SelectType,
				PageSize = PageSize,
				PageNumber = PageNimber

			};

			GetSelectedLists();
			ViewBag.PageSizeOptioons = new SelectList(new List<int> { 3, 5, 10, 15, 20, 25 }, PageSize);
			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			GetSelectedLists();
			return View();
		}

		[HttpPost]
		public IActionResult Create([FromQuery] Employee employee)
		{
			if (ModelState.IsValid)
			{
				ES.CreateEmployee(employee);
				return RedirectToAction("Success", new { id = employee.ID });
			}

			GetSelectedLists();
			return View(employee);
		}

		public IActionResult Success([FromRoute] int id)
		{
			var employee = ES.GetEmployeeById(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(employee);
		}

		[HttpGet]
		public IActionResult Update([FromRoute] int id)
		{
			var employee = ES.GetEmployeeById(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(employee);
		}

		[HttpPost]
		public IActionResult Update([FromRoute] Employee employee)
		{
			if (ModelState.IsValid)
			{
				ES.UpdateEmployee(employee);
				TempData["Сообщение"] = $"Сотрудник номер {employee.ID} и с именем {employee.FullName} обновлён.";
				return RedirectToAction("List");
			}

			GetSelectedLists();
			return View(employee);
		}

		[HttpGet]
		public IActionResult Delete([FromRoute] int id)
		{
			var employee = ES.GetEmployeeById(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(employee);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfifmed([FromRoute] int id)
		{
			var employee = ES.GetEmployeeById(id);
			if (employee == null)
			{
				return NotFound();
			}
			ES.DeleteEmployee(id);
			TempData["Сообщение"] = $"Сотрудник номер {employee.ID} и с именем {employee.FullName} удалён.";
			return RedirectToAction("List");
		}

		[HttpGet]
		public JsonResult GetPositions(Department department)
		{
			var Positions = new Dictionary<Department, List<string>> 
			{
				{ Department.IT, new List<string> {"Разработка ПО", "Системное администрирование", "Сетевое администрироание"}},
				{ Department.HR, new List<string> {"Специалист по кадрам", "Менеджер по кадрам", "Координатор"}},
				{ Department.Sales, new List<string> {"Менеджер по продажам", "Специолист по продажам", "Начальник отдела"}},
				{ Department.Admin, new List<string> {" Офис-менеджер", "Асистент", "Служащий ресепшина"}}				
			};			
		}
	}
}
