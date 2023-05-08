# Web Development Technologies Assignment 2

## Student Details

Name: Jeffrey Zezhong Tan

Student ID: S3851781

Type: Undergrad

Repository: https://github.com/rmit-wdt-fs-2023/s3851781-a2

This assignment was completed <b>individually</b>.

## Relevant Information
- The customer-facing website is in `/CustomerWebsite` and runs on `localhost:44335`
- The Web API is in `/WebApi` and runs on `localhost:5000`
- The admin-facing website is in `/AdminWebsite` and runs on `localhost:5200`

Regarding BillPay, you will need to create a Payee first before you can create a BillPay. This is because the BillPay table has a foreign key constraint on the Payee table.
There is a frontend for this linked in the BillPay page :)

## Question i) WebAPI Documentation
This web api follows the repository pattern such that there is one repository class called "managers" for each entity in the database (see `/Models/DataManager`).
These managers implement a common `IDataRepository` interface, with data entity types are in `/Models`.
The controllers in `/Controllers` define the following endpoints of the API:

Customer
- `GET /api/customer` - returns an array of all customer objects
- `GET /api/customer/{id}` - returns the customer object with the given CustomerID
- `POST /api/customer` - creates a customer
- `PUT /api/customer` - updates a customer
- `DELETE /api/customer/{id}` - deletes a customer with the given CustomerID

BillPay
- `GET /api/billPay` - returns an array of all BillPay objects
- `GET /api/billPay/{id}` - returns the BillPay object with the given BillPayID
- `POST /api/billPay` - creates a BillPay
- `PUT /api/billPay` - updates a BillPay
- `DELETE /api/billPay/{id}` - deletes a BillPay with the given BillPayID

Login
- `GET /api/login` - returns an array of all login objects
- `GET /api/login/{id}` - returns the login object with the given LoginID
- `POST /api/login` - creates a login
- `PUT /api/login` - updates a login
- `DELETE /api/login/{id}` - deletes a login with the given LoginID

## Acknowledgements
Many parts of this repository was bootstrapped using 
- [Day7-Lectorial.zip](https://rmit.instructure.com/courses/114530/files/29063883?wrap=1)
- [Day9-Lectorial.zip](https://rmit.instructure.com/courses/114530/files/29139909?wrap=1)


