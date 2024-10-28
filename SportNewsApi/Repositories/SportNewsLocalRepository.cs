using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Models;

namespace SportNewsWebApi.Repositories
{
    /// <summary>
    /// Класс локального репозитория для <see cref="SportNews"/>
    /// </summary>
    public class SportNewsLocalRepository : ISportNewsRepository
    {
        private static List<SportNews> News;

        static SportNewsLocalRepository()
        {
            News = [
                new SportNews { Id = 0, Title = "«Динамо» объявило о продлении контракта с Фоминым до 2029 года",
                    Content = "Московское «Динамо» продлило контракт с полузащитником Даниилом Фоминым до лета 2029 года, передает корреспондент «Матч ТВ».\r\n\r\nПредыдущие соглашение 27‑летнего футболиста с клубом было рассчитано до 30 июня 2025 года.\r\n\r\nФомин выступает за «Динамо» с августа 2020 года. Всего за «бело‑голубых» хавбек провел 144 матча во всех турнирах, забил 27 мячей и сделал 20 голевых передач. На его счету 15 матчей за сборную России.\r\n\r\nПрямые трансляции матчей МИР РПЛ и FONBET Кубка России смотрите на каналах «Матч ТВ» и МАТЧ ПРЕМЬЕР, а также сайтах sportbox.ru и matchtv.ru."},
                new SportNews { Id = 1, Title = "Антон Миранчук вошел в топ‑5 самых дорогих футболистов чемпионата Швейцарии по версии Transfermarkt",
                    Content = "Российский полузащитник «Сьона» Антон Миранчук вошел в пятерку самых дорогих футболистов чемпионата Швейцарии по версии портала Transfermarkt.\r\n\r\nМиранчук, покинувший летом московский «Локомотив» в статусе свободного агента, стал игроком «Сьона» в понедельник, соглашение рассчитано до 30 июня 2026 года.\r\n\r\nСогласно оценке источника, стоимость Миранчука составляет 6 млн евро. Россиянина опередили итальянец Симоне Пафунди из «Лозанны» (€10 млн) и конголезец Мешак Элиа из «Янг Бойз» (€7,5 млн), а швейцарский игрок «Янг Бойза» Филип Угринич обладает аналогичной трансферной стоимостью (€6 млн).\r\n\r\nКроме того, 28‑летний россиянин является самым возрастным в топ‑10 самых дорогих футболистов чемпионата Швейцарии.\r\n\r\nМиранчук является воспитанником московского «Локомотива», который покинул этим летом. Футболист выиграл Кубок России, чемпионат и Суперкубок страны."}
            ];
        }

        ///<inheritdoc/>
        public async Task<int> Add(SportNews sportNews)
        {
            sportNews.Id = News.Count;
            
            await Task.Run(() => News.Add(sportNews));

            return sportNews.Id;
        }

        ///<inheritdoc/>
        public async Task<SportNews?> GetById(int id)
        {
            return await Task.Run<SportNews?>(() => News.FirstOrDefault(news => news.Id == id));
        }

        ///<inheritdoc/>
        public Task<IEnumerable<SportNews>> GetAll()
        {
            return Task.FromResult<IEnumerable<SportNews>>(News);
        }

        ///<inheritdoc/>
        public async Task<bool> Delete(int id)
        {
            SportNews? sportNews = await GetById(id);

            if (sportNews == null)
                return false;

            await Task.Run(() => News.Remove(sportNews));

            return true;
        }

        ///<inheritdoc/>
        public async Task<bool> Update(SportNews sportNews)
        {
            return await Task.Run<bool>(() =>
            {
                for (int i = 0; i < News.Count; i++)
                {
                    if (News[i].Id == sportNews.Id)
                    {
                        News[i] = sportNews;

                        return true;
                    }
                }

                return false;
            });
        }

        public Task ConfirmSportNews(int id, int userId, DateTime confirmationTime)
        {
            throw new NotImplementedException();
        }
    }
}
