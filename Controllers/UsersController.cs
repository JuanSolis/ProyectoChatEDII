using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Services;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ActionResult<List<User>> Get() => _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("add")]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPost("verifyAccount")]
        public ActionResult verify(User user)
        {

            var userFound = _userService.GetByUsername(user.Username);

            if (userFound.Password.Equals(user.Password))
            {
                return CreatedAtRoute("GetUser", new { id = userFound.Id.ToString() }, userFound);
            }else {
                return BadRequest();
            }

            
        }
    }
}
