using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using dotnetcore.Intefaces;
using dotnetcore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace dotnetcore.repositories
{
    public class MongoConnection : IMongoConnection 
    {
        private readonly IMongoCollection<UserLogin> _repository;
        private readonly string _dbName = "InvestorHelper";
        private readonly string _dbCollection = "UserData";

        private readonly string key;

        // Depdendency Injection 
        public MongoConnection(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase(_dbName);
            _repository = database.GetCollection<UserLogin>(_dbCollection);
            this.key = config.GetSection("JwtKey").ToString();
        }

        public string Authenticate(string username, string password)
        {
            var user = _repository.Find(x => x.Username == username && x.Password == password).FirstOrDefault();

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = System.DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
