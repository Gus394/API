using Allog2405.Api.Models;
using Microsoft.AspNetCore.Mvc;
namespace Allog2405.Api.Controllers;

[ApiController]
[Route("api/customers/{customerID}/addresses")]
public class AddressController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AddressDTO>> GetAddresses(int customerID)
        {
            var customerFromDatabase = CustomerData.Get().listaCustomers.FirstOrDefault(customer => customer.id == customerID);
            if (customerFromDatabase == null) return NotFound();
            var addressesToReturn = new List<AddressDTO>();

            foreach (var address in customerFromDatabase.Addresses)
            {
                addressesToReturn.Add(new AddressDTO{
                    Id = address.Id,
                    Street = address.Street,
                    City = address.City
                });
            }
            return Ok(addressesToReturn);
        }

    // n ta transformando em dto
    [HttpGet("{addressID}")]
    public ActionResult<AddressDTO> GetAdress(int customerID, int addressID)
    {
        var addressesToReturn = CustomerData.Get().listaCustomers.FirstOrDefault(customer => customer.id == customerID)?.Addresses.FirstOrDefault(
            address => address.Id == addressID); // "?" para ver se n e null sei la
        return addressesToReturn != null ? Ok(addressesToReturn) : NotFound();
    }
    // o trabalho vai ser continuar isso, olhar material didatico
    // usar um bagulho chamado select many para pegar todos os enderecos e fazer a contagem de ids
}