using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Todo_backEnd.Model.LogIn_UsersModel;

using Todo_backEnd.Repository.LogInRepositoryInterface;
using Todo_backEnd.Service.LogInServiseInterface;

namespace Todo_backEnd.Controllers.LogInController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IGetUserByEmailServ _user;
        

        public LoginController(IConfiguration configuration, IGetUserByEmailServ user)
        {
            _config = configuration;
            _user = user;
           
        }



        private string GenerateToken(Users user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("CrudConnection"));
            IActionResult response = Unauthorized();


            var user_ = _user.AuthenticateUser(connection,user);
            if (user_ != null)
            {
                var token = GenerateToken(user_);
                response = Ok(new { token = token, message = "valid credentials" });
            }
            /*else
            {
                response = BadRequest(new { message = "Try Again... Invalid user or Invalid password" });
            }*/
            return response;
        }
    }
}
