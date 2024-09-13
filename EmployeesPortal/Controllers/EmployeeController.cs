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
				TempData["���������"] = $"��������� ����� {employee.ID} � � ������ {employee.FullName} �������.";
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
			TempData["���������"] = $"��������� ����� {employee.ID} � � ������ {employee.FullName} �����.";
			return RedirectToAction("List");
		}

		[HttpGet]
		public JsonResult GetPositions(Department department)
		{
			var Positions = new Dictionary<Department, List<string>> 
			{
				{ Department.IT, new List<string> {"���������� ��", "��������� �����������������", "������� ����������������"}},
				{ Department.HR, new List<string> {"���������� �� ������", "�������� �� ������", "�����������"}},
				{ Department.Sales, new List<string> {"�������� �� ��������", "���������� �� ��������", "��������� ������"}},
				{ Department.Admin, new List<string> {" ����-��������", "��������", "�������� ���������"}}				
			};			
		}
	}
}
