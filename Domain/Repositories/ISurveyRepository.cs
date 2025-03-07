using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISurveyRepository
    {
        Task<Survey?> SurveyQuestionsAsync(int idSurvey);
        
        Task<bool> SurveyExistsAsync(int surveyId);
        Task<bool> QuestionExistsAsync(int questionId);
        Task AddAnswerAsync(Answer answer);
    }
}
