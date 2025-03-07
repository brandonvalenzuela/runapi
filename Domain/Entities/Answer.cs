
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Answers")]  // Nombre de la tabla en la base de datos
    public class Answer
    {
        [Key]
        public int AnswerId { get; init; }  // Identificador único de la respuesta

        [ForeignKey("Question")]
        public int QuestionId { get; init; }  // Llave foránea a la pregunta

        [ForeignKey("User")]
        public int UserId { get; init; }  // Llave foránea al usuario que responde
        
        private readonly string? _responseText;
        public string? ResponseText {
            get => _responseText;
            init
            {
                if (value!.Length > 200)
                    throw new ArgumentException("Response text cannot exceed 500 characters.");
                _responseText = value;
            }
        }  // Texto de la respuesta

        public DateTime ResponseDate { get; init; }  // Fecha de la respuesta

        
        // Navegación
        public User? User { get; }
        public Question? Question { get; }
    }
}
