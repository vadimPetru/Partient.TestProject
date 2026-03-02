using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Application.Services.PatientServices;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Models;

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

        /// <summary>
        /// Запрос на получение все пациентов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPatients(
            CancellationToken cancellationToken)
        {
            var patients = await _service.GetPatients(cancellationToken);

            return Ok(patients ?? new List<Patient>());
        }

        /// <summary>
        /// Получает пациента по его идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор пациента (GUID)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Информация о пациенте</returns>
        /// <response code="200">Пациент найден</response>
        /// <response code="404">Пациент не найден</response>
        /// <response code="400">Неверный формат идентификатора</response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatient(Guid id, CancellationToken cancellationToken)
        {
            var partient = await _service.GetPatientById(id, cancellationToken);
            return Ok(partient);
        }

        /// <summary>
        /// Создает нового пациента
        /// </summary>
        /// <param name="request">Данные для создания пациента</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданный пациент</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientRequest request, CancellationToken cancellationToken)
        {
            var patientId = await _service.CreatePatient(request, cancellationToken);

            return Ok(patientId);
        }
        /// <summary>
        /// Удаляет пациента
        /// </summary>
        /// <param name="id">Идентификатор пациента для удаления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Нет содержимого</returns>
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] DeletePatientRequest request, CancellationToken cancellationToken)
        {
            var id = await _service.RemovePatient(request.Id, cancellationToken);

            return Ok($"Пациент был удален: {id}");
        }

        /// <summary>
        /// Обновляет существующего пациента
        /// </summary>
        /// <param name="id">Идентификатор пациента для обновления</param>
        /// <param name="request">Данные для обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленный пациент</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery]Guid id,[FromBody] PatientRequest request, CancellationToken cancellationToken)
        {
            var idnew = await _service.UpdatePatient(id,request, cancellationToken);

            return Ok($"Пациент был обновлен {idnew}");
        }

        /// <summary>
        /// Умный поиск по полю Birthdate
        /// </summary>
        /// <param name="Birthdate">Массив интервалов Birthdate</param>
        /// <returns>Массив пациентов</returns>
        [HttpGet("search")]
        public async Task<IActionResult> Date([FromQuery] ParserType type , [FromQuery] string[] birthDates, CancellationToken cancellationToken)
        {
            if (birthDates is null || !birthDates.Any())
                return BadRequest();

            var collection = _service.Handler(birthDates, type);
            var patients = await collection.ToListAsync(cancellationToken);

            return patients.Any() ? Ok(patients) : Ok(new List<Patient>());
        }

    }
}
