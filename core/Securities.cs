using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using PracticeWeb.Interface;
using BCryptTool = BCrypt.Net.BCrypt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
namespace PracticeWeb.core
{
    public class Security : IHasher
    {
        private readonly IHashids _hashids;
        public Security(IHashids hashids) => _hashids = hashids;
    public string HashPassword(string password)
        => BCryptTool.HashPassword(password, workFactor: 12);
    public bool VerifyPassword(string password, string hashPassword)
        => BCryptTool.Verify(password, hashPassword);
    
    public string CreateToken(int Userid)
        {
            DotNetEnv.Env.Load();
            var keyString = Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new InvalidOperationException("JWT Secret Key is missing in .env");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Userid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Ticket lasts 2 hours
                SigningCredentials = creds,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    public string CreateHashids(int GroupId)
        {
            return _hashids.Encode(GroupId);
        }
    public int DecodeHashids(string hash)
        {
            int[] DecodedArray = _hashids.Decode(hash);
            if (DecodedArray.Length == 0) throw new InvalidCredentialException("Invalid Id");
            int GroupId = DecodedArray[0];
            return GroupId;
        }
    }
}
