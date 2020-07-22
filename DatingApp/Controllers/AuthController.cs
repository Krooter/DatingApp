using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core;
using DatingApp.Data;
using DatingApp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authorizeData;
        private readonly IConfiguration _config;
        private readonly IMapper mapper;

        public AuthController(IAuthRepository authorizeData, IConfiguration config, IMapper mapper)
        {
            _authorizeData = authorizeData;
            _config = config;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _authorizeData.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = mapper.Map<User>(userForRegisterDto);

            var createdUser = await _authorizeData.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new { Controller = "User", id = createdUser }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _authorizeData.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new 
            { 
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}
