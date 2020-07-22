using AutoMapper;
using DatingApp.Core;
using DatingApp.Data;
using DatingApp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly IMapper mapper;

        public UserController(IDataRepository dataRepository, IMapper mapper)
        {
            this.dataRepository = dataRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await dataRepository.GetUsers();
            var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await dataRepository.GetUser(id);
            var userToReturn = mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await dataRepository.GetUser(id);

            mapper.Map(userForUpdateDto, userFromRepo);

            if(await dataRepository.SaveAll())
            {
                return NoContent();
            }
            throw new Exception($"Updating user {id} failed on saved!");
        }
    }
}
