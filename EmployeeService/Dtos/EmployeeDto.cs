using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EmployeeService.Dtos
{
    [DataContract]
    public class EmployeeDto
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public int? ManagerID { get; set; }

        [DataMember] public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
