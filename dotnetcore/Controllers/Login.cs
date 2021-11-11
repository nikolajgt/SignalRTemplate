using dotnetcore.Intefaces;
using Microsoft.AspNetCore.Mvc;
using dotnetcore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Login
    {
        private readonly IMongoConnection _repository;

        public Login(IMongoConnection repository)
        {
            _repository = repository;
        }

 
        [HttpPost]
        public ActionResult LoginUser([FromBody] UserLoginDTO user)
        {
            var token = _repository.Authenticate(user.Username, user.Password);
            if (token == null)
                return new UnauthorizedObjectResult(token);

            return new OkObjectResult(token);
        }

    }
}
