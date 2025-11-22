using Escalonadores.Model;
using Escalonadores.Struct;

namespace Escalonadores.Response
{
    public class EscalonadorHospitalResponse : ReturnStruct
    {
        public EscalonadorHospitalResponse()
        {
            execucao = new Execucao();
            eventos = new List<EscalonadorExecucao>();
        }

        public Execucao execucao { get; set; }
        public List<EscalonadorExecucao> eventos { get; set; }

    }
}
