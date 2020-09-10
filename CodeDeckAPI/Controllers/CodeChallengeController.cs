using System.Collections.Generic;
using AutoMapper;
using CodeDeckAPI.Data;
using CodeDeckAPI.Dtos;
using CodeDeckAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeDeckAPI.Controllers
{
    // /api/CodeDeck
    [ApiController]
    [Route("api/CodeDeck")]
    public class CodeChallengeController : ControllerBase
    {
        private readonly ICodeChallengeRepo _repo;
        private readonly IMapper _mapper;

        public CodeChallengeController(ICodeChallengeRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET /api/CodeDeck
        [HttpGet]
        public ActionResult <IEnumerable<CodeChallengeReadDto>> GetAllChallenges()
        {
            var challengeItems = _repo.GetAllChallenges();

            return Ok(_mapper.Map<IEnumerable<CodeChallengeReadDto>>(challengeItems));
        }

        // GET /api/CodeDeck/{id}
        [HttpGet("{id}", Name="GetCodeChallengeById")]
        public ActionResult <CodeChallengeReadDto> GetCodeChallengeById(int id)
        {
            var challengeItem = _repo.GetCodeChallengeById(id);

            if(challengeItem != null)
            {
                return Ok(challengeItem);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult <CodeChallengeReadDto> CreateCodeChallenge(CodeChallengeCreateDto codeChallengeCreateDto)
        {
            var codeChallengeModel = _mapper.Map<CodeChallenge>(codeChallengeCreateDto);
            _repo.CreateCodeChallenge(codeChallengeModel);
            _repo.SaveChanges();

            var codeChallengeReadDto = _mapper.Map<CodeChallengeReadDto>(codeChallengeModel);

            return CreatedAtRoute(nameof(GetCodeChallengeById), new {Id = codeChallengeReadDto.Id}, codeChallengeReadDto);
        }
    }
}