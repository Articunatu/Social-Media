# Social-Media

## Technical Overview

### Server
* Framework & language: .NET 9 / C#
* Database: Microsoft SQL Server
* Database driver: EntityFramework
* Structure: Domain-driven design with CQRS using MediatR
* Endpoints: Minimal API
* Authentication: JSON Web Token (No Identity package)
* Object mapping: Custom extension mapping
* Unit tests: FluentAssertions

### Client
* Framework: React through NextJS using TypeScript
* Style utility: Tailwind CSS
* Caching: Tanstack Query
* Api: Axios
* Component Library: DaisyUI

## Technical Details

The backend project is divided into 1 web API (Presentation) and 3 class libaries: Domain, Infrastructure and Application. Also  <br/>
There also a separate test folder which contain both unit and architechture tests.

### Database
This project uses a document-approach for the database - when a user posts something a message object will be added
to both the message container and as a sub model to the user posting the message.

Since almost every document is extremely large in terms of the amount of objects, every get request will
only select the necessary properties from each document, in order to maximize the performance. For the sake of better performance this project
is also using the Cosmos SDK instead of implementing Entity Framework with Cosmos DB.
