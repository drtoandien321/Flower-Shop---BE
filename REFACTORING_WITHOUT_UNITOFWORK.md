# ?? Refactoring: Simplified Architecture Without Unit of Work

## ? Ð? Hoàn Thành Refactoring!

### ?? Thay Ð?i Architecture

#### **Trý?c (V?i Unit of Work):**
```
Controller ? Service ? UnitOfWork ? Repository ? DbContext ? Database
```

#### **Sau (Ðõn gi?n hõn):**
```
Controller ? Service ? Repository ? DbContext ? Database
```

---

## ?? L? Do Refactor

### **Khi nào KHÔNG c?n Unit of Work?**

? **Project nh?/trung b?nh** - Không có logic ph?c t?p
? **CRUD ðõn gi?n** - M?i operation ch? thao tác 1-2 entities  
? **Không c?n transaction ph?c t?p** - Không có multi-step operations
? **Team nh?** - Ít ngý?i, d? maintain
? **Gi?m complexity** - Code ðõn gi?n hõn, d? hi?u hõn

### **Khi nào NÊN dùng Unit of Work?**

? **E-commerce l?n** - Order processing ph?c t?p
? **Financial system** - C?n ACID transactions ch?t ch?
? **Multi-step operations** - 1 business logic thao tác nhi?u tables
? **Team l?n** - C?n standardization và coordination

---

## ?? Nh?ng G? Ð? Thay Ð?i

### 1?? **Xóa UnitOfWork Layer**

```diff
- AnnFlowerProject/
-   ??? UnitOfWork/
-       ??? IUnitOfWork.cs
-       ??? UnitOfWork.cs
```

? **L?i ích:**
- Gi?m 2 files không c?n thi?t
- Gi?m abstraction layer
- Code ðõn gi?n hõn

---

### 2?? **Repository có SaveChangesAsync**

**Trý?c:**
```csharp
public interface IRepository<T>
{
    Task AddAsync(T entity);
    void Update(T entity);
    // Không có SaveChanges
}
```

**Sau:**
```csharp
public interface IRepository<T>
{
    Task AddAsync(T entity);
    void Update(T entity);
    Task<int> SaveChangesAsync(); // ? Thêm SaveChanges
}
```

**Implementation:**
```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FlowerShopDbContext _context;
    
    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
```

---

### 3?? **Service inject Repositories tr?c ti?p**

#### **ProductService:**

**Trý?c:**
```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork; // ? Inject UnitOfWork
    
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateProduct(CreateProductDto dto)
    {
        var product = new Product { /* ... */ };
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(); // Qua UnitOfWork
    }
}
```

**Sau:**
```csharp
public class ProductService
{
    private readonly IProductRepository _productRepository; // ? Inject Repository
    private readonly ICategoryRepository _categoryRepository;
    
    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task CreateProduct(CreateProductDto dto)
    {
        var product = new Product { /* ... */ };
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync(); // Tr?c ti?p qua Repository
    }
}
```

---

#### **AuthService:**

**Trý?c:**
```csharp
public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task SignUp(SignUpRequestDto dto)
    {
        var user = new User { /* ... */ };
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
```

**Sau:**
```csharp
public class AuthService
{
    private readonly IUserRepository _userRepository; // ? Tr?c ti?p
    
    public async Task SignUp(SignUpRequestDto dto)
    {
        var user = new User { /* ... */ };
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync(); // ? Ðõn gi?n hõn
    }
}
```

---

### 4?? **Program.cs - Simplified DI Registration**

**Trý?c:**
```csharp
// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // ? Extra layer

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
```

**Sau:**
```csharp
// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register Services (inject repositories directly)
builder.Services.AddScoped<IAuthService, AuthService>(); // ? Ðõn gi?n
builder.Services.AddScoped<IProductService, ProductService>();
```

? **L?i ích:**
- Ít dependencies hõn
- Clear và d? hi?u
- D? debug

---

## ?? So Sánh Chi Ti?t

### **Architecture Flow**

#### Trý?c:
```
ProductsController
    ? inject IProductService
ProductService
    ? inject IUnitOfWork
UnitOfWork
    ? expose IProductRepository, ICategoryRepository
ProductRepository, CategoryRepository
    ? inject FlowerShopDbContext
DbContext
    ?
SQL Server
```

**S? layers:** 6 layers  
**Dependencies:** Service depends on UnitOfWork, UnitOfWork depends on Repositories

---

#### Sau:
```
ProductsController
    ? inject IProductService
ProductService
    ? inject IProductRepository, ICategoryRepository
ProductRepository, CategoryRepository
    ? inject FlowerShopDbContext
DbContext
    ?
SQL Server
```

**S? layers:** 5 layers  
**Dependencies:** Service depends directly on Repositories

? **Gi?m 1 abstraction layer**

---

## ?? Code Examples

### **Create Product - Trý?c vs Sau**

#### Trý?c (V?i UnitOfWork):
```csharp
public async Task<ProductDto?> CreateProductAsync(CreateProductDto dto)
{
    var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
    if (category == null) return null;
    
    var product = new Product { /* ... */ };
    await _unitOfWork.Products.AddAsync(product);
    await _unitOfWork.SaveChangesAsync(); // ? Qua UnitOfWork
    
    var created = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.ProductId);
    return MapToDto(created);
}
```

#### Sau (Không có UnitOfWork):
```csharp
public async Task<ProductDto?> CreateProductAsync(CreateProductDto dto)
{
    var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
    if (category == null) return null;
    
    var product = new Product { /* ... */ };
    await _productRepository.AddAsync(product);
    await _productRepository.SaveChangesAsync(); // ? Tr?c ti?p qua Repository
    
    var created = await _productRepository.GetByIdWithCategoryAsync(product.ProductId);
    return MapToDto(created);
}
```

? **Ðõn gi?n hõn, ít boilerplate code**

---

### **Sign Up - Trý?c vs Sau**

#### Trý?c:
```csharp
public async Task<LoginResponseDto?> SignUpAsync(SignUpRequestDto dto)
{
    if (await _unitOfWork.Users.IsEmailExistsAsync(dto.Email))
        return null;
    
    var user = new User { /* ... */ };
    await _unitOfWork.Users.AddAsync(user);
    await _unitOfWork.SaveChangesAsync(); // ? Qua UnitOfWork
    
    var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(user.UserId);
    return GenerateResponse(userWithRole);
}
```

#### Sau:
```csharp
public async Task<LoginResponseDto?> SignUpAsync(SignUpRequestDto dto)
{
    if (await _userRepository.IsEmailExistsAsync(dto.Email))
        return null;
    
    var user = new User { /* ... */ };
    await _userRepository.AddAsync(user);
    await _userRepository.SaveChangesAsync(); // ? Tr?c ti?p
    
    var userWithRole = await _userRepository.GetByIdWithRoleAsync(user.UserId);
    return GenerateResponse(userWithRole);
}
```

? **R? ràng hõn, d? ð?c hõn**

---

## ?? Trade-offs (Ðánh ð?i)

### ? **Nh?ng g? m?t ði:**

1. **Transaction Coordination** - Không có central point ð? manage transactions
   ```csharp
   // Không c?n có th? làm:
   await _unitOfWork.BeginTransactionAsync();
   // ... multiple operations
   await _unitOfWork.CommitTransactionAsync();
   ```

2. **Single SaveChanges Point** - M?i repository có SaveChanges riêng
   ```csharp
   // Trý?c: 1 SaveChanges cho t?t c?
   await _unitOfWork.SaveChangesAsync();
   
   // Sau: M?i repo riêng
   await _productRepository.SaveChangesAsync();
   await _categoryRepository.SaveChangesAsync();
   ```

3. **Shared DbContext Guarantee** - Không ð?m b?o cùng 1 DbContext instance
   - Nhýng v?i DI Scoped lifetime, v?n OK trong 1 request

---

### ? **Nh?ng g? có ðý?c:**

1. **Simplicity** - Code ðõn gi?n hõn nhi?u
2. **Less Abstraction** - Ít layers, d? hi?u
3. **Direct Access** - Service g?i th?ng Repository
4. **Easier Testing** - Mock repositories thay v? UnitOfWork
5. **Faster Development** - Ít boilerplate code

---

## ?? Khi Nào C?n Transaction?

N?u b?n c?n complex transactions, có th? dùng tr?c ti?p DbContext:

```csharp
public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly FlowerShopDbContext _context; // Inject DbContext
    
    public async Task<bool> ComplexOperation()
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Step 1
            var product = new Product { /* ... */ };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            
            // Step 2
            // ... other operations
            
            // Commit
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

---

## ?? C?u Trúc Project Sau Refactoring

```
AnnFlowerProject/
??? Controllers/
?   ??? AuthController.cs
?   ??? ProductsController.cs
?   ??? WeatherForecastController.cs
?
??? Services/
?   ??? Interfaces/
?   ?   ??? IAuthService.cs
?   ?   ??? IProductService.cs
?   ??? Implementations/
?       ??? AuthService.cs          ? Inject Repositories
?       ??? ProductService.cs       ? Inject Repositories
?
??? Repositories/
?   ??? Interfaces/
?   ?   ??? IRepository.cs          ? Có SaveChangesAsync
?   ?   ??? IProductRepository.cs
?   ?   ??? ICategoryRepository.cs
?   ?   ??? IUserRepository.cs
?   ??? Implementations/
?       ??? Repository.cs           ? Implement SaveChangesAsync
?       ??? ProductRepository.cs
?       ??? CategoryRepository.cs
?       ??? UserRepository.cs
?
??? DTOs/
??? Models/
??? Data/
??? Program.cs                      ? Simplified DI
```

**? Ð? xóa:**
```
UnitOfWork/
??? IUnitOfWork.cs
??? UnitOfWork.cs
```

---

## ?? T?ng K?t

### **Refactoring Summary:**

| Aspect | Trý?c | Sau |
|--------|-------|-----|
| **Layers** | 6 layers | 5 layers |
| **Files** | +2 files (UnitOfWork) | -2 files |
| **Complexity** | Medium-High | Low-Medium |
| **Transaction Support** | ? Built-in | ?? Manual (if needed) |
| **Code Volume** | More | Less |
| **Learning Curve** | Steep | Gentle |
| **Suitable For** | Large/Complex apps | Small/Medium apps |

---

### **Khi Nào Dùng Architecture Nào?**

#### **Simple Architecture (Hi?n t?i):**
```
? CRUD applications
? Blog, CMS, Portfolio sites
? Small e-commerce
? Internal tools
? MVPs and prototypes
```

#### **UnitOfWork Architecture:**
```
? Banking systems
? Large e-commerce platforms
? ERP systems
? Multi-tenant SaaS
? Complex business logic
```

---

## ?? Architecture sau refactoring phù h?p v?i:

- ? **Ann Flower Shop** - E-commerce nh? v?i CRUD ðõn gi?n
- ? **Easy to understand** - Cho beginners
- ? **Quick development** - Ít boilerplate
- ? **Maintainable** - Code clear và straightforward

**Architecture này HOÀN H?O cho project c?a b?n!** ??
