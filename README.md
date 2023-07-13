# Social-Media

## Technical Overview

### Server
* Framework & language: .NET 6 / C#
* Database: Azure Cosmos DB (NoSQL Cloud)
* Database driver: Cosmos SDK
* Structure: Repository Pattern - Will shift to CQRS when first deployment is done
* Authentication: JSON Web Token
* Object mapping: Automapper
* Unit tests: xUnit

### Client
* Framework: ReactJS
* Preprocessor: SCSS
* Additional style utility: Tailwind CSS, MaterialUI

## Technical Details

The backend project is divided into 1 web API and 3 class libaries: Core, Business Logic and Tests. <br/>
No TypeScript is used in the client project, in order to get more used to not relying on type checkers.

### Database
This project uses a document-approach for the database - when a user posts something a message object will be added
to both the message container and as a sub model to the user posting the message.

Since almost every document is extremely large in terms of the amount of objects, every get request will
only select the necessary properties from each document, in order to maximize the performance. For the sake of better performance this project
is also using the Cosmos SDK instead of implementing Entity Framework with Cosmos DB.
