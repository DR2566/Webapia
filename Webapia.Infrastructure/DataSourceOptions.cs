namespace Webapia.Infrastructure;

public class DataSourceOptions
{
    public const string SectionName = "DataSource";

    public DataProvider Provider { get; set; } = DataProvider.Database;
}

public enum DataProvider
{
    Database,
    Mock
}