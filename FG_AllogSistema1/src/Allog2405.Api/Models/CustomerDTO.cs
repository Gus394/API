using Allog2405.Api.Entities;
namespace Allog2405.Api.Models;

public class CustomerDTO
{
    // id, maybe
    public int id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string FullName
    {
        get
        {
            return FirstName + " " + LastName;
        }
    }
    public string CPF {get; set;} = string.Empty;

    public CustomerDTO(Customer customer) {
        id = customer.id;
        FirstName = customer.firstName;
        LastName = customer.lastName;
        CPF = customer.cpf;
    }

    public CustomerDTO() {

    }
}