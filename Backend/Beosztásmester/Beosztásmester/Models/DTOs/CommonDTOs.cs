namespace Beosztasmester.DTOs;

public class PaginationFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class DateRangeDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}