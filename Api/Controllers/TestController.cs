using Core.Dto;
using Core.Entity;
using Core.Interfaces.Services;
using Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IBaseService<Test, TestDto> _testService;

        public TestController(ILogger<TestController> logger, IBaseService<Test, TestDto> testService)
        {
            _logger = logger;
            _testService = testService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IList<TestDto>>>> GetAll()
        {
            _logger.LogInformation("log info");
            var test = await _testService.GetAllAsync();
            return Ok(new ApiResponse<IList<TestDto>>(test));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TestDto>>> Get(Guid id)
        {
            var test = await _testService.GetByIdAsync(id);
            return Ok(new ApiResponse<TestDto>(test));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TestDto>>> Create(TestDto testDto)
        {
            var test = await _testService.AddAsync(testDto);
            return Ok(new ApiResponse<TestDto>(test));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TestDto>>> Update(Guid id, TestDto testDto)
        {
            var test = await _testService.UpdateAsync(testDto);
            return Ok(new ApiResponse<TestDto>(test));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var test = await _testService.RemoveAsync(id);
            return Ok(new ApiResponse<bool>(test));
        }
    }
}
