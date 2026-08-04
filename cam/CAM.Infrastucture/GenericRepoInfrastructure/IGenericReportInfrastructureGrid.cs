namespace CAM.Infrastucture.QueryResult
{
    public interface IGenericReportInfrastructureGrid
    {
        GenericReportInfrastructureGrid<T> GenerateReport<T>(long id);
    }
}
