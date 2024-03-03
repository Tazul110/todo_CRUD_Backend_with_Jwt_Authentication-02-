using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly IGetUserByEmailServ _userServ;
        

        public LoginController(IConfiguration configuration, IGetUserByEmailServ user)
        {
            _config = configuration;
            _userServ = user;
           
        }



        /*private string GenerateToken(Users user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

        private string GenerateToken(Users user, bool isRefreshToken = false)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.userEmail),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // Add additional claims if needed
    };

            var expirationMinutes = Convert.ToDouble(_config[isRefreshToken ? "Jwt:RefreshTokenExpirationInMinutes" : "Jwt:TokenExpirationInMinutes"]);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("CrudConnection"));
            IActionResult response = Unauthorized();

            var authenticatedUser = _userServ.AuthenticateUser(connection, user);

            if (authenticatedUser != null)
            {
                var token = GenerateToken(authenticatedUser);
                var refreshToken = GenerateToken(authenticatedUser, true);

                response = Ok(new { token = token, refreshToken = refreshToken, message = "valid credentials" });
            }
            else
            {
                response = BadRequest(new { message = "Try Again... Invalid user or Invalid password" });
            }

            return response;
        }
        /* public IActionResult Login(Users user)
         {
             SqlConnection connection = new SqlConnection(_config.GetConnectionString("CrudConnection"));
             IActionResult response = Unauthorized();


             var user_ = _userServ.AuthenticateUser(connection,user);
             if (user_ != null)
             {
                 var token = GenerateToken(user_);
                 response = Ok(new { token = token, message = "valid credentials" });
             }
             *//*else
             {
                 response = BadRequest(new { message = "Try Again... Invalid user or Invalid password" });
             }*//*
             return response;
         }*/
    }
}
