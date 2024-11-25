using ecobairroServer.Data;
using ecobairroServer.Source.Core.Models.Marcacao;
using ecobairroServer.Source.Core.Services.Abstraction;
using ecobairroServer.Source.Core.Services.Interface;

namespace ecobairroServer.Source.Core.Services
{
    public class ChamadasPinService : CrudService<ChamadasPin>, IChamadasPinService
    {
        public ChamadasPinService(DataContext context) : base(context) { }
    }
}
