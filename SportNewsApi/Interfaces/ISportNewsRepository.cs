using SportNewsWebApi.Models;

namespace SportNewsWebApi.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для взаимодействия с <see cref="SportNews"/>
    /// </summary>
    public interface ISportNewsRepository
    {
        /// <summary>
        /// Возвращает все новости
        /// </summary>
        /// <returns>Список новостей</returns>
        Task<IEnumerable<SportNews>> GetAll();

        /// <summary>
        /// Возвращает новость по id
        /// </summary>
        /// <param name="id">id новости</param>
        /// <returns>Новость с указанным id</returns>
        Task<SportNews?> GetById(int id);

        /// <summary>
        /// Добавляет новость
        /// </summary>
        /// <param name="sportNews">Добавляемая новость</param>
        /// <returns>id добавленной новости</returns>
        Task<int> Add(SportNews sportNews);

        /// <summary>
        /// Обновляет данные новости
        /// </summary>
        /// <param name="sportNews"></param>
        /// <returns>True, если удалось обновить, иначе false</returns>
        Task<bool> Update(SportNews sportNews);

        /// <summary>
        /// Удаляет новость по id
        /// </summary>
        /// <param name="id">id удаляемой новости</param>
        /// <returns>True, если удалось удалить, иначе false</returns>
        Task<bool> Delete(int id);

        Task ConfirmSportNews(int id, int userId, DateTime confirmationTime);
    }
}
