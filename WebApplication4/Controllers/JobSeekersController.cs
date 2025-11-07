using Bll.Dto.JobSeeker;
using Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;

        public JobSeekersController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        /// <summary>
        /// Отримати всіх шукачів роботи
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobSeekerReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobSeekerReadDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _jobSeekerService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Отримати шукача роботи за ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobSeekerReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobSeekerReadDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _jobSeekerService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Пошук шукачів роботи за ім'ям або навичками
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<JobSeekerReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobSeekerReadDto>>> Search([FromQuery] string query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid query",
                    Detail = "Query parameter cannot be empty",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var result = await _jobSeekerService.SearchAsync(query);
            return Ok(result);
        }

        /// <summary>
        /// Створити нового шукача роботи
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(JobSeekerReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobSeekerReadDto>> Create([FromBody] JobSeekerCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _jobSeekerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Оновити шукача роботи
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(JobSeekerReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobSeekerReadDto>> Update(int id, [FromBody] JobSeekerUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _jobSeekerService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Видалити шукача роботи
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _jobSeekerService.DeleteAsync(id);
            return NoContent();
        }
    }
}

