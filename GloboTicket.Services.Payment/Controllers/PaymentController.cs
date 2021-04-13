using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Services.Payment.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
