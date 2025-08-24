using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Services;

public partial class CitasService   // <-- partial aquí
{
    private readonly AppDbContext _db;

    public CitasService(AppDbContext db)
    {
        _db = db;
    }

    // ====== Alta de cita ======
    public async Task<int> RegistrarCita(int idAnimal, int idDoctor, DateTime fechaHora, string? observaciones)
    {
        _ = await _db.Animales.FindAsync(idAnimal)
            ?? throw new Exception("El animal no existe.");
        _ = await _db.Doctores.FindAsync(idDoctor)
            ?? throw new Exception("El doctor no existe.");

        var nextId = (await _db.Citas.MaxAsync(c => (int?)c.IdCita) ?? 0) + 1;

        var cita = new Cita
        {
            IdCita = nextId,
            IdAnimal = idAnimal,
            IdDoctor = idDoctor,
            FechaHora = fechaHora,
            Estado = "Agendada"
        };

        _db.Citas.Add(cita);
        await _db.SaveChangesAsync();

        await AgregarHistorial(nextId, "Agendada", observaciones);
        return nextId;
    }

    // ====== Reagendar ======
    public async Task ReagendarCita(int idCita, DateTime nuevaFechaHora, string? observaciones)
    {
        var cita = await _db.Citas.FindAsync(idCita)
            ?? throw new Exception("La cita no existe.");

        cita.FechaHora = nuevaFechaHora;
        cita.Estado = "Agendada";
        await _db.SaveChangesAsync();

        await AgregarHistorial(idCita, "Reagendada", observaciones);
    }

    // ====== Cancelar ======
    public async Task CancelarCita(int idCita, string? observaciones)
    {
        var cita = await _db.Citas.FindAsync(idCita)
            ?? throw new Exception("La cita no existe.");

        cita.Estado = "Cancelada";
        await _db.SaveChangesAsync();

        await AgregarHistorial(idCita, "Cancelada", observaciones);
    }

    // ====== Historial (vista) ======
    public async Task<List<Vista_HistorialCitasDetalle>> VerHistorialCita(int idCita)
    {
        return await _db.Vista_HistorialCitasDetalles   // usa el DbSet correcto del contexto
            .Where(v => v.IdCita == idCita)
            .OrderByDescending(v => v.FechaAccion)
            .ToListAsync();
    }

    // ====== Helper ======
    private async Task AgregarHistorial(int idCita, string accion, string? obs)
    {
        var nextHist = (await _db.HistorialCitas.MaxAsync(h => (int?)h.IdHistorial) ?? 0) + 1;

        _db.HistorialCitas.Add(new HistorialCita
        {
            IdHistorial = nextHist,
            IdCita = idCita,
            Accion = accion,
            Observaciones = obs,
            FechaAccion = DateTime.Now
        });

        await _db.SaveChangesAsync();
    }
}
