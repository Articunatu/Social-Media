# Social-Media

## Technical Overview

### Server
* Framework & language: .NET 9 / C#
* Database: Microsoft SQL Server
* Database driver: EntityFramework
* Structure: Domain-driven design with CQRS using MediatR
* Endpoints: Minimal API
* Authentication: JSON Web Token (No Identity package)
* Object mapping: Simple custom extensions
* Unit tests: FluentAssertions
* Validation: FluentValidations & Configurations in EFCore

### Client
* Framework: React through NextJS using TypeScript
* Style utility: Tailwind CSS
* Component Library: DaisyUI
* Caching: Tanstack Query
* Api: Axios

## Technical Details

The backend project is divided into 1 web API (Presentation) and 3 class libaries: Domain, Infrastructure and Application. Also  <br/>
There also a separate test folder which contain both unit and architechture tests.
