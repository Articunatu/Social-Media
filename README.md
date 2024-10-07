# Social-Media

## Technical Overview

### Server
* Framework & language: .NET 6 / C#
* Database: Azure Cosmos DB (NoSQL Cloud) + MS SQL
* Database driver: Cosmos SDK + EntityFramework
* Structure: Domain-driven design with CQRS
* Api: Endpoints without controllers
* Authentication: JSON Web Token
* Object mapping: Automapper
* Unit tests: xUnit

### Client
* Framework: React with TypeScript
* Style utility: Tailwind CSS

## Technical Details

The backend project is divided into 1 web API (Presenration) and 3 class libaries: Domain, Infrastructure and Application. Also  <br/>
There also a separate test folder which contain both unit and architechture tests.

### Database
This project uses a document-approach for the database - when a user posts something a message object will be added
to both the message container and as a sub model to the user posting the message.

Since almost every document is extremely large in terms of the amount of objects, every get request will
only select the necessary properties from each document, in order to maximize the performance. For the sake of better performance this project
is also using the Cosmos SDK instead of implementing Entity Framework with Cosmos DB.
