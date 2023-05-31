namespace Allog2405.Api.Models;

public class CustomerForUpdateDTO
{
    public int id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
}