# ??? Refactored Architecture - Repository Pattern

## ? HOÀN THÀNH S?P X?P L?I C?U TRÚC!

### ?? C?u trúc Project m?i:

```
AnnFlowerProject/
?
??? ?? Controllers/              # HTTP Request Handlers
?   ??? AuthController.cs
?   ??? ProductsController.cs
?   ??? WeatherForecastController.cs
?
??? ?? Services/                 # Business Logic Layer
?   ??? IAuthService.cs
?   ??? AuthService.cs          ? Refactored (dùng UnitOfWork)
?   ??? IProductService.cs
?   ??? ProductService.cs       ? Refactored (dùng UnitOfWork)
?
??? ?? Repositories/             # Data Access Layer ? M?I
?   ??? IRepository.cs          ? Generic Repository Interface
?   ??? Repository.cs           ? Generic Repository Implementation
?   ??? IProductRepository.cs   ? Product-specific Interface
?   ??? ProductRepository.cs    ? Product-specific Implementation
?   ??? ICategoryRepository.cs  ? Category-specific Interface
?   ??? CategoryRepository.cs   ? Category-specific Implementation
?   ??? IUserRepository.cs      ? User-specific Interface
?   ??? UserRepository.cs       ? User-specific Implementation
?
??? ?? UnitOfWork/              # Transaction Management ? M?I
?   ??? IUnitOfWork.cs         ? Unit of Work Interface
?   ??? UnitOfWork.cs          ? Unit of Work Implementation
?
??? ?? DTOs/                    # Data Transfer Objects
?   ??? AuthDTOs.cs
?   ??? ProductDTOs.cs
?
??? ?? Models/                  # Domain Entities
?   ??? User.cs
?   ??? Role.cs
?   ??? Product.cs
?   ??? Category.cs
?
??? ?? Data/                    # Database Context
?   ??? FlowerShopDbContext.cs
?
??? Program.cs                  ? Updated (DI Registration)
```

---

## ?? Architecture Flow

### Clean Architecture Layers:

```
???????????????????????????????????????????????????
?          Presentation Layer                     ?
?  ??????????????????????????????????????????    ?
?  ?         Controllers                     ?    ?
?  ?  - Handle HTTP Requests/Responses       ?    ?
?  ???????????????????????????????????????????    ?
??????????????????????????????????????????????????
                  ?
                  ?
???????????????????????????????????????????????????
?          Business Logic Layer                   ?
?  ??????????????????????????????????????????    ?
?  ?         Services                        ?    ?
?  ?  - Business Rules & Logic               ?    ?
?  ?  - Orchestration                        ?    ?
?  ???????????????????????????????????????????    ?
??????????????????????????????????????????????????
                  ?
                  ?
???????????????????????????????????????????????????
?     Transaction Management Layer                ?
?  ??????????????????????????????????????????    ?
?  ?         Unit of Work                    ?    ?
?  ?  - Coordinate Repositories              ?    ?
?  ?  - Manage Transactions                  ?    ?
?  ?  - Single SaveChanges                   ?    ?
?  ???????????????????????????????????????????    ?
??????????????????????????????????????????????????
                  ?
                  ?
???????????????????????????????????????????????????
?          Data Access Layer                      ?
?  ??????????????????????????????????????????    ?
?  ?         Repositories                    ?    ?
?  ?  - CRUD Operations                      ?    ?
?  ?  - Query Logic                          ?    ?
?  ?  - Include/Join Strategies              ?    ?
?  ???????????????????????????????????????????    ?
??????????????????????????????????????????????????
                  ?
                  ?
???????????????????????????????????????????????????
?          Database Layer                         ?
?  ??????????????????????????????????????????    ?
?  ?    DbContext (Entity Framework)         ?    ?
?  ???????????????????????????????????????????    ?
??????????????????????????????????????????????????
                  ?
                  ?
           ????????????????
           ?  SQL Server  ?
           ????????????????
```

---

## ?? So Sánh: Trý?c vs Sau

### ? TRÝ?C (Không có Repository)

```csharp
// Service tr?c ti?p dùng DbContext
public class ProductService
{
    private readonly FlowerShopDbContext _context;
    
    public ProductService(FlowerShopDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProductDto>> GetProducts()
    {
        // Service ph?i bi?t v? EF Core, Include, ToListAsync...
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
            
        return MapToDto(products);
    }
}
```

**V?n ð?:**
- ? Service l?n l?n business logic và data access
- ? Tight coupling v?i EF Core
- ? Khó test (ph?i mock DbContext)
- ? Code l?p l?i ? nhi?u service
- ? Không có transaction management t?p trung

---

### ? SAU (Có Repository & UnitOfWork)

```csharp
// Service ch? quan tâm business logic
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<ProductDto>> GetProducts()
    {
        // Service ch? g?i method t? Repository
        var products = await _unitOfWork.Products
            .GetAllWithCategoryAsync();
            
        return MapToDto(products);
    }
}
```

**L?i ích:**
- ? Separation of Concerns
- ? Loose coupling
- ? D? test (mock IUnitOfWork)
- ? Code reusable
- ? Transaction management t?p trung

---

## ?? Chi Ti?t Các Thành Ph?n

### 1?? Generic Repository (`IRepository<T>`)

**Các method cõ b?n cho m?i entity:**

```csharp
// Query
Task<T?> GetByIdAsync(int id);
Task<IEnumerable<T>> GetAllAsync();
Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

// Command
Task AddAsync(T entity);
Task AddRangeAsync(IEnumerable<T> entities);
void Update(T entity);
void Remove(T entity);
void RemoveRange(IEnumerable<T> entities);

// Utility
Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
```

---

### 2?? Specific Repositories

#### ProductRepository
```csharp
Task<IEnumerable<Product>> GetAllWithCategoryAsync();
Task<Product?> GetByIdWithCategoryAsync(int id);
Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
Task<IEnumerable<Product>> SearchByNameAsync(string keyword);
Task<IEnumerable<Product>> GetProductsInStockAsync();
```

#### CategoryRepository
```csharp
Task<IEnumerable<Category>> GetAllWithProductsAsync();
Task<Category?> GetByIdWithProductsAsync(int id);
Task<Category?> GetByNameAsync(string name);
```

#### UserRepository
```csharp
Task<User?> GetByEmailAsync(string email);
Task<User?> GetByIdWithRoleAsync(int id);
Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
Task<bool> IsEmailExistsAsync(string email);
```

---

### 3?? Unit of Work

**Qu?n l? t?t c? repositories và transactions:**

```csharp
public interface IUnitOfWork : IDisposable
{
    // Specific Repositories
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IUserRepository Users { get; }
    
    // Generic Repository
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    
    // Transaction Management
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

---

## ?? Ví D? S? D?ng

### 1. Simple Query

```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<ProductDto> GetProduct(int id)
    {
        // S? d?ng repository method
        var product = await _unitOfWork.Products
            .GetByIdWithCategoryAsync(id);
            
        return MapToDto(product);
    }
}
```

---

### 2. Create Entity

```csharp
public async Task<bool> CreateProduct(CreateProductDto dto)
{
    var product = new Product
    {
        ProductName = dto.Name,
        UnitPrice = dto.Price,
        CategoryId = dto.CategoryId
    };
    
    // Add vào repository
    await _unitOfWork.Products.AddAsync(product);
    
    // Save changes thông qua UnitOfWork
    await _unitOfWork.SaveChangesAsync();
    
    return true;
}
```

---

### 3. Update Entity

```csharp
public async Task<bool> UpdateProduct(int id, UpdateProductDto dto)
{
    var product = await _unitOfWork.Products.GetByIdAsync(id);
    
    if (product == null)
        return false;
    
    product.ProductName = dto.Name;
    product.UnitPrice = dto.Price;
    
    _unitOfWork.Products.Update(product);
    await _unitOfWork.SaveChangesAsync();
    
    return true;
}
```

---

### 4. Delete Entity

```csharp
public async Task<bool> DeleteProduct(int id)
{
    var product = await _unitOfWork.Products.GetByIdAsync(id);
    
    if (product == null)
        return false;
    
    _unitOfWork.Products.Remove(product);
    await _unitOfWork.SaveChangesAsync();
    
    return true;
}
```

---

### 5. Complex Transaction

```csharp
public async Task<bool> CreateOrderWithProducts(OrderDto dto)
{
    try
    {
        // B?t ð?u transaction
        await _unitOfWork.BeginTransactionAsync();
        
        // 1. Create Order
        var order = new Order { /* ... */ };
        await _unitOfWork.Repository<Order>().AddAsync(order);
        
        // 2. Update Product Stock
        foreach (var item in dto.Items)
        {
            var product = await _unitOfWork.Products
                .GetByIdAsync(item.ProductId);
                
            if (product == null || product.StockQuantity < item.Quantity)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            
            product.StockQuantity -= item.Quantity;
            _unitOfWork.Products.Update(product);
        }
        
        // 3. Commit t?t c? changes
        await _unitOfWork.CommitTransactionAsync();
        return true;
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
```

---

## ?? Dependency Injection

### Ðãng k? trong Program.cs:

```csharp
// Generic Repository
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Specific Repositories
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IUserRepository, UserRepository>();

// Unit of Work
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IProductService, ProductService>();
```

---

## ? Nh?ng G? Ð? Thay Ð?i

### 1. **ProductService.cs** ? Refactored
```diff
- private readonly FlowerShopDbContext _context;
+ private readonly IUnitOfWork _unitOfWork;

- var products = await _context.Products
-     .Include(p => p.Category)
-     .ToListAsync();
+ var products = await _unitOfWork.Products
+     .GetAllWithCategoryAsync();
```

### 2. **AuthService.cs** ? Refactored
```diff
- private readonly FlowerShopDbContext _context;
+ private readonly IUnitOfWork _unitOfWork;

- var user = await _context.Users
-     .Include(u => u.Role)
-     .FirstOrDefaultAsync(u => u.Email == email);
+ var user = await _unitOfWork.Users
+     .GetByEmailAsync(email);
```

### 3. **Program.cs** ? Updated
- Ðãng k? t?t c? Repositories
- Ðãng k? UnitOfWork
- Services gi? inject IUnitOfWork thay v? DbContext

---

## ?? Testing Benefits

### D? dàng mock v?i Repository Pattern:

```csharp
[Fact]
public async Task GetProduct_ShouldReturnProduct_WhenExists()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var mockProductRepo = new Mock<IProductRepository>();
    
    var testProduct = new Product 
    { 
        ProductId = 1, 
        ProductName = "Test" 
    };
    
    mockProductRepo
        .Setup(r => r.GetByIdWithCategoryAsync(1))
        .ReturnsAsync(testProduct);
        
    mockUnitOfWork
        .Setup(u => u.Products)
        .Returns(mockProductRepo.Object);
    
    var service = new ProductService(mockUnitOfWork.Object);
    
    // Act
    var result = await service.GetProductByIdAsync(1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test", result.ProductName);
}
```

---

## ?? Performance & Best Practices

### ? ÐANG ÁP D?NG:

1. **Eager Loading trong Repository**
   ```csharp
   .Include(p => p.Category)  // Tránh N+1 query
   ```

2. **Async/Await**
   ```csharp
   async Task<IEnumerable<Product>>  // Non-blocking I/O
   ```

3. **Single SaveChanges**
   ```csharp
   await _unitOfWork.SaveChangesAsync();  // Atomic operation
   ```

4. **Separation of Concerns**
   - Controller: HTTP handling
   - Service: Business logic
   - Repository: Data access
   - UnitOfWork: Transaction management

---

## ?? T?ng K?t

### ? Ð? Hoàn Thành:

| Thành ph?n | Tr?ng thái |
|------------|-----------|
| Generic Repository | ? Hoàn thành |
| Specific Repositories | ? 3 repositories (Product, Category, User) |
| Unit of Work | ? Hoàn thành |
| Refactor Services | ? 2 services (Product, Auth) |
| DI Registration | ? Program.cs updated |
| Build Success | ? No errors |

### ?? L?i Ích Ð?t Ðý?c:

- ? **Clean Architecture** - Layers r? ràng
- ? **Testability** - D? mock, d? test
- ? **Maintainability** - Code s?ch, d? b?o tr?
- ? **Reusability** - Repository dùng l?i ðý?c
- ? **Flexibility** - D? thay ð?i implementation
- ? **Transaction Support** - Qu?n l? transaction t?p trung

### ?? S?n Sàng S? D?ng!

Project gi? ð? có c?u trúc chu?n Clean Architecture v?i Repository Pattern và Unit of Work!

**Ch?y project:**
```sh
dotnet run --launch-profile https
```

**Test Swagger:**
```
https://localhost:7000/swagger
```

---

## ?? Tài Li?u Tham Kh?o

- `REPOSITORY_PATTERN_GUIDE.md` - Chi ti?t v? pattern
- `AUTH_API_GUIDE.md` - Hý?ng d?n Auth API
- `PRODUCT_API_COMPLETE_GUIDE.md` - Hý?ng d?n Product API
- `DATABASE_SETUP.md` - Hý?ng d?n setup database

**Architecture gi? ð? hoàn h?o!** ???
