using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Escalonadores.Model;

namespace Escalonadores.Controllers
{
    public class ControllerBaseAuthorized : Controller
    {
        public Context _context;

        public ControllerBaseAuthorized(Context context)
        {
            _context = context;
        }
    }
}