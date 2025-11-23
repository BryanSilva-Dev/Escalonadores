using Escalonadores.Model;

namespace Escalonadores.Repository
{
    public class PrioridadeManchesterRepository
    {
        private Context _context;

        public PrioridadeManchesterRepository(Context context)
        {
            _context = context;
        }

        public List<PrioridadeManchester> GetAll()
        {
            return _context.prioridadeManchester.ToList();
        }
    }
}