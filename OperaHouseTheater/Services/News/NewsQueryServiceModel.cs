namespace OperaHouseTheater.Services.News
{
    using System.Collections.Generic;

    public class NewsQueryServiceModel
    {
        public IEnumerable<NewsServiceModel> News { get; init; }
    }
}
