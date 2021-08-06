namespace OperaHouseTheater.Services.News
{
    using OperaHouseTheater.Data.Models;

    public interface INewsService
    {
        NewsQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int newsPerPage);

        void Add(string title,string content,string imageUrl, string videoUrl);

        News GetNewsById(int id);

        void Delete(int id);
        
    }
}
