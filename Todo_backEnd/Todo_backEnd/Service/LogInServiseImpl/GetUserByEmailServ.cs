using Microsoft.Data.SqlClient;
using Todo_backEnd.Model.LogIn_UsersModel;
using Todo_backEnd.Model.LogInResponseModel;
using Todo_backEnd.Repository.LogInRepositoryInterface;
using Todo_backEnd.Service.LogInServiseInterface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Todo_backEnd.Service.LogInServiseImpl
{
    public class GetUserByEmailServ : IGetUserByEmailServ
    {
        private readonly IGetUserByEmail _iGetUserByEmail;
        public GetUserByEmailServ(IGetUserByEmail IGetRepo)
        {
            _iGetUserByEmail = IGetRepo;
        }






        public Users AuthenticateUser(SqlConnection connection, Users user)
        {
           Users rs = _iGetUserByEmail.GetBy_Email(connection, user.userEmail);

            if (rs != null && user.userPassword == rs.userPassword)
            {
                return rs;
            }

            return null;
        }

        
    }
}
