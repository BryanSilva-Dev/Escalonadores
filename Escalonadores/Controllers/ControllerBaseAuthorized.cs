using Microsoft.AspNetCore.Mvc;
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
        public EscalonadorRepository _escalonadorRepository;
        public PrioridadeManchesterRepository _prioridadeManchesterRepository;
        public ControllerBaseAuthorized(Context context)
        {
            _context = context;
            _escalonadorExecucaoRepository = new(_context);
            _execucaoRepository = new(_context);
            _pacienteRepository = new(_context);
            _escalonadorRepository = new(_context);
            _prioridadeManchesterRepository = new(_context);
        }
    }
}