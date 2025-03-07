using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly SurveyDomainService _surveyDomainService;
        private readonly TrainingService _trainingService;

        public SurveyService(ISurveyRepository surveyRepository, SurveyDomainService surveyDomainService)
        {
            _surveyRepository = surveyRepository;
            _surveyDomainService = surveyDomainService;
        }
        public async Task<SurveyDto> GetQuestions(int surveyId)
        {
            
            var survey = await _surveyRepository.SurveyQuestionsAsync(surveyId);
            
            if (survey == null)
            {
                throw new InvalidOperationException("");
            }

            return new SurveyDto
            {
                Title = survey.Title!,
                Questions = survey.Questions!.Select(q => new QuestionDto()
                {
                    id = q.QuestionId,
                    tipo = q.QuestionType!.ToString(),
                    Text = q.QuestionText
                    
                }).ToList()
            };
        }

        // Serializar como json para 
        public async Task SubmitAnswers(SurveyResponseDto responseDto)
        {
            // Validar que la encuesta existe
            if (!await _surveyRepository.SurveyExistsAsync(responseDto.SurveyId))
            {
                throw new Exception("Survey does not exist.");
            }

            // Validar preguntas y guardar respuestas
            foreach (var question in responseDto.Questions)
            {
                var questionExists = await _surveyRepository.QuestionExistsAsync(question.QuestionId);
                
                if (!questionExists)
                {
                    throw new Exception($"Question ID {question.QuestionId} does not exist.");
                }

                var answer = new Answer
                {
                    UserId = responseDto.UserId,
                    QuestionId = question.QuestionId,
                    ResponseText = question.Answer,
                    ResponseDate = responseDto.ResponseDate
                };
                
                await _surveyRepository.AddAnswerAsync(answer);
            }
            
            #region Llamar al Servicio de IA
            
            //var result = await _trainingService.GenerateTrainingPlanAsync(answer);
                

            #endregion
        }
    }
}
