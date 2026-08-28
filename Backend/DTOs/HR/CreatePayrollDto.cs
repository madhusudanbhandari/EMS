using Backend.Models.Enums;

namespace Backend.Dtos.HR;

public class CreatePayrollDto
{
    public int PayrollPeriod{get;set;}
    public decimal BaseSalary{get;set;}
    public int Overtime{get;set;}
    public decimal Bonus{get;set;}
    public decimal GrossSalary{get;set;}
    public decimal TotalDeductions{get;set;}
    public decimal NetSalary{get;set;}
}