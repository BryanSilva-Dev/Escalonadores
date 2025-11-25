using Escalonadores.Model;

namespace Escalonadores.Repository
{
    public class EscalonadorRepository
    {
        private Context _context;

        public EscalonadorRepository(Context context)
        {
            _context = context;
        }

        public List<Escalonador> GetAll()
        {
            return _context.escalonador.ToList();
        }
    }
}