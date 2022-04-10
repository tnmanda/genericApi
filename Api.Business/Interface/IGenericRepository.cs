using Api.Data.ServiceModels;

namespace Api.Business.Interface
{
    public interface IGenericRepository
    {
        object GetForex(GetForexRequest request);
    }
}
