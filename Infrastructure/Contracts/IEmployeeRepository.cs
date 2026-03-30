using System.Threading.Tasks;
using EmployeeService.Dtos;

namespace Infrastructure.Contract
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task<bool> EnableEmployeeAsync(int id, int enable);
    }
}
