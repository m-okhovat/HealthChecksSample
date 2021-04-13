# GloboTicket ASP.NET Core Microservices Sample Application

GloboTicket is a sample ASP.NET Core Microservices application that you can learn about in the Pluralsight .NET Microservices Learning path. This path consists of the following courses:

- Microservices: The Big Picture
- Getting Started with ASP.NET Core Microservices
- Microservices Communication in ASP.NET Core
- Implementing a data management strategy for an ASP.NET Core Microservices Architecture
- Securing Microservices in ASP.NET Core
- Versioning and Evolving Microservices in ASP.NET Core
- Deploying ASP.NET Core microservices using Kubernetes and AKS
- Implementing cross-cutting concerns for ASP.NET Core microservices
- Strategies for Microservice Scalability and Availability in ASP.NET Core

### Prerequisites

In order to build and run the sample GloboTicket application, it is recommended that you have the following installed.

- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download). You can test that you have it installed by entering the command `dotnet --list-sdks`
- [Entity Framework Command Line Tools](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet). You can install these as a global tool with the command `dotnet tool install --global dotnet-ef`
- [SQL Server Express](https://docs.microsoft.com/en-us/sql/sql-server/editions-and-components-of-sql-server-version-15?view=sql-server-ver15).
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (Community Edition or Greater) or [Visual Studio Code](https://code.visualstudio.com/)

### Building the Code

You can either load `GloboTicket\GloboTicket.sln` in Visual Studio 2019 and build from within Visual Studio, or from the command line, in the same folder as `GloboTicket.sln`, enter the `dotnet build` command.

### Running the Migrations
Before you run GloboTicket for the first time, you need to run the database migrations for all microservices that have a SQL database.

The setup.bat file (in the solution root folder) can be run to complete all Entity Framework pre-requisites, or the migration commands may be run individually...

- Navigate into the `GloboTicket\GloboTicket.Services.EventCatalog` folder and run the `dotnet ef database update` command.
- Navigate into the `\GloboTicket\GloboTicket.Services.ShoppingBasket` folder and run the `dotnet ef database update` command.
- Navigate into the `\GloboTicket\GloboTicket.Services.Discount` folder and run the `dotnet ef database update` command.
- Navigate into the `\GloboTicket\GloboTicket.Services.Marketing` folder and run the `dotnet ef database update` command.
- Navigate into the `\GloboTicket\GloboTicket.Services.Order` folder and run the `dotnet ef database update` command.

### Azure Service Bus

To run the complete solution you will require an Azure Service Bus. Some of the microservices will not start without a valid Azure Service Bus connection string. See below for details on how to skip these. Note that to follow along with some of the later demos in module 3 ("Implementing Centralized Logging in Microservices") of this course, you will require the Azure Service bus.

For more complete information on setting up asynchronous communication, I recommend you see Gill Cleeren's course "[Microservices Communication in ASP.NET Core](https://app.pluralsight.com/library/courses/microservices-communication-asp-dot-net-core)". Module 4 "Setting up Asynchronous Communication between ASP.NET Core Microservices" includes demos which cover creating an Azure Service Bus for use with the GloboTicket application.

High-level steps

- Create a Service Bus in the Azure portal
  - Set a unique namespace and choose a suitable location
  - Choose the Standard pricing tier (required to create topics).
- Under the Entities section, click Topics
  - Create a topic named "checkoutmessage" with the remaining options left as the defaults.
  - Create a topic named "orderpaymentrequestmessage" with the remaining options left as the defaults.
  - Create a topic named "orderpaymentupdatedmessage" with the remaining options left as the defaults.
- Create subscribers
  - For checkoutmessage, add a named subscriber "globoticketorder" with a max delivery count of 10.
  - For checkoutmessage, add a named subscriber "globoticketpayment" with a max delivery count of 10.
  - For orderpaymentupdatedmessage, add a named subscriber "globoticketorder" with a max delivery count of 10.
- Replace the placeholder connection strings "Endpoint=sb://<your-namespace>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<your_key>" in the solution with your actual connection string. You can find the connection string under the "Shared access policies" area in settings for the Service Bus Namespace.

### Running the Application from Visual Studio 2019
You can run the GloboTicket application directly from within **Visual Studio**. Right-click on the solution file and select **Set Startup Projects**, and configure all projects (except GlobotTicket.Integration.Messages and GlobotTicket.Integration.MessagingBus) to either **Start** or **Start without Debugging** as desired. Now, when you run the project from within Visual Studio, all three projects will start up.

If you do not have an Azure Service Bus configured the following two services will fail to start at all so can be set to None. These are required for some demos in Module 3 but most of the demos can be followed without these.

- GloboTicket.Services.Ordering
- GloboTicket.Services.Payment

### Running the Application from the Command Line
Alternatively, you can run the GloboTicket application from the command line. You will need to open separate command prompts, one for each `csproj` file. For each project, navigate into the folder containing the `.csproj` file and run the command `dotnet run`.

**Note:** You may be asked to trust the .NET Core developer certificates. Make sure you do so, in order to use HTTPS to access the services.

### Launch in a browser
If you have followed the instructions, the following services will be running...

- The GloboTicket client application (website) will be running on port 5000, which you can access in the browser at [https://localhost:5000](https://localhost:5000).

 - The Event Catalog microservice will be running on port 5001 and you can view the API documentation at [https://localhost:5001/swagger](https://localhost:5001/swagger)

- The Shopping Basket microservice will be running on port 5002 and you can view the API documentation at [https://localhost:5002/swagger](https://localhost:5002/swagger)

- The Ordering microservice will be running on port 5005 and you can view the API documentation at [https://localhost:5005/swagger](https://localhost:5005/swagger)

- The Payment microservice will be running on port 5006 and you can view the API documentation at [https://localhost:5006/swagger](https://localhost:5006/swagger)

- The Discount microservice will be running on port 5007 and you can view the API documentation at [https://localhost:5007/swagger](https://localhost:5007/swagger)

- The Marketing microservice will be running on port 5008.