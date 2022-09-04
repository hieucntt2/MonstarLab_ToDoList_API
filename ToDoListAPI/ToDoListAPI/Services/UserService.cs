using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class UserService : IUserServices
    {
        private MyDBContext _context;
        private IConfiguration _configuration;

        public UserService(MyDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string> CreateUser(User user)
        {
            //Kiem tra user name duy nhat
            var checkUserName = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            //Khác null nghĩa là đã tồn tại
            if (checkUserName != null)
            {
                return "01";
            }
            //Chưa tồn tại thì thêm
            else
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "00";
            }
        }

        public async Task<string> Login(string username, string pass)
        {
            var res = "";
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username && user.Password == pass);
            if (user == null)
            {
                res = "01";
            }
            else
            {
                //GenerateToken token
                res = GenerateToken(user);
            }

            return res;
        }

        //Kiểm tra token hợp lệ
        public string VerifyToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            try
            {
                validationParameters.ValidateLifetime = true;
                validationParameters.ValidAudience = _configuration["Jwt:Issuer"].ToLower();
                validationParameters.ValidIssuer = _configuration["Jwt:Issuer"].ToLower();
                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);
                return "00";
            }
            catch
            {
                return "01";
            }
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //Prepare the user information that needs to be saved in claims
            var claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim ("UserId", user.UserId.ToString()),
            };

            //Get timeout from JWt configuration in application.json
            double timeout;
            if (!double.TryParse(_configuration["Jwt:Timeout"], out timeout))
            {
                timeout = 30;
            }

            //Generate required keys from JWt configuration in application.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(timeout),
                SigningCredentials = credentials
            };

            //Proceed to generate tokengt
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string strToken = tokenHandler.WriteToken(token);
            return strToken;
        }
    }
}