
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Users")]  // Nombre de la tabla en la base de datos
    public class User
    {
        [Key]
        public int UserId { get; set; }                   // Identificador único del usuario

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }             // Nombre del usuario

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }              // Apellido del usuario

        [Required]
        [EmailAddress]  // Validación de correo electrónico
        [MaxLength(450)]  // Limitar el tamaño del correo electrónico
        public string Email { get; set; }                 // Correo electrónico único

        [Required]
        public string PasswordHash { get; set; }          // Hash de la contraseña

        [Required]
        public DateTime CreatedAt { get; set; }           // Fecha de creación del usuario

        public DateTime? LastLogin { get; set; }          // Última fecha de acceso

        [MaxLength(50)]  // Limitar el tamaño del estado
        public string? Status { get; set; }                // Estado (Activo, Inactivo, etc.)

        // Navegación
        public ICollection<Subscription>? Subscriptions { get; set; } = [];
        public ICollection<UserRole>? UserRoles { get; set; } = [];
        public ICollection<Answer> Answers { get; set; } = []; // Colección de respuestas

        // Constructor
        public User(string firstName, string lastName, string email, string passwordHash)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
