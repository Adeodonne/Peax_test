using System.Net;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using EmployeeService.Contracts;
using EmployeeService.Dtos;
using Infrastructure.Contract;

namespace EmployeeService.Services
{
    public class Service1 : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public Service1(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                throw new WebFaultException<string>(
                    "Employee not found",
                    HttpStatusCode.NotFound);
            }

            return employee;
        }

        public async Task EnableEmployeeAsync(EnableEmployeeRequest request)
        {
            var updated = await _employeeRepository.EnableEmployeeAsync(request.Id, request.Enable);

            if (!updated)
            {
                throw new WebFaultException<string>(
                    "Employee not found",
                    HttpStatusCode.NotFound);
            }
        }
    }
}
