# Project Title

Ballast Lane technical excercise that simulates a blog posting system.

## Introduction

This project is a demonstration of a simple yet robust web application designed to simulate a blog posting system. The application showcases a comprehensive implementation of a **.NET C# web application** adhering to **Clean Architecture principles** and employing **Test-Driven Development (TDD) methodologies**.

### Core Functionality
The core functionality of this application allows users to perform CRUD (Create, Read, Update, Delete) operations on blog posts through a well-defined ASP.NET Web API. In addition to managing blog posts, the application supports user creation and authentication, ensuring that each blog post is associated with a user.

### Project Structure
The application is structured into serveral layers designed to be independent, promoting a clean separation of concerns in line with Clean Architecture principle:

- **Presentation layer**: This layer contains an ASP.NET Core Web API project created for handling the HTTP requests.

- **Application layer**: This layer encapsulate the application's core funtionality and rules.

- **Infaestructure layer**: This layer contains the all the database and external implementations that is not part of the core business rules and logic but it is still needed to performn their tasks. Additionally, this layer contains a database project where all database scripts are defined to create the database schema.

- **Domain layer**: This layer contains all entities definitions needed for the application.

- **Unit tests**: Additionally, unit test project was created to ensure reliability and maintainability, extensive unit tests cover all components of the application, from the data access logic to the API endpoints. This adherence to TDD methodologies not only guarantees the application's robustness but also facilitates future enhancements and bug fixes.

This project is fully runnable locally, with Docker preferred for ease of setup and consistency across development environments. The application's architecture, design decisions, and functionality are detailed further in this document, providing a comprehensive overview for both users and contributors.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- Git
- Docker
- Visual Studio 2022


### Installation

1. Download and run SQL Server docker image: `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong#Password" -p 1433:1433 --name ballastlane-blog -d mcr.microsoft.com/mssql/server:2022-latest`

    Note: To run the application, you can leave the default values but if you want to change:

    - The password, you can do it here: "MSSQL_SA_PASSWORD=Strong#Password". 
    - The ports: -p 1433:1433
    - The docker container name: --name ballastlane-blog

2. Clong the repostiory: `git clone https://github.com/cesarfp/ballastlane-blog.git`

3. Publish the database; Open the Visual Studio solution, go to the Ballastlane.Blog.Database project to create the database schema.

    Note: Select the database instance and choose a database name.

4. Execute the application: Once the Visual Studio solution is open, make make sure that the Ballastlane.Blog.Api project is selected as Startup project, and run the application.

## User Stories

1. **User Registration**

    As a new user, I want to be able to create an account so that I can login and access personalized features of the application.

2. **User Login**
    As a registered user, I want to be able to log in to the application using my credentials so that I can securely access and manage my information.

3. **Create Data Record**
    
    As a logged-in user, I want to be able to create new records in the application so that I can store information relevant to my needs.

4. **Read Data Records**
    
    As a logged-in user, I want to be able to view all my records so that I can browse the information I have previously entered.

5. **Update Data Record**
    
    As a logged-in user, I want to be able to update an existing record so that I can correct or change the information as needed.

6. **Delete Data Record**
    
    As a logged-in user, I want to be able to delete a record so that I can remove information that is no longer needed.
