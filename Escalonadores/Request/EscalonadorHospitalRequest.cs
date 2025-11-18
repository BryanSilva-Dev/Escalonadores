using Escalonadores.Struct;

namespace Escalonadores.Request
{
    public class EscalonadorHospitalRequest
    {
        public EscalonadorHospitalRequest()
        {
            listPacientes = new List<PacienteStruct>();
        }

        public long idAlgoritmo { get; set; }
        public long nMedicos { get; set; }
        public long qPacientes { get; set; }
        public List<PacienteStruct> listPacientes { get; set; }
    }
}
