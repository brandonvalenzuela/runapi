namespace Application.Dto;

public class SurveyResponseDto
{
    public int SurveyId { get; set; }
    public int UserId { get; set; }
    public DateTime ResponseDate { get; set; }
    public List<QuestionResponseDto> Questions { get; set; }
}