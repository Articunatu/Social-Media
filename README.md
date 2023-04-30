# Social-Media

## Intro
Social Media site with an appearance 

## Technical Overview

### Server
* Framework & Language: .NET 6 / C#
* Database: Azure Cosmos DB (NoSQL Cloud)
* Database driver: Cosmos SDK
* Structure: Repository Pattern - Will shift to CQRS when first deployment is done
* Authentication: JSON Web Token
* Object Mapping: Automapper
* Unit Tests: xUnit

### Client
* Framework: ReactJS
* Preprocessor: SCSS
* Additional Style Utility: Tailwind CSS

## Technical Details

The backend project is divided into 1 web API and 3 class libaries: Models, Core and Tests. <br/>
No TypeScript, in order to get more used to not relying on type checkers.
