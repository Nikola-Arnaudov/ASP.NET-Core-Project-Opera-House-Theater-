﻿namespace OperaHouseTheater.Services.Comments
{
    using OperaHouseTheater.Data.Models;

    public interface ICommentService
    {

        bool Create(int memberId, int performanceId, string content);

        void Delete(int id);

        int CurrentPerformanceExist(int id);

        Comment GetCommentById(int id);
    }
}
