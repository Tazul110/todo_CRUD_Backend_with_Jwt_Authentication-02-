using Microsoft.Data.SqlClient;
using Todo_backEnd.Model.LogIn_UsersModel;
using Todo_backEnd.Model.LogInResponseModel;

namespace Todo_backEnd.Service.LogInServiseInterface
{
    public interface IGetUserByEmailServ
    {
        //LogInResponse sGetUser(SqlConnection connection, string email);
        Users AuthenticateUser(SqlConnection connection, Users user);
    }
}
