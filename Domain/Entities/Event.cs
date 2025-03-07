
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Events")]  // Nombre de la tabla en la base de datos
    public class Event
    {
        [Key]
        public int EventId { get; set; }  // Llave primaria

        public string? Title { get; set; }  // Título o descripción del evento

        public DateTime StartTime { get; set; }  // Hora de inicio del evento

        public DateTime? EndTime { get; set; }  // Hora de fin (puede ser opcional)

        [ForeignKey("Calendar")]
        public int CalendarId { get; set; }  // Relación con el calendario

        public Calendar? Calendar { get; set; }  // Propiedad de navegación a Calendar

        // Puede haber diferentes tipos de eventos relacionados con las encuestas o sesiones de entrenamiento.
        public EventType Type { get; set; }  // Enumeración para diferenciar entre tipos de eventos (ej. Encuesta completada, Entrenamiento programado)

        // Relación con la entidad Survey (en caso de eventos relacionados con encuestas)
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        public Survey? Survey { get; set; }  // Propiedad de navegación para encuestas

        // Relación con TrainingSession (en caso de eventos relacionados con entrenamientos)
        [ForeignKey("TrainingSession")]
        public int? TrainingSessionId { get; set; }

        public TrainingSession? TrainingSession { get; set; }  // Propiedad de navegación para sesiones de entrenamiento
    }

    public enum EventType
    {
        SurveyCompleted,
        TrainingScheduled
    }
}
