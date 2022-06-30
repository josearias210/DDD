# Library DDD 
[![Version](https://img.shields.io/nuget/v/josearias210.DDD)](https://www.nuget.org/packages/josearias210.DDD)
[![Build Status](https://dev.azure.com/josearias210Prueba/Nugets/_apis/build/status/josearias210.DDD?branchName=main)](https://dev.azure.com/josearias210Prueba/Nugets/_build/latest?definitionId=10&branchName=main)


Library that offers basic utilities to start working with DDD


## Installing via NuGet

```bash
Install-Package josearias210.DDD
```

## Documentation

This package has the following utilities:

### For DDD:
- IAggregateRoot
- IRepository
- IBusinessRule
- Entity<T>
- ValueObject

### Extras:
- SuccessResult
- SuccessResult<T>
- Error
- ErrorResult

### Documentation:

#### IAggregateRoot
Interface used to mark classes as an aggregate IAggregateRoot

```csharp
    public class Order : IAggregateRoot
    {
        // ...
    }
```

Almost always a class marked as IAggregateRoot, must extend Entity class. We will see these later

#### IRepository<T>

Interface that is used to mark a class that is going to be a repository for an IAggregateRoot.

```csharp
    public class OrderRepository : IRepository<Order>
    {
    }
```
*Why don't you have the basic methods of crud? * this comes from the library documentation, but the short answer would be because we cannot guarantee that the IAggregateRoot allow all the basic operations of the crud, also the generic names in the actions are always avoided

#### IBusinessRule

This interface is used to mark the classes that will be rules for our domain.

```csharp
    public class NameIsRequiredRule : IBusinessRule
    {
        public string Message => throw new NotImplementedException();

        public bool IsBroken()
        {
            throw new NotImplementedException();
        }
    }
```
When you implement this class you must implement the following properties and methods:

- **Message**: It is an error message that will be returned in the exeption when the rule is not satisfied.

- **IsBroken**: It is the method where we must define the rule that must be satisfied.

Suppose we want to validate that a person's name is not empty. We can create a rule similar to the following.

```csharp
    public class NameIsRequiredRule : IBusinessRule
    {
        private readonly string value;
        public NameIsRequiredRule(string value) => (this.value) = (value);

        public string Message => $"Name is required.";

        public bool IsBroken()
        {
            return string.IsNullOrWhiteSpace(this.value);
        }
    }
```
The use of this rule will be seen with the entity class.

#### Entity<T>
Abstract class that is used to mark the classes that are entities within our domain. T would be the type of data used to identify (**Id** field data type of the entity base class) the entity.

```csharp
    public class Order : Entity<Guid>, IAggregateRoot
    {
        // ...
    }
```
When inherited from the Entity class, a method is available for business rule validations **CheckRule(IBusinessRule rule).**

```csharp
    public class Person :  Entity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }

        private Person(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static Person Create(Guid id, string name)
        {
            CheckRule(new NameIsRequiredRule(name));
            return new Person(id, name);
        }
    }
```

You can use as many rules as you want, when one fails automatically the exception will be returned.

#### ValueObject

It is a class used to create a ValueObject.

```csharp
    public class PersonName : ValueObject
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
           throw new NotImplementedException();
        }
    }
```

Using this class requires that you implement the **GetEqualityComponents** method which is used to determine the fields to use when comparing 2 ValueObjects of the same type.

```csharp
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        private Address(string street, string city, string state, string country)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
        }

        public static Address Create(string street, string city, string state, string country)
        {
            return new Address(street, city, state, country);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
        }
    }
```
This class as well as the entity has the method **CheckRule**.

```csharp
        public static Address Create(string street, string city, string state, string country)
        {
            CheckRule(new StreetRule(street));
            CheckRule(new CityRule(city));
            CheckRule(new StateRule(state));
            CheckRule(new CountrydRule(country));
			
            return new Address(street, city, state, country);
        }
```

#### Response

Although these following classes do not belong to DDD theory, they are included because of their common usage.

Standardizing responses is very commonly done. For that there are classes for this.

**All response classes inherit from Result**

- **SuccessResult**:  This is a class that represents a successful operation. It does not receive any parameters.

- **ErrorResult**: This is a class that represents a failed operation. We can send only the error message and we want a list of errors with more detail.


```csharp
    class Person {

        public string Name { get; set; }
        public Person()
        {
            var result = this.Validate();
            if (result.IsSuccess)
            {
                // code
            }
            else {
                // code
            }

        }

        public Result Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new ErrorResult("Name is required");
		// or
		/*
		return new ErrorResult(Name is required", new List<Error>
                {
                    new Error("Name null or empty")
                }});
		*/
            }

            return new SuccessResult();
        } 
    }
```

- **SuccessResult<T>**:  This class is similar to SuccessResult, but allows you to return data.

- **ErrorResult<T: This is a class that represents a failed operation.  We can send only the error message and we want a list of errors with more detail.

```csharp
    class Person
    {

        public string Name { get; set; }
        public Person()
        {
            var result = this.Validate();
            if (result.IsSuccess)
            {
                foreach (var order in result.Data)
                {
                    // code
                }
            }
        }

        public Result<List<Order>> Validate()
        {
            var orders = new List<Order>
                {
                    new Order(/*code*/),
                    new Order(/*code*/)
                };

            return new SuccessResult<List<Order>>(orders);
           // or fails
           // return new ErrorResult<List<Order>>("Error the read orders");
        }
    }
```

## Authors

- [@josearias210](https://www.github.com/josearias210)
