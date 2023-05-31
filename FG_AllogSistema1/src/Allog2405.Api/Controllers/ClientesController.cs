using System.Text.RegularExpressions;
using Allog2405.Api.Models;
using Allog2405.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Allog2405.Api.Controllers;

[ApiController]
[Route("api/Customers")]
public class CustomersController : ControllerBase {

    //Retorna o status de validação do cpf.
    //Valores de retorno:
    //0 = Sucesso
    //1 = CPF nulo
    //2 = CPF inválido
    //3 = CPF já existente
    private int ValidarCpf(string cpf)
    {
        string cpfRegexExp = @"^[0-9]{11}$";

        CustomerData _data = CustomerData.Get();

        Regex cpfRegex = new Regex(cpfRegexExp);

        if(cpf == null)
            return 1;
        if(!cpfRegex.Match(cpf).Success)
            return 2;
        foreach(Customer c in _data.listaCustomers)
            if (cpf == c.cpf)
                return 3;
        
        return 0;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDTO>> GetCustomers()
    {
        var customersToReturn = CustomerData.Get().listaCustomers.Select(Customer => new CustomerDTO{
            id = Customer.id,
            FirstName = Customer.firstName,
            LastName = Customer.lastName,
            CPF = Customer.cpf
        });
        return Ok(customersToReturn);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCustomerPorId")]
    public ActionResult<CustomerDTO> GetCustomerPorId(int id) {
        var Customer = CustomerData.Get().listaCustomers.FirstOrDefault(c => c.id == id);
        if (Customer == null)
        {
            return NotFound();
        }

        CustomerDTO CustomerResult = new CustomerDTO(Customer);
        return Ok(CustomerResult);
    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDTO> GetCustomerPorCpf(string cpf) {
        var Customer = CustomerData.Get().listaCustomers.FirstOrDefault(c => c.cpf == cpf);
        if(Customer == null) return NotFound();

        CustomerDTO CustomerResult = new CustomerDTO(Customer);
        return Ok(CustomerResult);
    }

    [HttpPost]
    public ActionResult<CustomerDTO> CreateCustomer(CustomerForCreationDTO CustomerBody)
    {
        var CustomerEntity = new Customer()
        {
            id = CustomerData.Get().listaCustomers.Max(c => c.id) + 1,
            firstName = CustomerBody.FirstName,
            lastName = CustomerBody.LastName,
            cpf = CustomerBody.Cpf
        };

        CustomerData.Get().listaCustomers.Add(CustomerEntity);

        CustomerDTO CustomerToReturn = new CustomerDTO()
        {
            id = CustomerEntity.id,
            FirstName = CustomerEntity.firstName,
            LastName = CustomerEntity.lastName,
            CPF = CustomerEntity.cpf
        };

        return CreatedAtRoute(
            "GetCustomerPorId",
            new {id = CustomerToReturn.id},
            CustomerToReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id, CustomerForUpdateDTO customerForUpdateDTO)
    {
        if (id != customerForUpdateDTO.id) return BadRequest();
        var customerFromDatabase = CustomerData.Get().listaCustomers.FirstOrDefault(customer => customer.id == id);
        if (customerFromDatabase == null) return NotFound();
        
        customerFromDatabase.firstName = customerForUpdateDTO.FirstName;
        customerFromDatabase.lastName = customerForUpdateDTO.LastName;
        customerFromDatabase.cpf = customerForUpdateDTO.Cpf;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(int id)
    {
        var customerFromDatabase = CustomerData.Get().listaCustomers.FirstOrDefault(customer => customer.id == id);
        if (customerFromDatabase == null) return NotFound();
        CustomerData.Get().listaCustomers.Remove(customerFromDatabase);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDTO> patchDocument,
        [FromRoute] int id)
        {
            var customerFromDatabase = CustomerData.Get().listaCustomers.FirstOrDefault(customer => customer.id == id);
            if (customerFromDatabase == null) return NotFound();
            var customerToPatch = new CustomerForPatchDTO
            {
                FirstName = customerFromDatabase.firstName,
                LastName = customerFromDatabase.lastName,
                Cpf = customerFromDatabase.cpf
            };
            patchDocument.ApplyTo(customerToPatch);

            customerFromDatabase.firstName = customerToPatch.FirstName;
            customerFromDatabase.lastName = customerToPatch.LastName;
            customerFromDatabase.cpf = customerToPatch.Cpf;

            return NoContent();
        }
}

// Customer Icollection<address> adresses = new list<adress>();