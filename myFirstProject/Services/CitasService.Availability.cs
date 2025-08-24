using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Services
{
    public partial class CitasService
    {
        /// <summary>
        /// Devuelve slots disponibles (cada minutesSlot) para la semana que inicia en weekStart.
        /// Respeta los horarios de HorariosDoctor y excluye citas ya tomadas (no canceladas).
        /// </summary>
        public async Task<List<DateTime>> GetDisponibilidadSemana(int idDoctor, DateOnly weekStart, int minutesSlot = 30)
        {
            var start = weekStart.ToDateTime(TimeOnly.MinValue);
            var end = start.AddDays(7);

            // Horarios configurados del doctor en esa semana
            var horarios = await _db.HorariosDoctors
                .Where(h => h.IdDoctor == idDoctor)
                .ToListAsync();

            // Citas ya tomadas (no canceladas) en esa semana
            var tomadas = await _db.Citas
                .Where(c => c.IdDoctor == idDoctor
                         && c.FechaHora >= start
                         && c.FechaHora < end
                         && c.Estado != "Cancelada")
                .Select(c => c.FechaHora)
                .ToListAsync();

            var ocupadas = new HashSet<DateTime>(tomadas);
            var libres = new List<DateTime>();

            foreach (var h in horarios)
            {
                // Mapea DiaSemana -> DayOfWeek
                var dow = ParseDiaSemana(h.DiaSemana);
                var day = GetDayOfWeekInWeek(start, dow);

                // Ventana de trabajo del día
                var dayStart = day.Add(new TimeSpan(h.HoraInicio.Hour, h.HoraInicio.Minute, h.HoraInicio.Second));
                var dayEnd   = day.Add(new TimeSpan(h.HoraFin.Hour,    h.HoraFin.Minute,    h.HoraFin.Second));




                for (var t = dayStart; t < dayEnd; t = t.AddMinutes(minutesSlot))
                {
                    if (!ocupadas.Contains(t))
                        libres.Add(t);
                }
            }

            return libres.OrderBy(x => x).ToList();
        }

        // ---- Helpers ----

        private static DayOfWeek ParseDiaSemana(string? dia)
        {
            if (string.IsNullOrWhiteSpace(dia)) return DayOfWeek.Monday;

            var s = dia.Trim().ToLowerInvariant();
            return s switch
            {
                "1" or "lunes"                       => DayOfWeek.Monday,
                "2" or "martes"                      => DayOfWeek.Tuesday,
                "3" or "miercoles" or "miércoles"    => DayOfWeek.Wednesday,
                "4" or "jueves"                      => DayOfWeek.Thursday,
                "5" or "viernes"                     => DayOfWeek.Friday,
                "6" or "sabado" or "sábado"          => DayOfWeek.Saturday,
                "7" or "domingo"                     => DayOfWeek.Sunday,
                _                                    => DayOfWeek.Monday
            };
        }

        private static DateTime GetDayOfWeekInWeek(DateTime weekStart, DayOfWeek dow)
        {
            // weekStart = primer día de esa semana (p.ej. lunes)
            int delta = ((int)dow - (int)weekStart.DayOfWeek + 7) % 7;
            return weekStart.Date.AddDays(delta);
        }
    }
}
