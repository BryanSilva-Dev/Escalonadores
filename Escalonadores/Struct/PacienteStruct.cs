namespace Escalonadores.Struct
{
    public class PacienteStruct
    {
        public PacienteStruct() 
        {

        }
        public long tempoChegada { get; set; }
        public long duracao { get; set; }
        public long idPrioridadeManchester { get; set; }
        public long? quantum { get; set; }
    }
}
