using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Subscription?> Subscriptions { get; set; }
        public DbSet<TrainingPlan?> TrainingPlans { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId); // Llave primaria
                entity.HasIndex(u => u.Email).IsUnique(); // Email único
                entity.HasMany(u => u.UserRoles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId);
            });

            #region Role
            // Configuración para Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId); // Llave primaria
                entity.HasMany(r => r.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId);
            });
            #endregion

            #region UserRole
            // Configurar relación muchos a muchos entre User y Role
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.RoleId, ur.UserId }); // Clave primaria compuesta

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)  // Asumiendo que tienes una colección en User
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)  // Asumiendo que tienes una colección en Role
                .HasForeignKey(ur => ur.RoleId);
            #endregion

            #region Survey
            // Configurar relación entre Survey y Question
            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)  // Relación inversa
                .HasForeignKey(q => q.SurveyId);
            #endregion

            #region Question
            // Configurar relación entre Question y Survey
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)  // Asumiendo que tienes una colección en Survey
                .HasForeignKey(q => q.SurveyId);

            // Configurar relación entre Question y Answer
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)  // Relación inversa
                .HasForeignKey(a => a.QuestionId);
            #endregion

            #region Answer
            // Configurar relación entre Answer y Question
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)  // Asumiendo que tienes una colección en Question
                .HasForeignKey(a => a.QuestionId);

            // Configurar relación entre Answer y User
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)  // Asumiendo que tienes una colección en User
                .HasForeignKey(a => a.UserId);
            #endregion

            #region Subscription
            // Configurar relación entre Subscription y User
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)  // Si necesitas una colección en User, ajusta aquí
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Configura el comportamiento de eliminación

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 2)");
            #endregion

            #region TrainingPlan
            // Configurar relación entre TrainingPlan y User
            modelBuilder.Entity<TrainingPlan>()
                .HasOne(tp => tp.User)
                .WithMany() // Si necesitas una colección en User, ajusta aquí
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento de eliminación

            // Configurar relación entre TrainingPlan y TrainingSessions
            modelBuilder.Entity<TrainingPlan>()
                .HasMany(tp => tp.TrainingSessions)
                .WithOne() // Configura si TrainingSession tiene referencia a TrainingPlan
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento de eliminación
            #endregion

            #region TrainingSession
            // Configurar relación entre TrainingSession y User
            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.User)
                .WithMany() // Si necesitas una colección en User, ajusta aquí
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Configura el comportamiento de eliminación

            // Configurar relación entre TrainingSession y Workout
            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.Workout)
                .WithMany() // Si necesitas una colección en Workout, ajusta aquí
                .HasForeignKey(ts => ts.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction); // Configura el comportamiento de eliminación

            // Configurar relación entre TrainingSession y TrainingPlan
            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.TrainingPlan)
                .WithMany(tp => tp.TrainingSessions) // Asegúrate de que TrainingPlan tenga una colección de TrainingSessions
                .HasForeignKey(ts => ts.TrainingPlanId)
                .OnDelete(DeleteBehavior.NoAction); // Configura el comportamiento de eliminación
            #endregion

            #region Workout
            // Configurar relación entre Workout y TrainingPlan
            modelBuilder.Entity<Workout>()
                .HasOne(w => w.TrainingPlan)
                .WithMany(tp => tp.Workouts)  // Asegúrate de que TrainingPlan tenga una colección de Workouts
                .HasForeignKey(w => w.TrainingPlanId)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento de eliminación

            // Configurar relación entre Workout y TrainingSessions
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.TrainingSessions)
                .WithOne(ts => ts.Workout)  // Asegúrate de que TrainingSession tenga una propiedad de navegación a Workout
                .HasForeignKey(ts => ts.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento de eliminación

            #endregion

            #region Calendar
            // Configurar relación entre Calendar y User
            modelBuilder.Entity<Calendar>()
                .HasOne(c => c.User)
                .WithMany()  // Asumiendo que no necesitas una colección de Calendars en User
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relación entre Calendar y TrainingPlan
            modelBuilder.Entity<Calendar>()
                .HasOne(c => c.TrainingPlan)
                .WithMany()  // Asumiendo que no necesitas una colección de Calendars en TrainingPlan
                .HasForeignKey(c => c.TrainingId)
                .OnDelete(DeleteBehavior.NoAction);  // Opción para manejar la eliminación

            // Configurar relación entre Calendar y Event
            modelBuilder.Entity<Calendar>()
                .HasMany(c => c.Events)
                .WithOne(e => e.Calendar)  // Relación inversa
                .HasForeignKey(e => e.CalendarId);  // Asegúrate de tener esta propiedad en la clase Event
            #endregion

            #region Event
            // Configurar relación entre Event y Calendar
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Calendar)
                .WithMany(c => c.Events)  // Asumiendo que tienes una colección de eventos en Calendar
                .HasForeignKey(e => e.CalendarId)
                .OnDelete(DeleteBehavior.Cascade);  // Configura el comportamiento de eliminación

            // Configurar relación entre Event y Survey
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Survey)
                .WithMany()  // Asumiendo que no necesitas una colección de eventos en Survey
                .HasForeignKey(e => e.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);  // Configura el comportamiento de eliminación

            // Configurar relación entre Event y TrainingSession
            modelBuilder.Entity<Event>()
                .HasOne(e => e.TrainingSession)
                .WithMany()  // Asumiendo que no necesitas una colección de eventos en TrainingSession
                .HasForeignKey(e => e.TrainingSessionId)
                .OnDelete(DeleteBehavior.SetNull);  // Configura el comportamiento de eliminación
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
