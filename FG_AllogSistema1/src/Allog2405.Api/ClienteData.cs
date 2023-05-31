using Allog2405.Api.Entities;

namespace Allog2405.Api;

public class CustomerData {
    static private CustomerData _data;
    public List<Customer> listaCustomers {get; set;}

    private CustomerData() {
        this.listaCustomers = new List<Customer>{
            new Customer {
                id = 1,
                firstName = "Pedro",
                lastName = "Coelho",
                cpf = "12345678901"
            },
                new Customer {
                id = 2,
                firstName = "João",
                lastName = "Pedro",
                cpf = "98765432109"
            }
        };
    }

    static public CustomerData Get() {
        if(_data == null) {
            _data = new CustomerData();
        }

        return _data;
    }
}