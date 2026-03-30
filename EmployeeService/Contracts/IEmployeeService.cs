using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using EmployeeService.Dtos;

namespace EmployeeService.Contracts
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetEmployeeById?id={id}",
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "EnableEmployee?id={id}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Task EnableEmployeeAsync(int id, int enable);
    }
}
