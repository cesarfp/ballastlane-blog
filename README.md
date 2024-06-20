# Project Title

Ballast Lane technical excercise that simulates a blog posting system.

## Introduction

The Ballastlane Blog API is a simple web application developed as part of a technical interview exercise. It showcases a .NET C# application using ASP.NET Web API, focusing on Clean Architecture principles and Test-Driven Development (TDD) methodologies. This application allows users to perform CRUD operations on blog records and manage user authentication without relying on Entity Framework, Dapper, or Mediator.


## Technical Architecture

### Components

- **Ballastlane.Blog.Infraestructure**: Contains database and non-related bussines logic implementations. The database used for the application was SQL Server 2022.
- **Ballastlane.Blog.Api**: Features ASP.NET Web API endpoints for CRUD operations and user management. For this exercise two controllers were created UserController and PostController.
- **Ballastlane.Blog.Database**: Contains all the scripts needed to build the database schema.
- **Ballastlane.Blog.Application**: Contains business rules and validation, independent of the data layer and API.
- **Ballastlane.Blog.UnitTests**: Covers all components, ensuring adherence to TDD methodologies.

### API Endpoints

Below are the available endpoints for the Ballast Lane Blog API, detailing how to interact with the blog records and user management functionalities.

#### User Controller

This controller defines the register and login endpoints
- Register endpoint: Implements the user registration. Currently, an email and a password are needed.
- Login endpoint: Implements the user login. A user needs to be previously registered to be able to login. If the login is succesful the endpoint will generate a JWT token to be able to interact with the Post controller endpoints.

#### Post Controller
To interact with this controller a JWT token is required.

- **Create post endpoint**: This endponts allows a user to create posts.
- **Get post endpoint**: This endpoint get the post especified by the user.
- **Get posts endpoint**: This endpoint get the list of posts created by the user.
- **Delete post endpoint**: This endpoint remove the post especified by the user. 
- **Update post endpoint**: This endpoint update the post especified by the user.


### Development

This application was developed following Clean Architecture principles, ensuring a separation of concerns and independence of components. The development process was driven by the user stories outlined above.

## Running the Application Locally

This project is fully runnable locally, with Docker preferred for ease of setup and consistency across development environments. The application's architecture, design decisions, and functionality are detailed further in this document, providing a comprehensive overview for both users and contributors.


### Prerequisites

- Git
- Docker
- Visual Studio 2022

### Installation
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

1. **Download and run SQL Server docker image**: `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong#Password" -p 1433:1433 --name ballastlane-blog -d mcr.microsoft.com/mssql/server:2022-latest`

    Note: To run the application, you can leave the default values but if you want to change:

    - The password, you can do it here: "MSSQL_SA_PASSWORD=Strong#Password". 
    - The ports: -p 1433:1433
    - The docker container name: --name ballastlane-blog



2. **Clong the repostiory**: `git clone https://github.com/cesarfp/ballastlane-blog.git`

3. **Publish the database**: Open the Visual Studio solution, go to the Ballastlane.Blog.Database project to create the database schema.

    Note: Select the database instance and choose a database name.

4. **Execute the application**: Once the Visual Studio solution is open, make make sure that the Ballastlane.Blog.Api project is selected as Startup project, and run the application.

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








