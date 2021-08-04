namespace OperaHouseTheater.Services.News
{
    
    public interface INewsService
    {
        NewsQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int newsPerPage);

        
    }
}
