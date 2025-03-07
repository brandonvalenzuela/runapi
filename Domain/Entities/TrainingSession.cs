
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("TrainingSessions")]  // Nombre de la tabla en la base de datos
    public class TrainingSession
    {
        [Key]
        public int TrainingSessionId { get; set; } // Llave primaria

        public DateTime SessionDate { get; set; } // Fecha de la sesión

        public string? Notes { get; set; } // Notas opcionales para el entrenamiento

        public bool Completed { get; set; } // Indica si la sesión se completó

        // Relación con Workout
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public Workout? Workout { get; set; }

        // Relación con el usuario que realizó la sesión
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        // Llave foránea a TrainingPlan
        [ForeignKey("TrainingPlan")]
        public int TrainingPlanId { get; set; }
        public TrainingPlan? TrainingPlan { get; set; } // Propiedad de navegación

        public DateTime CreatedAt { get; set; } // Fecha de creación
        
        public TrainingSession() 
        {
            // Constructor por defecto (requerido por EF)
            // Opcionalmente, puedes inicializar algún campo
            CreatedAt = DateTime.UtcNow; 
        }
        
        public TrainingSession(int userId, int workoutId, int trainingPlanId, DateTime sessionDate, bool completed, string? notes = null)
        {
            UserId = userId;
            WorkoutId = workoutId;
            TrainingPlanId = trainingPlanId;
            SessionDate = sessionDate;
            Completed = completed;
            Notes = notes;
            CreatedAt = DateTime.UtcNow; // O la lógica que necesites
        }
    }
}
