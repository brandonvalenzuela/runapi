
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Omitidas en la base de datos
    /// </summary>
    [Table("Roles")]  // Nombre de la tabla en la base de datos
    public class Role
    {
        [Key]
        public int RoleId { get; set; }                      // Identificador único del rol

        [Required]
        [MaxLength(100)]
        public string? RoleName { get; set; }                 // Nombre del rol (Ej. Administrador, Usuario)
        public ICollection<UserRole>? UserRoles { get; set; } = [];// Propiedad de navegación para la relación muchos a muchos con UserRole
    }
}
