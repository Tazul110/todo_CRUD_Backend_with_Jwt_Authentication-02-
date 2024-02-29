using Microsoft.Data.SqlClient;
using Todo_backEnd.Model.LogIn_UsersModel;
using Todo_backEnd.Model.LogInResponseModel;

namespace Todo_backEnd.Repository.LogInRepositoryInterface
{
    public interface IGetUserByEmail
    {
        Users GetBy_Email(SqlConnection connection, string email);
    }
}
