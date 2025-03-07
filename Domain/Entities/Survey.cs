
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Surveys")]  // Nombre de la tabla en la base de datos
    public class Survey
    {
        [Key]
        public int SurveyId { get; init; }  // Identificador único de la encuesta

        public string? Title { get; init; }  // Título de la encuesta

        public string? Description { get; init; }  // Descripción de la encuesta
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; init; }  // Fecha de creación

        // Navegación
        public ICollection<Question>? Questions { get; init; } = new List<Question>();  // Inicializa la colección
    }
}
