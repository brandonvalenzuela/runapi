using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISurveyService
    {
        Task<SurveyDto> GetQuestions(int surveyId);

        Task SubmitAnswers(SurveyResponseDto response);
    }
}
