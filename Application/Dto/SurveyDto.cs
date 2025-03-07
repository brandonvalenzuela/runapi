using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class SurveyDto
    {
        public string Title { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
