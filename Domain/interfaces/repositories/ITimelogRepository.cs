public interface ITimelogRepository
{
    void DeleteLog(Timelog Log);
    List<Timelog> GetAllLog(int TaskId);
    void CreateLog(Timelog Log);
}