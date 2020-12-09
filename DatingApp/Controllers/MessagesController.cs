using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Data;
using System.Security.Claims;
using DatingApp.Dtos;
using DatingApp.Core;
using DatingApp.Data.Helpers;

namespace DatingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDataRepository datingRepository;
        private readonly IMapper mapper;

        public MessagesController(IDataRepository datingRepository, IMapper mapper)
        {
            this.datingRepository = datingRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var messageFromRepo = await datingRepository.GetMessage(id);
            if(messageFromRepo == null)
            {
                NotFound();
            }
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await datingRepository.GetMessagesForUser(messageParams);

            var messages = mapper.Map<IEnumerable<MessageForReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recepientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesRepo = await datingRepository.GetMessageThread(userId, recipientId);

            var messageThread = mapper.Map<IEnumerable<MessageForReturnDto>>(messagesRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            messageForCreation.SenderId = userId;

            var recipient = await datingRepository.GetUser(messageForCreation.RecipientId);

            if(recipient == null)
            {
                return BadRequest("Could not find user");
            }
            var message = mapper.Map<Message>(messageForCreation);

            datingRepository.Add(message);

            if(await datingRepository.SaveAll())
            {
                return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageForCreation);
            }
            throw new Exception("Creating the massage failed on save");
        }

    }
}
