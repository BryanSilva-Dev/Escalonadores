using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Escalonadores.Model;
using Escalonadores.Repository;

namespace Escalonadores.Controllers
{
    public class ControllerBaseAuthorized : Controller
    {
        public Context _context;
        public EscalonadorExecucaoRepository _escalonadorExecucaoRepository;
        public ExecucaoRepository _execucaoRepository;
        public PacienteRepository _pacienteRepository;

        public ControllerBaseAuthorized(Context context)
        {
            _context = context;
            _escalonadorExecucaoRepository = new(_context);
            _execucaoRepository = new(_context);
            _pacienteRepository = new(_context);
        }
    }
}