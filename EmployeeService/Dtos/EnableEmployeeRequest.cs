using System.Runtime.Serialization;

namespace EmployeeService.Dtos
{
    [DataContract]
    public class EnableEmployeeRequest
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int Enable { get; set; }
    }
}
