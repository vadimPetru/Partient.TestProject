using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Partient.TestProject.Application.Services;

namespace Partient.TestProject.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetPatients()
        {
            var partients = _service.GetPatientResponses(1,10,CancellationToken.None);
            return Ok(partients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatient(Guid id)
        {
            var partients = _service.GetPatientById(id,CancellationToken.None);
            return Ok();
        }
    }
}
