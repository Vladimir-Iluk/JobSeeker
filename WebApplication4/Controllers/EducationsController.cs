using Bll.Dto.Education;
using Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationService _educationService;

        public EducationsController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        /// <summary>
        /// Отримати освіту за ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EducationReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EducationReadDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _educationService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Отримати всю освіту шукача роботи
        /// </summary>
        [HttpGet("jobseeker/{jobSeekerId}")]
        [ProducesResponseType(typeof(IEnumerable<EducationReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EducationReadDto>>> GetByJobSeeker(int jobSeekerId, CancellationToken cancellationToken)
        {
            var result = await _educationService.GetByJobSeekerAsync(jobSeekerId);
            return Ok(result);
        }

        /// <summary>
        /// Створити нову освіту
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(EducationReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EducationReadDto>> Create([FromBody] EducationCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _educationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Оновити освіту
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EducationReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EducationReadDto>> Update(int id, [FromBody] EducationUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _educationService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Видалити освіту
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _educationService.DeleteAsync(id);
            return NoContent();
        }
    }
}

