using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly MyDbContext _dbContext;

        public SurveyRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Survey?> SurveyQuestionsAsync(int idSurvey)
        {
            return await _dbContext.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyId == idSurvey && s.IsActive);
        }
        
        
        public async Task<bool> SurveyExistsAsync(int surveyId)
        {
            return await _dbContext.Surveys.AnyAsync(s => s.SurveyId == surveyId);
        }

        public async Task<bool> QuestionExistsAsync(int questionId)
        {
            return await _dbContext.Questions.AnyAsync(q => q.QuestionId == questionId);
        }

        public async Task AddAnswerAsync(Answer answer)
        {
            _dbContext.Answers.Add(answer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
