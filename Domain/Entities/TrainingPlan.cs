
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("TrainingPlans")]  // Nombre de la tabla en la base de datos
    public class TrainingPlan
    {
        [Key]
        public int TrainingPlanId { get; set; }               // Identificador único del entrenamiento

        [ForeignKey("User")]
        public int UserId { get; set; }                   // Llave foránea al usuario

        public string? Description { get; set; }           // Descripción del entrenamiento generado

        public DateTime GeneratedAt { get; set; }         // Fecha de generación del entrenamiento

        public string? IAOutput { get; set; }              // Resultado generado por IA (JSON u otro formato)

        // Relación con el usuario
        public User? User { get; set; }

        // Propiedad de navegación a TrainingSessions
        public ICollection<TrainingSession>? TrainingSessions { get; set; }
        public ICollection<Workout>? Workouts { get; set; }

        // Constructor
        public TrainingPlan()
        {
            // Constructor vacío necesario para Entity Framework
        }

        public TrainingPlan(int userId, string? description, DateTime generatedAt, string? iaOutput)
        {
            UserId = userId;
            Description = description;
            GeneratedAt = generatedAt;
            IAOutput = iaOutput;
            TrainingSessions = new List<TrainingSession>(); // Inicializar la colección
        }
    }
}
