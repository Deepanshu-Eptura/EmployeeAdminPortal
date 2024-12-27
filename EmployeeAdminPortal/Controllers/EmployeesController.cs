using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    //localhost:xxxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // using this private we can access our Dbcontext
        private readonly ApplicationDbContext dbContext;

        //connecting to database
        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //  READ method
        [HttpGet]
        public IActionResult GetAllEmployees()
        {   // 1st approach
            //var allEmployees = dbContext.Employees.ToList();
            //return Ok(allEmployees);

            return Ok(dbContext.Employees.ToList()); //2nd approach
        }

        // get by id 
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
            {
                return NotFound();  // 404 respone i.e. NOT FOUND
            }
            return Ok(employee);
        }

        [HttpPost]
        // for this we require Dto in Models  i.e Data Transfer Object
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            // converting into Entity
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary

            };

            dbContext.Employees.Add(employeeEntity);

            dbContext.SaveChanges();

            return Ok(employeeEntity);  // 200 respone
        }

        // Update method

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
            {
                return NotFound();  // 404 respone i.e. NOT FOUND
            }

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            dbContext.SaveChanges();

            return Ok(employee);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id) 
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges(); 
            return Ok();
        }
    }
}
