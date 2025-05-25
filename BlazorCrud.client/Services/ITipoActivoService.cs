using BlazorCrud.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCrud.Client.Services
{
    public interface ITipoActivoService
    {
        Task<List<TipoActivoDTO>> Lista();
    }
}
