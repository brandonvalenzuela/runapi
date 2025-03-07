
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Calendars")]  // Nombre de la tabla en la base de datos
    public class Calendar
    {
        [Key]
        public int CalendarId { get; set; }  // Identificador único del calendario

        [ForeignKey("User")]
        public int UserId { get; set; }  // Llave foránea al usuario

        public int? TrainingId { get; set; }  // Llave foránea opcional al entrenamiento

        public string? Name { get; set; }  // Nombre del calendario, puede ser por usuario o grupo

        public DateTime EventDate { get; set; }  // Fecha del evento

        public string? EventType { get; set; }  // Ej. "Entrenamiento", "Suscripción"

        public string? Description { get; set; }  // Descripción del evento

        // Colección de eventos en el calendario
        public ICollection<Event>? Events { get; set; } = [];  // Inicializa la colección

        // Navegación
        public User? User { get; set; }
        public TrainingPlan? TrainingPlan { get; set; }  // Relación con entrenamientos personalizados
    }
}
