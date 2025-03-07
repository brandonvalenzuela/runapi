using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Omitidas en la base de datos
    /// </summary>
    [Table("UserRoles")]  // Nombre de la tabla en la base de datos
    public class UserRole
    {
        [Key]
        [Column(Order = 0)]
        public int RoleId { get; set; }  // Llave foránea al rol

        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }  // Llave foránea al usuario

        // Navegación
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
