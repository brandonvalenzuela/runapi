using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiRun.Controllers;

public class TrainingPlansControllerDev : Controller
{
    private readonly TrainingService _trainingService;

    public TrainingPlansControllerDev(TrainingService trainingService)
    {
        _trainingService = trainingService;
    }
    
    /// <summary>
    /// Genera contenido a partir de un input JSON
    /// </summary>
    /// <param name="jsonInput">El input en formato JSON</param>
    /// <returns>Contenido generado por Gemini API</returns>
    [HttpPost("generate-plan")]
    public async Task<IActionResult> GenerateTrainingPlanDev([FromBody] string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return BadRequest("El contenido de entrada no puede estar vacío.");
        }

        try
        {
            var result = await _trainingService.GenerateTrainingPlanAsync(input);
            return Ok(new {result});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Ocurrió un error interno.", details = ex.Message });
        }
    }
}