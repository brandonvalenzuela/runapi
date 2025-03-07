using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;


namespace Application.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ITrainingPlanRepository _trainingPlanRepository;

        public TrainingService(HttpClient httpClient, IConfiguration configuration,
            ITrainingPlanRepository trainingPlanRepository)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["GeminiApi:ApiKey"]
                      ?? throw new InvalidOperationException("La clave API no está configurada");
            _trainingPlanRepository = trainingPlanRepository;

        }

        public async Task<TrainingPlan?> GenerateTrainingPlanAsync(string jsonInput)
        {
            if (string.IsNullOrEmpty(jsonInput))
            {
                throw new InvalidOperationException("Input is empty");
            }

            var requestUrl =
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={_apiKey}";

            var requestPayload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = jsonInput ?? null }
                        }
                    }
                }
            };

            try
            {
                if (requestPayload.contents is null || requestPayload.contents.Length == 0)
                {
                    throw new InvalidOperationException("Input is empty");
                }

                using var response =
                    await _httpClient.PostAsJsonAsync(requestUrl, requestPayload).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                // Retornamos el JSON crudo.
                var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                // Serializamos el JSON
                var serializeJson= await CreateTrainingPlanFromGeminiResponse(jsonResponse);
                
                // Persistimos los datos del JSON
                var result = await CreateTrainingPlanFromDtoAsync(1,jsonResponse ,serializeJson);
                
                return result;
            }
            catch (Exception ex)
            {
                // Aquí podrías usar un ILogger en vez de Console
                Console.WriteLine($"Excepción: {ex.Message}");
                throw;
            }
        }
        
        public async Task<TrainingDto> CreateTrainingPlanFromGeminiResponse(string jsonResponse)
        {
            // 1. Deserializar la respuesta “externa” de Gemini
            var outerResponse = JsonSerializer.Deserialize<GeminiOuterResponse>(jsonResponse);
            if (outerResponse?.candidates == null || outerResponse.candidates.Count == 0)
                throw new InvalidOperationException("No se encontraron candidatos en la respuesta externa.");

            // 2. Tomar el valor de 'text'
            var textField = outerResponse
                .candidates[0]
                .content
                .parts[0]
                .text; // Aquí está tu bloque con ```json ... ```

            // 3. Limpiar el contenido: eliminar ```json, ``` y cualquier ruido adicional
            //    Este ejemplo asume que textField podría contener algo como:
            //      "```json\n{ ... }```\n"
            //    Entonces reemplazamos esos backticks o saltos de línea no deseados.
            textField = textField.Replace("```json", "");
            textField = textField.Replace("```", "");
            textField = textField.Trim();

            // 4. Localizar la primera '{' y la última '}' para aislar el JSON
            var startIndex = textField.IndexOf('{');
            var endIndex = textField.LastIndexOf('}');
            if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex)
                throw new InvalidOperationException("No se encontró un bloque JSON válido en el campo 'text'.");

            // Extraer solo el JSON puro
            var jsonInner = textField.Substring(startIndex, endIndex - startIndex + 1);

            // 5. Deserializar ese JSON interno a tu DTO del plan   

            var planDto = DeserializeTrainingPlanJson(jsonInner);
            
            if (planDto == null)
                throw new InvalidOperationException("Error al deserializar el plan interno de Gemini.");
            
            return planDto;
        }
        
        public TrainingDto DeserializeTrainingPlanJson(string jsonResponse)
        {
            // Deserializa con System.Text.Json (o Newtonsoft.Json, si prefieres):
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
    
            var trainingDto = JsonSerializer.Deserialize<TrainingDto>(jsonResponse, options);
            if (trainingDto == null)
                throw new InvalidOperationException("No se pudo deserializar el JSON a TrainingDto.");

            return trainingDto;
        }
        
        public async Task<TrainingPlan?> CreateTrainingPlanFromDtoAsync(int userId, string jsonOriginal, TrainingDto dto)
        {
            // 1) Crear la entidad TrainingPlan
            var trainingPlan = new TrainingPlan
            {
                UserId = userId,
                Description = $"Plan 5K (por ejemplo) generado por IA", 
                GeneratedAt = DateTime.UtcNow,
                IAOutput = jsonOriginal, // Guardas el JSON crudo (opcional)
                // Podrías inicializar otras propiedades si lo deseas
            };
            
            // 2) Agregar el plan al repositorio/DbContext
            await _trainingPlanRepository.AddTrainigPlanAsync(trainingPlan);
            // Esto te da un trainingPlan con un TrainingPlanId asignado.
        
            // 3) Crear y mapear los Workouts y TrainingSessions
            //    para cada semana del dto.plan
        
            // Notarás que en tu dominio, un Workout está ligado a un TrainingPlan
            // y un TrainingSession generalmente se liga a un Workout + un User + una fecha.
        
            // A) Creemos la lista de Workout
            var allWorkouts = new List<Workout>();
        
            // B) Opcional: podrías también generar TrainingSessions dependiendo de la lógica
            var allSessions = new List<TrainingSession>();
        
            foreach (var week in dto.plan)
            {
                // Podrías, si lo deseas, inferir una fecha para la session
                // basándote en weekNumber. (Ver más abajo "Calcular fecha").
                foreach (var w in week.workouts)
                {
                    // 3.1) Crear un Workout en base al WorkoutDTO
                    //     Asignar un "Name" y un "Description" (o solo uno)
                    var workout = new Workout
                    {
                        TrainingPlanId = trainingPlan.TrainingPlanId, // Relación con TrainingPlan
                        Name = w.type, 
                        Description = w.description,
                        DurationMinutes = 0, // si deseas parsear la duración
                        Difficulty = MapDifficultyLevel(w.intensityLevel),
                        EquipmentRequired = null, // si quieres
                        CreatedAt = DateTime.UtcNow
                    };
        
                    // Agregar a la lista de Workouts
                    allWorkouts.Add(workout);
        
                    // 3.2) Opcionalmente, crear una TrainingSession si tu modelo
                    //     asume que cada Workout de la semana es una sesión
                    //     y si quieres asociarla a un día específico con fecha
                    //     calculada.
                    var sessionDate = CalculateSessionDate(week.weekNumber, w.day, dto.meta.raceDate);
                    
                    var trainingSession = new TrainingSession
                    {
                        UserId = userId,
                        Workout = workout,  // Asignamos el workout
                        TrainingPlanId = trainingPlan.TrainingPlanId,
                        SessionDate = sessionDate,
                        Completed = false, // Por defecto
                        Notes = w.additionalNotes
                    };
                    allSessions.Add(trainingSession);
                }
            }
        
            // 4) Persistir los Workouts y Sessions en DB
            //    (puedes hacerlo directo al DbContext, 
            //     o usar repositorios específicos para cada entidad).
            await _trainingPlanRepository.AddWorkoutsPlanAsync(allWorkouts);
        
            await _trainingPlanRepository.AddTrainingSessionAsync(allSessions);
        
            return trainingPlan; // Retorna el plan que se generó
        }
        
        private Workout.DifficultyLevel MapDifficultyLevel(string intensityLevel)
        {
            return intensityLevel?.ToLower() switch
            {
                "n/a" => Workout.DifficultyLevel.Facil,
                "suave" => Workout.DifficultyLevel.Facil,
                "moderado" => Workout.DifficultyLevel.Medio,
                "intenso" => Workout.DifficultyLevel.Dificil,
                "máximo esfuerzo" => Workout.DifficultyLevel.Dificil,
                _ => Workout.DifficultyLevel.Facil,
            };
        }
        
        
        private DateTime CalculateSessionDate(int weekNumber, string dayName, string raceDateString)
        {
            // 1) Parsear la raceDate
            DateTime raceDate = DateTime.Parse(raceDateString); 
            // Ej: "01/09/2024"

            // 2) Podrías restar (weeksToRace - weekNumber) semanas a raceDate
            //    o si asumes que la semana 1 empieza ya "hoy", 
            //    esto es muy variable según tu lógica.
            // Ejemplo: supongamos la semana 1 inicia 8 semanas antes de raceDate
            // y 'Lunes' es offset 0, 'Martes' offset 1, etc.

            // offset base (semana)
            int offsetSemana = (weekNumber - 1) * 7;

            // offset día
            var dayOffset = dayName.ToLower() switch
            {
                "lunes" => 0,
                "martes" => 1,
                "miércoles" => 2,
                "jueves" => 3,
                "viernes" => 4,
                "sábado" => 5,
                "domingo" => 6,
                _ => 0
            };

            // Suponiendo raceDate es la semana final. 
            // Por ejemplo, si weeksToRace = 8, la semana 1 arrancaría 8 semanas antes
            // dateStart = raceDate - 8 semanas
            // (Esto es un ejemplo, ajusta la lógica como desees)

            // A) parse weeksToRace si deseas, o puedes suponer 1er semana = hoy
            //    O un dateStart fijo.

            // Aquí, supongamos que la semana 8 (última) es la del raceDate
            // => la semana 1 arranca raceDate - 8 semanas
            // => la semana actual arranca startDate + offsetSemana + dayOffset

            DateTime startDate = raceDate.AddDays(-7 * 8); 
            // si tu plan dice que hay 8 semanas totales
    
            return startDate.AddDays(offsetSemana + dayOffset);
        }

    }
}
