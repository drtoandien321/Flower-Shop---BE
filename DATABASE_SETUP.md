# Hý?ng d?n k?t n?i SQL Server Database

## ? Ð? c?u h?nh xong!

### 1. Connection String
**File:** `appsettings.json`
```json
"ConnectionStrings": {
  "FlowerShopDB": "Server=localhost;Database=Flower_Shop;User Id=sa;Password=12345;TrustServerCertificate=True"
}
```

### 2. Packages ð? cài ð?t
- Microsoft.EntityFrameworkCore (10.0.1)
- Microsoft.EntityFrameworkCore.SqlServer (10.0.1)
- Microsoft.EntityFrameworkCore.Tools (10.0.1)

### 3. Các file ð? t?o
- `Data/FlowerShopDbContext.cs` - Database context
- `Models/Product.cs` - Entity model m?u
- `Controllers/ProductsController.cs` - API controller m?u v?i CRUD operations

---

## ?? Cách s? d?ng

### Bý?c 1: T?o Database (n?u chýa có)
```sql
CREATE DATABASE Flower_Shop;
```

### Bý?c 2: T?o Migration
```bash
dotnet ef migrations add InitialCreate
```

### Bý?c 3: Update Database
```bash
dotnet ef database update
```

### Bý?c 4: Ch?y ?ng d?ng
```bash
dotnet run --launch-profile https
```

---

## ?? Test k?t n?i Database

### 1. Test k?t n?i (Không c?n token)
**Endpoint:** `GET https://localhost:7000/api/Products/test-connection`

**Response thành công:**
```json
{
  "message": "K?t n?i database thành công!",
  "database": "Flower_Shop"
}
```

### 2. Test CRUD v?i Products (C?n JWT token)

**Login trý?c:**
```
POST https://localhost:7000/api/Auth/login
Body: { "username": "admin", "password": "password" }
```

**L?y danh sách Products:**
```
GET https://localhost:7000/api/Products
Header: Authorization: Bearer {your_token}
```

**T?o Product m?i:**
```
POST https://localhost:7000/api/Products
Header: Authorization: Bearer {your_token}
Body: {
  "productName": "Hoa H?ng",
  "description": "Hoa h?ng ð? týõi",
  "price": 50000,
  "stock": 100
}
```

---

## ?? Tùy ch?nh

### Thêm Entity m?i

1. **T?o Model** trong folder `Models/`:
```csharp
public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
```

2. **Thêm DbSet** vào `FlowerShopDbContext.cs`:
```csharp
public DbSet<Category> Categories { get; set; }
```

3. **T?o Migration:**
```bash
dotnet ef migrations add AddCategory
dotnet ef database update
```

### Thay ð?i Connection String

S?a trong `appsettings.json`:
- `Server=localhost` - Server name/IP
- `Database=Flower_Shop` - Tên database
- `User Id=sa` - Username
- `Password=12345` - Password

---

## ?? Lýu ? quan tr?ng

1. **SQL Server ph?i ðang ch?y**
2. **Mixed Mode Authentication ph?i ðý?c b?t** ð? dùng SQL Authentication (sa)
3. **Firewall** ph?i cho phép k?t n?i ð?n SQL Server
4. **TrustServerCertificate=True** - B? qua SSL certificate validation (ch? dùng trong môi trý?ng development)

---

## ?? Troubleshooting

### L?i: "Cannot open database"
- Ki?m tra database ð? t?n t?i chýa
- Ch?y: `CREATE DATABASE Flower_Shop;`

### L?i: "Login failed for user 'sa'"
- Ki?m tra password ðúng chýa
- Ki?m tra SQL Server Authentication ð? b?t chýa

### L?i: "A network-related or instance-specific error"
- Ki?m tra SQL Server ðang ch?y chýa
- Ki?m tra TCP/IP protocol ð? enable chýa
- Ki?m tra port 1433 có m? không
