namespace Allog2405.Api.Models;

public class CustomerWithAddressDTO
{
    public int id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string Cpf {get; set;} = string.Empty;
    public ICollection<AddressDTO> Addresses {get; set;} = new List<AddressDTO>();
}