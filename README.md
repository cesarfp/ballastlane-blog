# Project Title

Ballastlane Technical Exercise: Simulating a Blog Posting System

## Introduction

The Ballastlane Blog API is a straightforward web application developed as part of a technical interview exercise. It demonstrates a .NET C# application using ASP.NET Web API, emphasizing Clean Architecture principles and Test-Driven Development (TDD) methodologies. Users can perform CRUD operations on blog records and manage user authentication without relying on Entity Framework, Dapper, or Mediator.


## Technical Architecture

### Components

- **Presentation layer**
    - **Ballastlane.Blog.Api**: Features ASP.NET Web API endpoints for CRUD operations and user management.

- **Infrastructure layer**
    - **Ballastlane.Blog.Infraestructure**: Contains service implementations that are not part of the business logic but are needed for the application to function.

    - **Ballastlane.Blog.Persistence**: Contains database repository implementations.

    - **Ballastlane.Blog.Database**: Contains all the scripts needed to build the database schema.

- **Core layer**
    - **Ballastlane.Blog.Application**: Contains business rules and validation, independent of the infraestructure layer and API.

    - **Ballaslane.Blog.Domain**: Contains entity definitions relevant to the application domain.

- **Tests**
    - **Ballastlane.Blog.UnitTests**: Covers all components, ensuring adherence to TDD methodologies.

### API Endpoints
Below are the available endpoints for the Ballast Lane Blog API, detailing how to interact with the blog records and user management functionalities.

#### User Controller
This controller defines the register and login endpoints
- **Register endpoint**: Implements the user registration. Currently, an email and a password are needed.
- **Login endpoint**: Implements user login. Users must be previously registered to log in. If the login is successful, the endpoint will generate a JWT token for interacting with the Post controller endpoints.

#### Post Controller
To interact with this controller, a JWT token is required.

- **Create post endpoint**: This endpoint allows a user to create posts.
- **Get post endpoint**: This endpoint retrieves the post specified by the user.
- **Get posts endpoint**: This endpoint retrieves the list of posts created by the user.
- **Delete post endpoint**: This endpoint removes the post specified by the user.
- **Update post endpoint**: This endpoint updates the post specified by the user.


### Development
This application was developed following Clean Architecture principles, ensuring a separation of concerns and independence of components. The development process was driven by the user stories outlined above.

## Running the Application Locally
This project is fully runnable locally, with Docker preferred for ease of setup and consistency across development environments. The application’s architecture, design decisions, and functionality are detailed further in this document, providing a comprehensive overview for both users and contributors.


### Prerequisites
- Git
- Docker
- Visual Studio 2022

### Installation
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

1. **Download and run SQL Server docker image**: `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Strong#Password" -p 1433:1433 --name ballastlane-blog -d mcr.microsoft.com/mssql/server:2022-latest`

    Note: You can leave the default values, but if you want to make changes:

    - To modify the password, adjust the value here: "MSSQL_SA_PASSWORD=Strong#Password". 
    - To change the ports, modify the `-p 1433:1433` section.
    - To set a different container name, adjust: `--name ballastlane-blog`

2. **Clong the repostiory**: `git clone https://github.com/cesarfp/ballastlane-blog.git`

3. **Publish the database**: 
- Open the Visual Studio solution.
- Go to the Ballastlane.Blog.Database and publish it. 
- Select the database instance and use "BallastlaneBlog" as database name.
    - Note: You can use a different database name however, considering update the connection string in the appsetting.json file in the web api project.

4. **Execute the application**: 
- Ensure that the Ballastlane.Blog.Api project is selected as the startup project in Visual Studio.
- Run the application.

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








