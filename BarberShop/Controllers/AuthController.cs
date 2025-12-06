using AutoMapper;
using BarberShop.Data;
using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BarberShopContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(BarberShopContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.userName == request.userName);

            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                userName = request.userName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                roleID = request.roleID
                // Populate other properties as needed
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }
        [HttpPut("update")]
        public async Task<ActionResult<User>> Update(UserDto request)
        {
            // 1️⃣ Tìm user theo ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userID == request.userID);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // 2️⃣ Cập nhật tên user nếu có
            if (!string.IsNullOrEmpty(request.userName))
            {
                user.userName = request.userName;
            }

            // 3️⃣ Cập nhật role nếu có
            user.roleID = request.roleID;

            // 4️⃣ Cập nhật password nếu có
            if (!string.IsNullOrEmpty(request.password))
            {
                CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            // 5️⃣ Lưu vào database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userName == request.userName);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            return Ok(token);
        }
            
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.userName),
        new Claim(ClaimTypes.Role, user.roleID.ToString()),
        new Claim("userID", user.userID.ToString(), ClaimValueTypes.Integer)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private ClaimsPrincipal ValidateToken(string jwt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Optional: Set clock skew to zero to account for time differences between the server and client
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);

            return principal;
        }

        // Modify CreatePasswordHash to accept and return strings
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}


