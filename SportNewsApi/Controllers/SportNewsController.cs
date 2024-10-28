using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Models;
using SportNewsWebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace SportNewsWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportNewsController(ISportNewsService service) : ControllerBase
    {
        [NonAction]
        private NotFoundObjectResult SportNewsNotFound(int id)
        {
            return NotFound(new { Message = $"Новость с id = {id} не найдена" });
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            SportNews? sportNews = await service.GetById(id);

            if (sportNews == null)
                return SportNewsNotFound(id);

            return Ok(sportNews);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new { SportNews = await service.GetAll() });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSportNewsRequest request, CancellationToken stoppingToken)
        {
            return Ok(new 
            { 
                Id = await service.Add(request, stoppingToken)
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SportNews sportNews)
        {
            if (await service.Update(sportNews))
                return Ok(new { Message = "Данные обновлены" });

            return SportNewsNotFound(sportNews.Id);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (await service.Delete(id))
                return Ok(new { Message = $"Новость с id = {id} удалена" });

            return SportNewsNotFound(id);
        }
    }
}
