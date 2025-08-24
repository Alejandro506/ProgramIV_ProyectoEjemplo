using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace myFirstProject.Services;

public class CitasService
{
    private readonly string _cs;
    public CitasService(IConfiguration cfg) =>
        _cs = cfg.GetConnectionString("Default")!;

    public async Task<int> RegistrarCita(int idAnimal, int idDoctor, DateTime fechaHora, string observaciones)
    {
        using var conn = new SqlConnection(_cs);
        var p = new DynamicParameters();
        p.Add("@IdAnimal", idAnimal);
        p.Add("@IdDoctor", idDoctor);
        p.Add("@FechaHora", fechaHora);
        p.Add("@Observaciones", observaciones);
        // Tu SP devuelve SELECT @NuevoIdCita AS IdCitaCreada
        return await conn.QuerySingleAsync<int>(
            "PrograAvanzada202502User06.RegistrarCita",
            p, commandType: CommandType.StoredProcedure);
    }

    public async Task ReagendarCita(int idCita, DateTime nuevaFechaHora, string observaciones)
    {
        using var conn = new SqlConnection(_cs);
        var p = new DynamicParameters();
        p.Add("@IdCita", idCita);
        p.Add("@NuevaFechaHora", nuevaFechaHora);
        p.Add("@Observaciones", observaciones);
        await conn.ExecuteAsync(
            "PrograAvanzada202502User06.ReagendarCita",
            p, commandType: CommandType.StoredProcedure);
    }

    public async Task CancelarCita(int idCita, string observaciones)
    {
        using var conn = new SqlConnection(_cs);
        var p = new DynamicParameters();
        p.Add("@IdCita", idCita);
        p.Add("@Observaciones", observaciones);
        await conn.ExecuteAsync(
            "PrograAvanzada202502User06.CancelarCita",
            p, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<dynamic>> VerHistorialCita(int idCita)
    {
        using var conn = new SqlConnection(_cs);
        var p = new DynamicParameters();
        p.Add("@IdCita", idCita);
        return await conn.QueryAsync(
            "PrograAvanzada202502User06.VerHistorialCita",
            p, commandType: CommandType.StoredProcedure);
    }
}
