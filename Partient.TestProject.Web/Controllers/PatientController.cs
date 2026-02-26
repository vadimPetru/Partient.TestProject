using Microsoft.AspNetCore.Mvc;
using Partient.TestProject.Application.DTO_s;
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

        /// <summary>
        /// Получает список пациентов с пагинацией
        /// </summary>
        /// <param name="pageSize">Количество элементов на странице (по умолчанию 10, максимум 100)</param>
        /// <param name="pageNumber">Номер страницы (начиная с 1)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список пациентов</returns>
        /// <response code="200">Успешно получен список пациентов</response>
        /// <response code="400">Неверные параметры пагинации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        public IActionResult GetPatients(int pageSize , int pageNumber, CancellationToken cancellationToken)
        {
            var partients = _service.GetPatientResponses(pageSize,pageNumber, cancellationToken);
            return Ok(partients);
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
        public IActionResult GetPatient(Guid id, CancellationToken cancellationToken)
        {
            var partients = _service.GetPatientById(id, cancellationToken);
            return Ok();
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

            return CreatedAtAction(nameof(GetPatient), new { id = patientId});
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
            await _service.RemovePatient(request.Id, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Обновляет существующего пациента
        /// </summary>
        /// <param name="id">Идентификатор пациента для обновления</param>
        /// <param name="request">Данные для обновления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленный пациент</returns>
        [HttpPatch]
        public async Task<IActionResult> Update([FromQuery]Guid id,[FromBody] PatientRequest request, CancellationToken cancellationToken)
        {
            await _service.UpdatePatient(id,request, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Умный поиск по полю Birthdate
        /// </summary>
        /// <param name="Birthdate">Массив интервалов Birthdate</param>
        /// <returns>Массив пациентов</returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string[]? Birthdate, CancellationToken cancellationToken)
        {
            var result = await _service.Search(Birthdate, cancellationToken);

           return result is null ? NotFound() : Ok(result);
        }
    }
}
