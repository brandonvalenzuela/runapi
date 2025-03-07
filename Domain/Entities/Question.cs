
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Questions")]  // Nombre de la tabla en la base de datos
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }  // Identificador único de la pregunta

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }  // Llave foránea a la encuesta

        public string? QuestionText { get; set; }  // Texto de la pregunta

        public string? QuestionType { get; set; }  // Tipo de pregunta (Ej. Abierta, Múltiple opción)
        public bool IsRequired { get; set; }
        // Navegación
        public Survey? Survey { get; set; }
        public ICollection<Answer>? Answers { get; set; } = new List<Answer>();  // Inicializa la colección
    }
}
