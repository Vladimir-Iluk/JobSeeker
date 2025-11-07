using Bll.Dto.Cv;
using Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CvsController : ControllerBase
    {
        private readonly ICvService _cvService;

        public CvsController(ICvService cvService)
        {
            _cvService = cvService;
        }

        /// <summary>
        /// Отримати CV за ID
        /// </summary>
        /// <param name="id">ID резюме</param>
        /// <param name="cancellationToken">Токен скасування</param>
        /// <returns>Резюме з вказаним ID</returns>
        /// <response code="200">Повертає резюме</response>
        /// <response code="404">Резюме не знайдено</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CvReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CvReadDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _cvService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Отримати всі CV шукача роботи
        /// </summary>
        [HttpGet("jobseeker/{jobSeekerId}")]
        [ProducesResponseType(typeof(IEnumerable<CvReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CvReadDto>>> GetByJobSeeker(int jobSeekerId, CancellationToken cancellationToken)
        {
            var result = await _cvService.GetByJobSeekerAsync(jobSeekerId);
            return Ok(result);
        }

        /// <summary>
        /// Створити нове CV
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CvReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CvReadDto>> Create([FromBody] CvCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _cvService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Оновити CV
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CvReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CvReadDto>> Update(int id, [FromBody] CvUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _cvService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Видалити CV
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _cvService.DeleteAsync(id);
            return NoContent();
        }
    }
}

