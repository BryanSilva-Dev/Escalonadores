using Escalonadores.Model;
using Escalonadores.Request;
using Escalonadores.Response;
using Escalonadores.Struct;
using Microsoft.AspNetCore.Mvc;

namespace Escalonadores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EscalonadorHospitalController : ControllerBaseAuthorized
    {
        public EscalonadorHospitalController(Context context) : base(context)
        {

        }

        [HttpPost]
        public IActionResult Post([FromBody] EscalonadorHospitalRequest request)
        {
            EscalonadorHospitalResponse response = new EscalonadorHospitalResponse();
            try
            {
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }
    }
}