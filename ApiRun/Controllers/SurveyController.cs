using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiRun.Controllers
{
    [ApiController]
    [Route("api/survey")]
    public class SurveyController : Controller
    {
        private readonly ISurveyService _surveyService;
        
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions(int surveyId)
        {
            try
            {
                var response = await _surveyService.GetQuestions(surveyId);
                return Ok(new {response});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] SurveyResponseDto response)
        {
            try
            {
                await _surveyService.SubmitAnswers(response);
            
                return Ok(new { Message = "Responses submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
