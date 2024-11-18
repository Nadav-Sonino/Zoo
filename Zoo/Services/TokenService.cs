using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zoo.Models;

namespace Zoo.Services
{
    public class TokenService : ITokenService
    {
        //אחראי לקבלת מידע מהאפ-סטינגס
        private readonly IConfiguration configuration;

        //המפתח המוצפן
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;

            //מייצר מפתח חדש ע"י קידוד הסטרינג לבייטים כי זו הדרישה - מי שאחראי לקידוד נקרא
            //SymmetricSecurityKey
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
        }

        public string CreateToken(Admin admin)
        {
            //Obj inside token = CLAIMS
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.UserName!),
                new Claim(ClaimTypes.Role, "Admin")
            };

            //מי שעושה את החתימה המיוחד עם אלגוריתם לבחירתי
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //הטוקן עצמו
            var token = new JwtSecurityToken
            (
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            //המרה לסטרינג
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
