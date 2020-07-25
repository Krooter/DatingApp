using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Data;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using DatingApp.Dtos;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using DatingApp.Core;
using SQLitePCL;

namespace DatingApp.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly IMapper mapper;
        private readonly IOptions<ClaudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;

        public PhotosController(IDataRepository dataRepository, IMapper mapper, IOptions<ClaudinarySettings> cloudinaryConfig)
        {
            this.dataRepository = dataRepository;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                this.cloudinaryConfig.Value.CloudName,
                this.cloudinaryConfig.Value.ApiKey,
                this.cloudinaryConfig.Value.ApiSecret
            );

            this.cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await dataRepository.GetPhoto(id);

            var photo = mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotosFoUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await dataRepository.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();
            var uploadResultAvatar = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResultAvatar = cloudinary.Upload(uploadParams);
                }
                using (var originStream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, originStream),
                    };
                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.UrlPhoto = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            photoForCreationDto.UrlPhotoAvatar = uploadResultAvatar.Url.ToString();
            photoForCreationDto.PublicIdAvatar = uploadResultAvatar.PublicId;

            var photo = mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);

            if (await dataRepository.SaveAll())
            {
                var photoToReturn = mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> MainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await dataRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photoForRepo = await dataRepository.GetPhoto(id);

            if (photoForRepo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }

            var currentMainPhoto = await dataRepository.GetMainPhoto(userId);

            if( currentMainPhoto == null)
            {
                photoForRepo.IsMain = true;

            } 
            else
            {
                currentMainPhoto.IsMain = false;

                photoForRepo.IsMain = true;
            }

            if (await dataRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await dataRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photoForRepo = await dataRepository.GetPhoto(id);
            var photoForRepoAvatar = await dataRepository.GetPhoto(id);

            if (photoForRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoForRepo.PublicId);
                var deleteParamsAvatar = new DeletionParams(photoForRepo.PublicIdAvatar);

                var result = cloudinary.Destroy(deleteParams);
                var resultAvatar = cloudinary.Destroy(deleteParamsAvatar);

                if (result.Result == "ok")
                {
                    dataRepository.Delete(photoForRepo);
                }
            }
            
            if(photoForRepo.PublicId == null)
            {
                dataRepository.Delete(photoForRepo);
            }

            if(await dataRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }
    }
}
