# Social-Media

## Technical Overview

### Server
* Framework & language: .NET 9 / C#
* Database: Microsoft SQL Server
* Database driver: EntityFramework
* Structure: Domain-driven design, clean architechture, with CQRS using MediatR
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

The backend project is divided into 1 web API (Presentation) and 3 class libaries: Domain, Infrastructure and Application.  <br/>
There also a separate test folder which contain both unit and architechture tests.
In comparison to how I would work in most modern projects there's no use of repository pattern here.
Instead I'm trying out how far you can go with the argument that "EFCore is already a good enough abstraction on its own", having a contexts directly 
injected into business logic classes. Currently this classes are tested with InMemoryDb instead of mocking, which might not be the optimal solution; you get to 
somewhat test the queries in the function. However, this approach is known to have side-effects where the InMemoryDb won't fully reflect how a real database
would behave, and a successful test can still result in the tested query breaking the application when run.
The plan is too eventually complement it with more architechtural testing.
