using Backend.Models.Enums;

namespace Backend.Dtos.HR;

public class PayrollResponseDto
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public int PayrollPeriod{get;set;}
    public decimal BaseSalary{get;set;}
    public int Overtime{get;set;}
    public decimal Bonus{get;set;}
    public decimal GrossSalary{get;set;}
    public decimal TotalDeductions{get;set;}
    public decimal NetSalary{get;set;}
    public SalaryStatus Status{get;set;}
    public DateTime ProcessedAt{get;set;}
}