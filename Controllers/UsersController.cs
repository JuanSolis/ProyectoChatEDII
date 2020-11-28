using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Services;
using ChatApp.Models;
using CifradoCesar;

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
            Cesar cifrarPassword = new Cesar();

            string passsCifrada = cifrarPassword.Cifrar(user.Password);

            var result = _userService.GetByUsername(user.Username);

            if (result == null)
            {
                user.Password = passsCifrada;
                _userService.Create(user);
                return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
            }
            else {
                return BadRequest();
            }
            

            
        }

        [HttpPost("verifyAccount")]
        public ActionResult verify(User user)
        {
            Cesar descifrar = new Cesar();
            var userFound = _userService.GetByUsername(user.Username);

            string passDescif = descifrar.Cifrar(user.Password);



            if (userFound != null)
            {
                if (userFound.Password.Equals(passDescif))
                {
                    return CreatedAtRoute("GetUser", new { id = userFound.Id.ToString() }, userFound);
                }
                else
                {
                    return BadRequest();
                }
            }
            else {
                return BadRequest();
            }
            

            
        }
    }
}
