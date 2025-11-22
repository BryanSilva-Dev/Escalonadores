using Escalonadores.Model;

namespace Escalonadores.Repository
{
    public class EscalonadorExecucaoRepository
    {
        private Context _context;

        public EscalonadorExecucaoRepository(Context context)
        {
            _context = context;
        }

        public EscalonadorExecucao Create(EscalonadorExecucao escalonadorExecucao)
        {
            _context.escalonadorExecucao.Add(escalonadorExecucao);
            _context.SaveChanges();
            return escalonadorExecucao;
        }

        public List<EscalonadorExecucao> CreateRange(List<EscalonadorExecucao> escalonadorExecucao)
        {
            _context.AddRange(escalonadorExecucao);
            _context.SaveChanges();
            return escalonadorExecucao;
        }
    }
}
