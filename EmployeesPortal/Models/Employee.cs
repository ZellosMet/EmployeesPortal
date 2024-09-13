using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static EmployeesPortal.Models.EmployeeEnums;
namespace EmployeesPortal.Models
{
	public class Employee
	{
		public int ID { get; set; }

		[Required(ErrorMessage = "Неверное имя.")]
		[StringLength(100, ErrorMessage = "Полное имя не должно содержать более 100 символов.")]
		[Display(Name = "Имя")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Неверный адрес почты.")]
		[EmailAddress(ErrorMessage = "Проверьте адрес электронной почты.")]
		[Display(Name = "Электронная почта")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Ошибка в наименовании должности.")]
		[StringLength(50, ErrorMessage = "В наименовании должности не может быть больше 50 символов.")]
		public string Position { get; set; }

		[Required(ErrorMessage = "Ошибка в наименовании подразделения.")]
		[StringLength(50, ErrorMessage = "В наименовании должности не может быть больше 50 символов.")]
		public Departament? Departament { get; set; }

		[Required(ErrorMessage = "Неверная дата приёма на работу.")]
		[Display(Name = "Дата приёма на работу")]
		[DataType(DataType.Date, ErrorMessage = "Неверный формат даты")]
		public DateTime? HireData { get; set; }
		[Required(ErrorMessage = "Неверная дата рождения.")]
		[Display(Name = "Дата рождения")]
		public DateTime? DateOfBirth { get; set; }

		[Required(ErrorMessage = "Ошибка вида занятости.")]
		[Display(Name = "Тип занятости")]
		public EmployeeType? Type { get; set; }

		[Required(ErrorMessage = "Неверно указан пол.")]
		[StringLength(3, ErrorMessage = "Укажите Муж или Жен.")]
		public string Pol { get; set; }

		[Required(ErrorMessage = "Ошибка в сумме заработка.")]
		[Range(0, double.MaxValue, ErrorMessage = "Зарплата не может быть отрицательное")]
		[DataType(DataType.Currency)]
		public decimal? Salary { get; set; }
	}
}
