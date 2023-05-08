# Web Development Technologies Assignment 1

## Student Details

Name: Jeffrey Zezhong Tan

Student ID: S3851781

Undergrad

Repository: https://github.com/rmit-wdt-fs-2023/s3851781-a1

This assignment was completed <b>individually</b>.

## Question F - Design Patterns
### Dependency Injection
#### Summary
In dependency injection, classes are coded such that they receive the dependencies they rely on to perform its function. In this way the class does not need to know about the specific implementation of the dependencies, allowing for greater reusability of the class.

#### Purpose
To perform the business logic in this code, the client (`CLIClient` & `CLISession`) calls upon many services. These services in turn use DAOs (Data Access Objects) to interact with the database. 

The services `LoginService` and `TransactionService` are implemented such that they recieve these DAOs as initialisation parameters in their constructors. 

As for DAOs, I create interfaces such as `IAccountDAO`, `ITransactionDAO` etc. and then implementaions such as `MCBAAccountDAO` and `MCBATransactionDAO`. The services can then specify the types of their DAOs as these interfaces and thus accept any DAO implementation which implements the interfaces.

In this way, we have allowed greater extensibility for the application to scale to multiple sources/versions of DAOs in the future, completely decoupling the services from the DAOs.

#### Where
- See `CLI/CLIClient` and `CLI/CLISession` for examples of initialising services.
- See `Services/` for service classes
- See `DAOs/Interfaces` for interfaces and `DAOs/` for DAO classes that implement these interfaces

### Facade 
#### Summary
Facades are classes that simplify complex under-the-hood operations by exposing simplified methods which handle the complexity for the caller. 
#### Purpose
To ensure good organisation and seperation of concerns in the code, I have put all database querying code into DAOs. DAOs then expose easy to use methods for CRUD operations. In this way the SQL code as well as the code which is required to transform data into objects is hidden from the caller.

For the services classes, the DAOs are facades to the underlying database.

Services themselves also then orchestrate the calling of DAO methods to perform high level operations. For example, when calling `Deposit()` in the TransactionService, under the hood it does validation, calls the `TransactionDAO` to create a transaction, and calls the AccountDAO to update the account balance.

Since the CLI classes call the services, to them services are facades to the business logic.

#### Where
- See `CLI/CLIClient` for examples of calling service methods
- See `DAOs/` and Services/ to see classes that hide complex logic by exposing methods 

Given more time, I would have also implemented a `MCBAServiceFactory` to simplify the initialisation of a `MCBAService` as the initialising class currently needs to know about the DAOs the Service depends on and makes the code verbose.

## Question G - Class Library
I implement a class library called StringLibrary which is a static class with static methods. Essentially these are utility funcitons. One function `Truncate` is used to truncate a string with ellipsis and another `FormatDateTime` it to get the DD/MM/YYY hour : min AM or PM format required by the business. These are used in the My Statements feature to properly display the tabulated data of transaction history.

Putting these functions inside a class library means that they can be resused again and again by one or more projects.

## Question H - Required Property
Database entity models are stored nside `/Models`. They allow us to strongly type our entities as objects to and from the database as well as using these types for method parameters. For each column marked non null in the database constraints, I have applied the `required` keyword in the model class properties. 

The reason for this is to enforce consistency in the codebase to the contraints of the database. For example, without the `required` keyword, any developer could instantiate the `Account` class without the `AccountNumber` property. When the developer tries to create an account on the database, they will cause an exception. With the `required` keyword, these mistakes could not happen.

## Question I - Asynchronous Implementation
Asynchronous programming is used in my application for the feature of seeding data into the database when there are no customers in the database, as per business requirements.

In the `DatabaseInitializer` class, `SeedDatabase` is used to call the web service and run multiple insert operations on the database for the Customer, Account, Transaction, Login entities. Many of these inserts can happen in parallel. So the DAOs are implemented such that their insert methods are marked async and therefore allowing `SeedDatabase` to call `Task.WhenAll()` on multiple transactions in parallel. This allows for a faster implementation of the code.

The `main` method is also marked async and therefore can call `await` on `DatabaseInitializer.SeedDatabase()`. This allows for a more readable asynchronous code which looks synchronous.