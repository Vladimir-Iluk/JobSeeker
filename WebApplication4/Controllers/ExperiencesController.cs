using Bll.Dto.Experience;
using Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperienceService _experienceService;

        public ExperiencesController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        /// <summary>
        /// Отримати досвід роботи за ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperienceReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExperienceReadDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _experienceService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Отримати весь досвід роботи шукача роботи
        /// </summary>
        [HttpGet("jobseeker/{jobSeekerId}")]
        [ProducesResponseType(typeof(IEnumerable<ExperienceReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExperienceReadDto>>> GetByJobSeeker(int jobSeekerId, CancellationToken cancellationToken)
        {
            var result = await _experienceService.GetByJobSeekerAsync(jobSeekerId);
            return Ok(result);
        }

        /// <summary>
        /// Створити новий досвід роботи
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ExperienceReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExperienceReadDto>> Create([FromBody] ExperienceCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _experienceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Оновити досвід роботи
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExperienceReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExperienceReadDto>> Update(int id, [FromBody] ExperienceUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _experienceService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Видалити досвід роботи
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _experienceService.DeleteAsync(id);
            return NoContent();
        }
    }
}

