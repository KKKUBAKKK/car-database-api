# Car Rental API

## Objective
The Car Rental API is designed to manage car rental operations, including user authentication, car management, rental processing, and return handling. It provides endpoints for both customers and employees to interact with the system.

## File Structure
The project is organized as follows:

```
car-database-api/
├── Controllers/
│   ├── AuthController.cs
│   ├── Customer.cs
│   │   ├── RentalsController.cs
│   │   ├── CarsController.cs
│   ├── Employee.cs
│   │   ├── RentalsManagementController.cs
│   │   └── CarsManagementController.cs
├── Data/
│   └── CarRentalDbContext.cs
├── Helpers/
│   ├── CarRentalCalculator.cs
│   ├── Constants.cs
│   └── MappingProfile.cs
├── Auth/
│   └── Roles.cs
├── DTOs/
│   ├── CarDto.cs
│   ├── RentalDto.cs
│   ├── ReturnRecordDto.cs
|   ├── ...
│   └── RentalHistoryDto.cs
├── Migrations/
│   └── [Migration Files]
├── Models/
│   ├── Car.cs
│   ├── Customer.cs
│   ├── Rental.cs
│   ├── ...
│   └── ReturnRecord.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── car-database-api.http
```

### Directories and Files

- **Controllers/**: Contains the API controllers that handle HTTP requests and responses.
- `AuthController.cs`: Handles login and assigning roles
  - ***Customer***: Contains controllers meant to be used by Customer Api
     - `CarsController.cs`: Handles car-related operations.
     - `RentalsController.cs`: Manages rental operations.
  - ***Employee***: Contains controllers meant to be used by Employee App
     - `RentalsManagementController.cs`: Manages rental operations.
     - `CarsManagementController.cs`: Handles car return operations.

- **Data/**: Contains the database context and seed data.
  - `CarRentalDbContext.cs`: Defines the database context for Entity Framework Core.

- **DTOs/**: Contains Data Transfer Objects used for transferring data between the client and server.
  - `CarDto.cs`: Represents car data.
  - `RentalDto.cs`: Represents rental data.
  - `ReturnRecordDto.cs`: Represents return record data.
  - `RentalHistoryDto.cs`: Represents rental history data.
  - ...

- **Migrations/**: Contains Entity Framework Core migration files.

- **Models/**: Contains the entity models representing the database tables and some other classes.
  - `Car.cs`: Represents the car entity.
  - `Customer.cs`: Represents the customer entity.
  - `Rental.cs`: Represents the rental entity.
  - `ReturnRecord.cs`: Represents the return record entity.
  - `CustomerApi.cs`: Represents the customer Api entity.
  - `Employee.cs`: Represents the employee entity.
  - `RentalOffer.cs`: Represents the rental offer entity.
  - ...

- **Properties/**: Contains project properties and settings.
  - `launchSettings.json`: Configuration for launching the application.

- **appsettings.json**: Configuration file for application settings.

- **Program.cs**: Entry point of the application, configuring services and middleware.

- **car-database-api.http**: Contains HTTP request examples for testing the API.

## Getting Started
To get started with the Car Rental API, follow these steps:

1. **Clone the repository**:
   ```sh
   git clone <repository-url>
   cd car-database-api
   ```

2. **Configure the database connection**:
   Update the connection strings in `appsettings.json` and `Program.cs` as needed.

3. **Apply migrations and seed data**:
   ```sh
   dotnet ef database update
   ```

4. **Run the application**:
   ```sh
   dotnet run
   ```

5. **Access the API**:
   The API can be accessed at `https://localhost:5001` (or the configured URL).

## API Endpoints
The API provides various endpoints for managing car rentals. Some of the key endpoints include:

- **Authentication**:
  - `POST /api/auth/login`: Authenticate and get a JWT token.

- **Customer Operations**:
  - `GET /api/customer/cars`: Get available cars.
  - `POST /api/customer/rentals/offers`: Create a new rental offer.
  - `POST /api/customer/rentals`: Create a new rental.
  - `POST /api/customer/rentals/return`: Return a rented car.

- **Employee Operations**:
  - `GET /api/employee/cars`: Get all cars.
  - `GET /api/employee/rentals/pending-returns`: Get pending returns.
  - `POST /api/employee/rentals/complete-return`: Complete a car return.

For detailed API documentation, refer to the Swagger UI available at `https://localhost:5001/swagger` when running the application in development mode.
