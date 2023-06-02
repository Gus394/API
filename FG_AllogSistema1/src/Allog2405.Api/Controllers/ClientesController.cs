using System.Text.RegularExpressions;
using Allog2405.Api.Models;
using Allog2405.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        // configurar isso de forma global
        // URM mapeamento entidade bancod dados fsdfsgdfg
        // customer.addresses.add()
        // select many

        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var ValidationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
            ValidationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return UnprocessableEntity(ValidationProblemDetails);
        }
        
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

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressDTO>> GetCustomersWithAddresses()
    // basicamente a gente ja pegou o dado do banco de dados e eu to dando um select pq eu quero transformar o meu customer q e uma entidade em um dto
    // em qual dto: customerwithaddressdto, entao o select vai percorrer essa lista e ele vai me retornar a cada iteracao esse objeto ja mapeado isso se chama mapeamento
    // mesma coisa com a entidade. Serializacao e tal sei la
    {
        var customerFromDatabase = CustomerData.Get().listaCustomers;
        var customersToReturn = customerFromDatabase.Select(customer => new CustomerWithAddressDTO{
            id = customer.id,
            FirstName = customer.firstName,
            LastName = customer.lastName,
            Cpf = customer.cpf,
            Addresses = customer.Addresses.Select(address => new AddressDTO{
                Id = address.Id,
                City = address.City,
                Street = address.Street
            }).ToList() // para resolver o problema de conversao immplicita, pegou o icollection e "forcou" com c cedilha
        });
        return Ok(customersToReturn);
    }

    // posso criar na mesma classe controller esse metodo mas n fica legal pq tem q coloacr um monte de coisa
    
}

// Customer Icollection<address> adresses = new list<adress>();

// existem situacoes em que nao e necessario retornar todos os dados apenas algum por exemplo so os enderecos
// vai mandar a id pesquisar e retornar da api somente os enderecos
// somente retornar os enderecos.