namespace Allog2405.Api.Entities;

public class Customer {
    public int id {get; set;}
    public string firstName {get; set;} = string.Empty;
    public string lastName {get; set;} = string.Empty;
    public string cpf {get; set;} = string.Empty;
    public ICollection<Address> Addresses {get; set;} = new List<Address>(); // singleton address
}