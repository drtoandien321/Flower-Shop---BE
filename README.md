# ?? Ann Flower Shop API

API Backend cho h? th?ng qu?n l? c?a hàng hoa s? d?ng ASP.NET Core v?i Clean Architecture.

## ??? Công Ngh? S? D?ng

- **Framework**: .NET 10.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 10.0.1
- **Authentication**: JWT Bearer
- **Documentation**: Swagger/OpenAPI
- **Password Hashing**: BCrypt.Net

## ?? C?u Trúc Project

```
AnnFlowerProject/
??? Controllers/         # HTTP Request Handlers
??? Services/           # Business Logic Layer
??? Repositories/       # Data Access Layer
??? UnitOfWork/        # Transaction Management
??? DTOs/              # Data Transfer Objects
??? Models/            # Domain Entities
??? Data/              # Database Context
```

## ?? Architecture Pattern

- **Repository Pattern**: Tách bi?t data access logic
- **Unit of Work Pattern**: Qu?n l? transactions
- **Dependency Injection**: Loose coupling
- **Clean Architecture**: Separation of Concerns

## ?? Features

### Authentication & Authorization
- ? JWT Token Authentication
- ? User Registration (Sign Up)
- ? User Login
- ? Role-based Authorization (Admin, Customer)
- ? Password Hashing v?i BCrypt

### Product Management
- ? L?y danh sách s?n ph?m
- ? L?y chi ti?t s?n ph?m
- ? L?c s?n ph?m theo danh m?c
- ? T?m ki?m s?n ph?m
- ? Qu?n l? danh m?c

### Database Entities
- Users (USERS)
- Roles (ROLE)
- Products (PRODUCT)
- Categories (CATEGORY)

## ?? Getting Started

### Prerequisites
- .NET 10.0 SDK
- SQL Server
- Visual Studio 2022 ho?c VS Code

### Installation

1. **Clone repository**
```bash
git clone https://github.com/your-username/AnnFlowerProject.git
cd AnnFlowerProject
```

2. **Update Connection String**

S?a `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "FlowerShopDB": "Server=localhost;Database=Flower_Shop;User Id=sa;Password=your_password;TrustServerCertificate=True"
  }
}
```

3. **Run Migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Run Application**
```bash
dotnet run --launch-profile https
```

5. **Access Swagger**
```
https://localhost:7000/swagger
```

## ?? API Documentation

### Authentication Endpoints

#### Sign Up
```http
POST /api/Auth/signup
Content-Type: application/json

{
  "fullname": "Nguyen Van A",
  "email": "test@example.com",
  "password": "123456",
  "phone": "0901234567"
}
```

#### Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "123456"
}
```

### Product Endpoints

#### Get All Products
```http
GET /api/Products
```

#### Get Product By ID
```http
GET /api/Products/{id}
```

#### Get Products By Category
```http
GET /api/Products/category/{categoryId}
```

#### Get All Categories
```http
GET /api/Products/categories
```

## ?? JWT Configuration

JWT settings trong `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "https://localhost:7000",
    "Audience": "https://localhost:7000"
  }
}
```

## ?? NuGet Packages

```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.1" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.1" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.1" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.1" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.2.1" />
```

## ??? Database Schema

### USERS Table
```sql
CREATE TABLE USERS (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(200) NOT NULL,
    RoleID INT NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES ROLE(RoleID)
);
```

### PRODUCT Table
```sql
CREATE TABLE PRODUCT (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(200) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL,
    ImageURL NVARCHAR(500) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    CategoryID INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES CATEGORY(CategoryID)
);
```

## ?? Testing

S? d?ng Swagger UI ð? test các API endpoints:
1. Ch?y application
2. Truy c?p `https://localhost:7000/swagger`
3. Th?c hi?n Sign Up ð? t?o account
4. Login ð? l?y JWT token
5. Click nút "Authorize" và nh?p token
6. Test các protected endpoints

## ?? Documentation Files

- `AUTH_API_GUIDE.md` - Hý?ng d?n Authentication API
- `PRODUCT_API_COMPLETE_GUIDE.md` - Hý?ng d?n Product API
- `REPOSITORY_PATTERN_GUIDE.md` - Gi?i thích Repository Pattern
- `REFACTORED_ARCHITECTURE_SUMMARY.md` - T?ng quan Architecture
- `DATABASE_SETUP.md` - Hý?ng d?n setup database

## ?? Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License.

## ?? Author

**Your Name**
- GitHub: [@your-username](https://github.com/your-username)

## ?? Acknowledgments

- ASP.NET Core Documentation
- Clean Architecture Principles
- Repository & Unit of Work Patterns

---

? N?u project này h?u ích, h?y cho m?t star nhé!
