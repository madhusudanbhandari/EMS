
namespace Backend.Dtos.Common;

public class QueryParameters
{
    private const int MaxPageSize=100;
    public int Page{get;set;}=1;
    private int _pageSize=3;

    public string? Search {get;set;}

    public string? SortBy {get;set;}
    public string? SortOrder{get; set;}="asc";

    public int? DepartmentId {get;set;}
    public int PageSize
    {
        get=>_pageSize;

        set
        {
            _pageSize=value>MaxPageSize
            ?MaxPageSize:value;
        }
    }
}
