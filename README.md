# Library DDD 
[![Version](https://img.shields.io/nuget/v/josearias210.DDD)](https://www.nuget.org/packages/josearias210.DDD) [![Build Status](https://dev.azure.com/AntonioProductos/Simple%20DDD/_apis/build/status%2Fjosearias210.DDD?branchName=main)](https://dev.azure.com/AntonioProductos/Simple%20DDD/_build/latest?definitionId=16&branchName=main)


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
- DomainEvent

### Extras:
- SuccessResult
- SuccessResult<T>
- Error
- ErrorResult

#### IAggregateRoot
Interface used to mark classes IAggregateRoot.

```csharp
    public class Order : IAggregateRoot
    {
        // ...
    }
```

Almost always a class marked as IAggregateRoot, must extend Entity class. We will see these later.

#### IRepository<T>

Interface that is used to mark a class that is going to be a repository for an IAggregateRoot class.

```csharp
    public class OrderRepository : IRepository<Order>
    {
    }
```
**Why don't you have the basic methods of crud?** this comes from the library documentation, but the short answer would be because we cannot guarantee that the IAggregateRoot allow all the basic operations of the crud, also the generic names in the actions are always avoided

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

#### Domain Event

This class allows us to generate our domain events that are associated with an Entity. We just need to extend this class. We can pass all the additional information that is needed to the class constructor.

```csharp
public class OrderCreatedDomainEvent : DomainEvent
{
    public OrderCreatedDomainEvent() { }
}
```

Now we can add our event to the list of events available to publish to the domain.

```csharp
public class Order : Entity<Guid>
{
    public string Code { get; private set; }
    public string Description { get; private set; }

    private Order(string code, string description)
    {
        this.Code = code;
        this.Description = description;
    }

    public static Order Create(string code, string description)
    {
        Order order = new Order(code, description);
        order.AddDomainEvent(new OrderCreatedDomainEvent());
        return order;
    }
}
```

When we need to access our domain events we can do so through the "DomainEvents" property of our entity.

```csharp
Order order = Order.Create("0001", "Order Test");

foreach (var e in order.DomainEvents)
{
    Console.WriteLine(e.Id);
    Console.WriteLine(e.Type);
    Console.WriteLine(e.OccurredOn);
    Console.WriteLine(e.IsPublished);
}
```

You can see we have some properties available for all events:

- **Id:** It is a Guid that is generated when the event is created
- **Type:** This string that corresponds to the class name of the event
- **OccurredOn:** The creation date of the event, by default it is UTC
- **IsPublished:** Boolean value that indicates whether the event has already been published or not, by default it is false at the time of event creation.

If you want an event to be marked as published we must call the "Published" method of the domain event.

```csharp
Order order = Order.Create("0001", "Order Test");

foreach (var e in order.DomainEvents)
{
    e.Published();
}
```

If we want to publish all the events simultaneously using the "PublishAllEvents" method of the entity.

```csharp
Order order = Order.Create("0001", "Order Test");
order.PublishAllEvents();
```

#### **NOTE:**
The objective of this library in event management is to facilitate the work in the management of domain rules and events. It is the developer's responsibility to find the mechanism to make "real" publication of the event(Example mediator for synchronous process).

For example, if you are using mediatoR and EF to save to the database, you can override SaveChangesAsync in the context to get the domain events and process it. 

```csharp
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);
            var entitiesWithEvents = ChangeTracker.Entries<Entity<Guid>>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                foreach (var @event in entity.DomainEvents)
                {
                    await _mediator.Send(@event); // Is sample
                    @event.Published();
                }
            }
            return result;
        }
    
 ```

We should also add "IRequest" from mediator to our event.

```csharp
public class CreateOrderDomainEvent : DomainEvent, IRequest
{
    public string Code { get; }

    public CreateOrderDomainEvent(string code)
    {
        this.Code = code;
    }
}
  ```
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

- **ErrorResult<T>**: This is a class that represents a failed operation.  We can send only the error message and we want a list of errors with more detail.

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

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## Reference
- [alexdunn.org](https://josef.codes/my-take-on-the-result-class-in-c-sharp)
- [docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [enterprisecraftsmanship.com](https://enterprisecraftsmanship.com/posts)

## Authors

- [@josearias210](https://www.github.com/josearias210)
