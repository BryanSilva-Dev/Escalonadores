using Escalonadores.Model;
using Escalonadores.Struct;

namespace Escalonadores.Response
{
    public class LookupResponse : ReturnStruct
    {
        public LookupResponse() 
        {
            listEscalonador = new List<Escalonador>();
            listPrioridades = new List<PrioridadeManchester>();
        }

        public List<Escalonador> listEscalonador { get; set; }
        public List<PrioridadeManchester> listPrioridades { get; set; }
    }
}
