using OperaHouseTheater.Data.Models;

namespace OperaHouseTheater.Services.Comments
{
    public interface ICommentService
    {

        bool Create(int memberId, int performanceId, string content);

        void Delete(int id);

        int CurrentPerformanceExist(int id);

        int GetMemberId(string userId);

        Comment GetCommentById(int id);
    }
}
