using Escalonadores.Model;

namespace Escalonadores.Repository
{
    public class ExecucaoRepository
    {
        private Context _context;

        public ExecucaoRepository(Context context)
        {
            _context = context;
        }

        public Execucao Create(Execucao execucao)
        {
            _context.execucao.Add(execucao);
            _context.SaveChanges();
            return execucao;
        }

        public Execucao Update(Execucao execucao)
        {
            _context.Update(execucao);
            _context.SaveChanges();
            return execucao;
        }
    }
}
