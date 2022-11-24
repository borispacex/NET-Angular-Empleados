using BackEndApi_minimal.Data;
using BackEndApi_minimal.Data.Models;
using BackEndApi_minimal.Services.Contrato;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi_minimal.Services.Implementacion
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly DBEmpleadoContext _dbContext;

        public DepartamentoService(DBEmpleadoContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Departamento>> GetList()
        {
            try
            {
                List<Departamento> lista = new List<Departamento>();
                lista = await _dbContext.Departamentos.ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
