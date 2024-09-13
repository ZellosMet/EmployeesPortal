using static EmployeesPortal.Models.EmployeeEnums;

namespace EmployeesPortal.Models
{
	public class EmployeeService
	{
		private static List<Employee> Employees = new List<Employee>()
		{ 
			new Employee
			{
				ID=1, 
				FullName = "Иван Петрович Черненко", 
				Email="Ivan@chernenko.com", 
				Position = "Программист", 
				Departament = Departament.IT, 
				HireData = DateTime.Now.AddYears(-3), 
				DateOfBirth = DateTime.Now.AddYears(-30),
				Type = EmployeeType.Permanent,
				Pol = "МУЖ",
				Salary = 60000
			},
			new Employee
			{
				ID=2,
				FullName = "Мухаммад Абрикосович Черешнев",
				Email="muha@bubka.com",
				Position = "Менеджер",
				Departament = Departament.HR,
				HireData = DateTime.Now.AddYears(-5),
				DateOfBirth = DateTime.Now.AddYears(-36),
				Type = EmployeeType.Permanent,
				Pol = "МУЖ",
				Salary = 80000
			},
			new Employee
			{
				ID=3,
				FullName = "Антон Антонович Антонов",
				Email="aaa@aaa.com",
				Position = "Администратор",
				Departament = Departament.Admin,
				HireData = DateTime.Now.AddYears(-4),
				DateOfBirth = DateTime.Now.AddYears(-25),
				Type = EmployeeType.Intern,
				Pol = "МУЖ",
				Salary = 40000
			},
			new Employee
			{
				ID=4,
				FullName = "Антонина Петровна Шпак",
				Email="Anntonovna@shpak.com",
				Position = "Продажи",
				Departament = Departament.Sales,
				HireData = DateTime.Now.AddYears(-6),
				DateOfBirth = DateTime.Now.AddYears(-20),
				Type = EmployeeType.Contract,
				Pol = "ЖЕН",
				Salary = 50000
			},
            new Employee
            {
                ID=5,
                FullName = "Фёдор Сергеевич Фролов",
                Email="Serg@frolov.com",
                Position = "Администратор",
                Departament = Departament.Admin,
                HireData = DateTime.Now.AddYears(-8),
                DateOfBirth = DateTime.Now.AddYears(-36),
                Type = EmployeeType.Permanent,
                Pol = "МУЖ",
                Salary = 100000
            },
            new Employee
            {
                ID=6,
                FullName = "Александр Александрович Легостаев",
                Email="AA@legostaev.com",
                Position = "Администратор",
                Departament = Departament.Admin,
                HireData = DateTime.Now.AddYears(-30),
                DateOfBirth = DateTime.Now.AddYears(-65),
                Type = EmployeeType.Permanent,
                Pol = "МУЖ",
                Salary = 100000
            },
            new Employee
            {
                ID=7,
                FullName = "Никита Сергеевич Сивков",
                Email="Nic@siv.com",
                Position = "Программист",
                Departament = Departament.IT,
                HireData = DateTime.Now.AddYears(-1),
                DateOfBirth = DateTime.Now.AddYears(-32),
                Type = EmployeeType.Temporary,
                Pol = "МУЖ",
                Salary = 80000
            },
            new Employee
            {
                ID=8,
                FullName = "Анна Александровна Линейцева",
                Email="Anna@lin.com",
                Position = "Менеджер",
                Departament = Departament.HR,
                HireData = DateTime.Now.AddYears(-10),
                DateOfBirth = DateTime.Now.AddYears(-45),
                Type = EmployeeType.Contract,
                Pol = "ЖЕН",
                Salary = 95000
            },
            new Employee
            {
                ID=9,
                FullName = "Марина Андреевна Шорстова",
                Email="Mar@shorst.com",
                Position = "Менеджер",
                Departament = Departament.HR,
                HireData = DateTime.Now.AddYears(-20),
                DateOfBirth = DateTime.Now.AddYears(-53),
                Type = EmployeeType.Contract,
                Pol = "ЖЕН",
                Salary = 95000
            },
            new Employee
            {
                ID=10,
                FullName = "Ольга Николаевна Горбачёва",
                Email="Olga@Gorb.com",
                Position = "Администратор",
                Departament = Departament.Admin,
                HireData = DateTime.Now.AddYears(-15),
                DateOfBirth = DateTime.Now.AddYears(-48),
                Type = EmployeeType.Permanent,
                Pol = "ЖЕН",
                Salary = 150000
            }
        };

        public async Task<(List<Employee> employees, int total_count)> GetEmployee(string search_term, string selected_department, string selected_type, int page_number, int page_size)
        {
            var filtered_employees = Employees.AsQueryable();
            if (!string.IsNullOrEmpty(search_term))
            {
                filtered_employees = filtered_employees.Where(p => p.FullName.Contains(search_term, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(selected_department))
            {
                if (Enum.TryParse(selected_department, out Departament departament))
                {
                    filtered_employees = filtered_employees.Where(p => p.Departament == departament);
                }
            }
            if (!string.IsNullOrEmpty(selected_type))
            {
                if (!Enum.TryParse(selected_type, out EmployeeType type))
                {
                    filtered_employees = filtered_employees.Where(p => p.Type == type);
                }
            }
            int total_count = filtered_employees.Count();
            filtered_employees = filtered_employees.Skip((page_number - 1) * page_size).Take(page_size);
            return await Task.FromResult((filtered_employees.ToList(), total_count));
        }
        public Employee? GetEmployeeById(int id)
        {
            return Employees.FirstOrDefault(e => e.ID == id);
        }
        public void CreateEmployee(Employee employee)
        {
            employee.ID = Employees.Max(e => e.ID) + 1;
            Employees.Add(employee);
        }
        public void UpdateEmployee(Employee employee)
        {
            var existing_employee = GetEmployeeById(employee.ID);
            if (existing_employee != null)
            {
                existing_employee.FullName = employee.FullName;
                existing_employee.Email = employee.Email;
                existing_employee.Position = employee.Position;
                existing_employee.Departament = employee.Departament;
                existing_employee.HireData = employee.HireData;
                existing_employee.DateOfBirth = employee.DateOfBirth;
                existing_employee.Type = employee.Type;
                existing_employee.Pol = employee.Pol;
                existing_employee.Salary = employee.Salary;
            }
        }
        public void DeleteEmployee(int id)
        { 
            var employee = GetEmployeeById(id);
            if (employee != null)
            { 
                Employees.Remove(employee);
            }
        }
	}
}
