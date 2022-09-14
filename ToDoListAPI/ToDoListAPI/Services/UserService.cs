using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToDoListAPI.IService;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class UserService : IUserService
    {
        private MyDBContext _context;
        private IConfiguration _configuration;

        public UserService(MyDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> CreateUser(User user)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Email == user.Email);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            if (checkUser != null)
            {
                return null;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string> Login(string username, string pass)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(pass);
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username && user.Password == passwordHash);
            bool verified = BCrypt.Net.BCrypt.Verify(user.Password, passwordHash);
            if (user == null && verified == false)
            {
                return null;
            }
            //GenerateToken token
            return GenerateToken(user);
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //Prepare the user information that needs to be saved in claims
            var claims = new List<Claim>() {
                new Claim (ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
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