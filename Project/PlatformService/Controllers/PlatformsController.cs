using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.SyncDataServices.Http;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly IHttpCommandDataClient _httpCommandDataClient;

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper,
            IHttpCommandDataClient httpCommandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _httpCommandDataClient = httpCommandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platforms = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repository.GetPlatformById(id);
            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
             try 
            {
              await _httpCommandDataClient.SendRequestToCommandsService(platformReadDto);
            }
            catch(Exception ex)
            {
                 Console.WriteLine("Can not send request to commands service :" + ex.Message);
            }
            
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}