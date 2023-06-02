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
                cpf = "12345678901",
                Addresses = new List<Address>()
                {
                    new Address
                    {
                        Id = 1,
                        Street = "asfdf",
                        City = "asd"
                    },
                    new Address
                    {
                        Id = 2,
                        Street = "asfdf",
                        City = "asd"
                    }
                }
            },
                new Customer {
                id = 2,
                firstName = "João",
                lastName = "Pedro",
                cpf = "98765432109",
                Addresses = new List<Address>()
                {
                    new Address
                    {
                        Id = 3,
                        Street = "asfdf",
                        City = "asd"
                    },
                    new Address
                    {
                        Id = 4,
                        Street = "asfdf",
                        City = "asd"
                    }
                }
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