# Eshopping

   Eshopping is a backend project designed for managing product orders. It’s a system built using .NET 9 and Entity Framework Core, following modern software architecture principles such as DDD (Domain-Driven Design) and CQRS (Command Query Responsibility Segregation)

   The entire application is fully containerized using Docker, which makes it simple to run, test, and deploy across different environments
   
---

## Technologies Used

   - **.NET 9**
   - **Entity Framework Core**
   - **SQL Server**
   - **CQRS** (Command Query Responsibility Segregation)
   - **DDD** (Domain-Driven Design)
   - **MediatR** (for Domain Events and Command Dispatching)
   - **FluentValidation** (Request Validation)
   - **AutoMapper**
   - **Autofac** (Dependency Injection Container)
   - **xUnit** (Unit Testing)
   - **Swagger/OpenAPI**
   - **Docker** & **Docker Compose**

---

## Project Structure

   The solution is organized into the following layers:

   - **Domain**: Entities (AppUser, Product, ProductCategory, Order and OrderItem), Events, Constants, Enums, Exceptions, SeedWork(Entity, IRepository, IUnitOfWork)  
   - **Application**: Commands, Queries, Validators, Behaviors
   - **Infrastructure**: EF Core Repositories, DbContext
   - **API**: REST Controllers, Dependency Injection config, Background Services, Database config
   - **Tests**: xUnit-based test projects for Commands, Queries, Behaviors and others

---

## Running the Project

### Option 1: IIS Express

   Run the project via Visual Studio using IIS Express:

   - Swagger: [https://localhost:44380/swagger/index.html](https://localhost:44380/swagger/index.html)
   - Base API URL: [https://localhost:44380/api](https://localhost:44380/api)

---

### Option 2: Docker 

   Run the project in containers:

#### Steps:

   1. Install Docker Desktop: (https://docs.docker.com/desktop/setup/install/windows-install/)
   2. Open Docker Desktop
   3. Open your terminal **at the root directory** (where `docker-compose.yml` and `.dockerfile` are located)
   4. Run the following commands using (bash, command prompt or similar):
      
      docker-compose build --no-cache
      docker-compose up
      
   5. The application will be accessible at:
      - API: [http://localhost:4500/api](http://localhost:4500/api)
      - Swagger: [http://localhost:4500/swagger/index.html](http://localhost:4500/swagger/index.html)

   6. Add your endpoints to test specific APIs

---

## Key Features

   ### Order Management

   - Create, Cancel Orders (Commands)
   - Orders stored in SQL Server with status tracking and event raised
   - Order status auto-updated to "Shipped" through a background job each 1 min if any 'pending' status
   - Orders **list all orders** (Query)

   ### Product Management

   - Products have stock tracking
   - If product stock reaches 0, a **domain event** is triggered: `ProductStockDepletedDomainEvent` to increase the target product

### Validations

   Using **FluentValidation** with custom rules:

   - Create Order:

      - Each ProductId must refer to an existing product
      - At least one OrderItem must be included in the request
      - Every item must specify a valid quantity greater than zero

   - Cancel Order:

      - Validates that the target order exists and only can be cancel when the status is 'pending'
   
   - Delete Order:
      
      - Validates that the target order exists and only can be deleted when the status in range of [Pending - Canceled]

### Tests

   - Command/Validation/Behavior tests using **xUnit**
   - Located under the `Tests` folder

### Domain Events

   Handled using **MediatR** pipeline:

   - Events like `OrderCreatedDomainEvent` and `ProductStockDepletedDomainEvent`
   - Dispatched via `DispatchDomainEventsAsync()` during save lifecycle

### Background Service

   - A hosted background service runs **every minute**
   - Scans for orders with `Pending` status and updates them to `Shipped`

   ### Seed Data

   On application startup, the following seed logic executes:

   - Checks for existence of **Products**, **Product Categories**, and at least **one User**
   - If not found, it loads them from `appsettings.json` and save them to db
   - Default Admin User:
      - Email: `ayoub.boudaoud@admin.com`

---

## Useful Paths

   - **Swagger UI**: `/swagger/index.html`
   - **API Base URL**: `/api`
   - **Example Order Endpoint**: `/api/orders`
   - **Seed Configuration**: `appsettings.json`

---

## Contact

   Developed by **Ayoub Boudaoud**

---

_last updated: 2025-08-05_