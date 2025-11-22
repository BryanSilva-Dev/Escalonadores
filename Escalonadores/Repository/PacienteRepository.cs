using Escalonadores.Model;

namespace Escalonadores.Repository
{
    public class PacienteRepository
    {
        private Context _context;

        public PacienteRepository(Context context)
        {
            _context = context;
        }

        public Paciente Create(Paciente paciente)
        {
            _context.paciente.Add(paciente);
            _context.SaveChanges();
            return paciente; 
        }

        public List<Paciente> CreateRange(List<Paciente>listPacientes)
        {
            _context.AddRange(listPacientes);
            _context.SaveChanges();
            return listPacientes;
        }

        public List<Paciente> UpdateRange(List<Paciente> listPacientes)
        {
            _context.UpdateRange(listPacientes);
            _context.SaveChanges();
            return listPacientes;
        }
    }
}
