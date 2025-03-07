
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Workouts")]  // Nombre de la tabla en la base de datos
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }  // Llave primaria

        public string? Name { get; set; }  // Nombre del entrenamiento
        public string? Description { get; set; }  // Descripción del entrenamiento
        public int DurationMinutes { get; set; }  // Duración en minutos

        public DifficultyLevel Difficulty { get; set; }  // Enum: Fácil, Medio, Difícil
        public string? EquipmentRequired { get; set; }  // Equipamiento necesario (opcional)

        public DateTime CreatedAt { get; set; }  // Fecha de creación

        // Relación con TrainingPlan
        [ForeignKey("TrainingPlan")]
        public int TrainingPlanId { get; set; }
        public TrainingPlan? TrainingPlan { get; set; }  // Propiedad de navegación

        // Relación con TrainingSessions
        public ICollection<TrainingSession>? TrainingSessions { get; set; }  // Colección de sesiones de entrenamiento

        // Constructor para inicializar CreatedAt
        public Workout()
        {
            CreatedAt = DateTime.UtcNow;  // O la lógica que necesites
        }

        public enum DifficultyLevel
        {
            Facil, // Fácil
            Medio, // Medio
            Dificil // Difícil
        }
    }

}
