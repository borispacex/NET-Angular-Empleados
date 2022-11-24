using BackEndApi_minimal.Data.Models;

namespace BackEndApi_minimal.Services.Contrato
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> GetList();
    }
}
