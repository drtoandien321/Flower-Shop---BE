# ?? API Danh Sách S?n Ph?m - Hý?ng D?n Chi Ti?t

## ? Ð? Hoàn Thành!

### ?? Các thành ph?n ð? t?o:

#### 1. **Models**
- ? `Product.cs` - Ð? s?a v?i CategoryId (int) + Navigation property
- ? `Category.cs` - Model danh m?c

#### 2. **DTOs** (`DTOs/ProductDTOs.cs`)
- `ProductDto` - Chi ti?t s?n ph?m ð?y ð? (có Description)
- `ProductListDto` - Danh sách s?n ph?m t?i ýu (không có Description)
- `CategoryDto` - Thông tin danh m?c + s? lý?ng s?n ph?m

#### 3. **Services**
- `IProductService` - Interface
- `ProductService` - Implementation v?i:
  - ? Include Category ð? tránh N+1 query
  - ? Projection ð? t?i ýu performance
  - ? Async/await pattern

#### 4. **API Controller** (`ProductsController.cs`)
- 4 endpoints công khai (không c?n JWT)

---

## ?? API Endpoints

### 1?? L?y T?t C? S?n Ph?m

**Endpoint:** `GET /api/Products`

**Authorization:** ? Không c?n (Public)

**Response:**
```json
{
  "success": true,
  "message": "L?y danh sách s?n ph?m thành công",
  "data": [
    {
      "productId": 1,
      "productName": "Hoa H?ng Ð?",
      "unitPrice": 50000.00,
      "stockQuantity": 100,
      "imageURL": "https://example.com/rose-red.jpg",
      "categoryName": "Hoa Týõi"
    },
    {
      "productId": 2,
      "productName": "Hoa Tulip Vàng",
      "unitPrice": 75000.00,
      "stockQuantity": 50,
      "imageURL": "https://example.com/tulip-yellow.jpg",
      "categoryName": "Hoa Nh?p Kh?u"
    }
  ],
  "total": 2
}
```

---

### 2?? L?y Chi Ti?t S?n Ph?m

**Endpoint:** `GET /api/Products/{id}`

**Example:** `GET /api/Products/1`

**Authorization:** ? Không c?n (Public)

**Response Success (200):**
```json
{
  "success": true,
  "message": "L?y thông tin s?n ph?m thành công",
  "data": {
    "productId": 1,
    "productName": "Hoa H?ng Ð?",
    "unitPrice": 50000.00,
    "stockQuantity": 100,
    "imageURL": "https://example.com/rose-red.jpg",
    "description": "Hoa h?ng ð? týõi, nh?p kh?u t? Ecuador. Thích h?p làm quà t?ng ngý?i yêu, b?n bè.",
    "categoryId": 1,
    "categoryName": "Hoa Týõi"
  }
}
```

**Response Not Found (404):**
```json
{
  "success": false,
  "message": "Không t?m th?y s?n ph?m v?i ID 999"
}
```

---

### 3?? L?y S?n Ph?m Theo Danh M?c

**Endpoint:** `GET /api/Products/category/{categoryId}`

**Example:** `GET /api/Products/category/1`

**Authorization:** ? Không c?n (Public)

**Response:**
```json
{
  "success": true,
  "message": "L?y danh sách s?n ph?m theo danh m?c thành công",
  "data": [
    {
      "productId": 1,
      "productName": "Hoa H?ng Ð?",
      "unitPrice": 50000.00,
      "stockQuantity": 100,
      "imageURL": "https://example.com/rose-red.jpg",
      "categoryName": "Hoa Týõi"
    }
  ],
  "total": 1
}
```

---

### 4?? L?y Danh Sách Danh M?c

**Endpoint:** `GET /api/Products/categories`

**Authorization:** ? Không c?n (Public)

**Response:**
```json
{
  "success": true,
  "message": "L?y danh sách danh m?c thành công",
  "data": [
    {
      "categoryId": 1,
      "categoryName": "Hoa Týõi",
      "productCount": 15
    },
    {
      "categoryId": 2,
      "categoryName": "Hoa Nh?p Kh?u",
      "productCount": 8
    },
    {
      "categoryId": 3,
      "categoryName": "Hoa Khô",
      "productCount": 5
    }
  ],
  "total": 3
}
```

---

## ?? Cách S? D?ng

### Bý?c 1: T?o Migration

```bash
# T?o migration cho Product và Category
dotnet ef migrations add AddProductAndCategory

# Update database
dotnet ef database update
```

### Bý?c 2: Insert Sample Data

```sql
-- Insert Categories
INSERT INTO CATEGORY (CategoryName) VALUES 
('Hoa Týõi'),
('Hoa Nh?p Kh?u'),
('Hoa Khô'),
('Ch?u Cây'),
('Bó Hoa');

-- Insert Sample Products
INSERT INTO PRODUCT (ProductName, UnitPrice, StockQuantity, ImageURL, Description, CategoryID)
VALUES 
-- Hoa Týõi
('Hoa H?ng Ð?', 50000, 100, 'https://example.com/rose-red.jpg', 
 'Hoa h?ng ð? týõi, nh?p kh?u t? Ecuador. Thích h?p làm quà t?ng ngý?i yêu, b?n bè.', 1),
 
('Hoa H?ng Tr?ng', 45000, 80, 'https://example.com/rose-white.jpg',
 'Hoa h?ng tr?ng tinh khôi, tý?ng trýng cho s? trong sáng và thu?n khi?t.', 1),

('Hoa Ly Tr?ng', 60000, 60, 'https://example.com/lily-white.jpg',
 'Hoa ly tr?ng thõm ngát, phù h?p cho các d?p l?, cý?i h?i.', 1),

-- Hoa Nh?p Kh?u
('Hoa Tulip Vàng', 75000, 50, 'https://example.com/tulip-yellow.jpg',
 'Hoa Tulip vàng nh?p kh?u t? Hà Lan. Týõi m?i, ch?t lý?ng cao.', 2),

('Hoa Tulip H?ng', 80000, 40, 'https://example.com/tulip-pink.jpg',
 'Hoa Tulip h?ng nh?p kh?u, màu s?c týõi sáng, b?n lâu.', 2),

-- Hoa Khô
('Hoa Lavender Khô', 120000, 30, 'https://example.com/lavender-dry.jpg',
 'Hoa Lavender khô nh?p kh?u t? Pháp. Thõm lâu, trang trí ð?p.', 3),

('Hoa Baby Khô', 90000, 40, 'https://example.com/baby-dry.jpg',
 'Hoa Baby khô, màu pastel nh? nhàng. Trang trí ph?ng, ch?p ?nh.', 3),

-- Ch?u Cây
('Ch?u Sen Ðá Mix', 150000, 35, 'https://example.com/succulent-mix.jpg',
 'Ch?u sen ðá mix ða d?ng lo?i. D? chãm sóc, trang trí bàn làm vi?c.', 4),

('Ch?u Xýõng R?ng Mini', 80000, 50, 'https://example.com/cactus-mini.jpg',
 'Ch?u xýõng r?ng mini xinh x?n. Phù h?p làm quà t?ng.', 4),

-- Bó Hoa
('Bó Hoa H?ng 20 Bông', 450000, 20, 'https://example.com/bouquet-rose-20.jpg',
 'Bó hoa h?ng ð? 20 bông, gói gi?y kraft sang tr?ng.', 5),

('Bó Hoa Mix Tulip', 550000, 15, 'https://example.com/bouquet-tulip-mix.jpg',
 'Bó hoa Tulip mix nhi?u màu, ph?i cùng hoa baby và lá dýõng x?.', 5);
```

### Bý?c 3: Ch?y ?ng D?ng

```bash
dotnet run --launch-profile https
```

Truy c?p Swagger: `https://localhost:7000/swagger`

---

## ?? Test Trên Swagger

### Test 1: L?y T?t C? S?n Ph?m
1. M? Swagger UI: `https://localhost:7000/swagger`
2. T?m endpoint `GET /api/Products`
3. Click **"Try it out"**
4. Click **"Execute"**
5. Xem response v?i danh sách s?n ph?m

### Test 2: L?y S?n Ph?m Theo ID
1. Endpoint: `GET /api/Products/{id}`
2. Click **"Try it out"**
3. Nh?p ID (ví d?: `1`)
4. Click **"Execute"**
5. Xem chi ti?t s?n ph?m

### Test 3: L?y S?n Ph?m Theo Danh M?c
1. Endpoint: `GET /api/Products/category/{categoryId}`
2. Click **"Try it out"**
3. Nh?p CategoryId (ví d?: `1`)
4. Click **"Execute"**
5. Xem danh sách s?n ph?m trong danh m?c ðó

### Test 4: L?y Danh Sách Danh M?c
1. Endpoint: `GET /api/Products/categories`
2. Click **"Try it out"**
3. Click **"Execute"**
4. Xem t?t c? danh m?c v?i s? lý?ng s?n ph?m

---

## ?? Test B?ng Curl

### L?y t?t c? s?n ph?m
```bash
curl -X GET "https://localhost:7000/api/Products" \
  -H "accept: application/json"
```

### L?y s?n ph?m theo ID
```bash
curl -X GET "https://localhost:7000/api/Products/1" \
  -H "accept: application/json"
```

### L?y s?n ph?m theo danh m?c
```bash
curl -X GET "https://localhost:7000/api/Products/category/1" \
  -H "accept: application/json"
```

### L?y danh sách danh m?c
```bash
curl -X GET "https://localhost:7000/api/Products/categories" \
  -H "accept: application/json"
```

---

## ?? Database Schema

### B?ng PRODUCT
```sql
CREATE TABLE PRODUCT (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(200) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL,
    ImageURL NVARCHAR(500) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    CategoryID INT NOT NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) 
        REFERENCES CATEGORY(CategoryID)
);
```

### B?ng CATEGORY
```sql
CREATE TABLE CATEGORY (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);
```

---

## ?? Response Format Chu?n

### Success Response (200)
```json
{
  "success": true,
  "message": "Thông báo thành công",
  "data": { /* D? li?u */ },
  "total": 10  // Ch? có trong list endpoints
}
```

### Error Response (500)
```json
{
  "success": false,
  "message": "Thông báo l?i",
  "error": "Chi ti?t l?i k? thu?t"
}
```

### Not Found Response (404)
```json
{
  "success": false,
  "message": "Không t?m th?y d? li?u"
}
```

---

## ? Performance Optimizations

### 1. **Include Category**
```csharp
.Include(p => p.Category)
```
- Load Category cùng Product trong 1 query
- Tránh N+1 query problem

### 2. **Projection v?i Select**
```csharp
.Select(p => new ProductListDto { ... })
```
- Ch? l?y fields c?n thi?t
- Gi?m data transfer
- Tãng t?c ð? query

### 3. **Async/Await**
```csharp
async Task<List<ProductListDto>>
```
- Non-blocking I/O
- Tãng kh? nãng x? l? concurrent requests

---

## ?? Troubleshooting

### ? L?i: "No data returned" ho?c "Empty array"
**Nguyên nhân:** Database chýa có d? li?u

**Gi?i pháp:**
```sql
-- Ki?m tra d? li?u
SELECT * FROM CATEGORY;
SELECT * FROM PRODUCT;

-- N?u r?ng, ch?y script insert ? trên
```

---

### ? L?i: "Cannot open database 'Flower_Shop'"
**Nguyên nhân:** Connection string sai ho?c database chýa t?n t?i

**Gi?i pháp:**
```sql
-- T?o database
CREATE DATABASE Flower_Shop;

-- Ki?m tra connection string trong appsettings.json
"ConnectionStrings": {
  "FlowerShopDB": "Server=localhost;Database=Flower_Shop;User Id=sa;Password=12345;TrustServerCertificate=True"
}
```

---

### ? L?i: "Foreign key constraint failed"
**Nguyên nhân:** Insert Product v?i CategoryID không t?n t?i

**Gi?i pháp:**
```sql
-- Ph?i insert Category trý?c
INSERT INTO CATEGORY (CategoryName) VALUES ('Hoa Týõi');

-- Sau ðó m?i insert Product
INSERT INTO PRODUCT (..., CategoryID) VALUES (..., 1);
```

---

### ? L?i: "The instance of entity type 'Product' cannot be tracked"
**Nguyên nhân:** EF Core tracking issue

**Gi?i pháp:** Ð? fix b?ng cách dùng `.Select()` projection thay v? tr? v? entity tr?c ti?p

---

## ?? Có Th? M? R?ng

### 1. **Pagination (Phân trang)**
```csharp
public async Task<PagedResult<ProductListDto>> GetProductsAsync(
    int pageNumber = 1, 
    int pageSize = 10)
{
    var query = _context.Products.Include(p => p.Category);
    
    var total = await query.CountAsync();
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductListDto { ... })
        .ToListAsync();
        
    return new PagedResult<ProductListDto>
    {
        Items = items,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalCount = total
    };
}
```

### 2. **Search (T?m ki?m)**
```csharp
public async Task<List<ProductListDto>> SearchProductsAsync(string keyword)
{
    return await _context.Products
        .Include(p => p.Category)
        .Where(p => p.ProductName.Contains(keyword) || 
                    p.Description.Contains(keyword))
        .Select(p => new ProductListDto { ... })
        .ToListAsync();
}
```

### 3. **Filter (L?c)**
```csharp
public async Task<List<ProductListDto>> FilterProductsAsync(
    decimal? minPrice = null,
    decimal? maxPrice = null,
    int? minStock = null)
{
    var query = _context.Products.Include(p => p.Category).AsQueryable();
    
    if (minPrice.HasValue)
        query = query.Where(p => p.UnitPrice >= minPrice.Value);
        
    if (maxPrice.HasValue)
        query = query.Where(p => p.UnitPrice <= maxPrice.Value);
        
    if (minStock.HasValue)
        query = query.Where(p => p.StockQuantity >= minStock.Value);
        
    return await query
        .Select(p => new ProductListDto { ... })
        .ToListAsync();
}
```

### 4. **Sort (S?p x?p)**
```csharp
public async Task<List<ProductListDto>> GetProductsSortedAsync(string sortBy = "name")
{
    var query = _context.Products.Include(p => p.Category).AsQueryable();
    
    query = sortBy.ToLower() switch
    {
        "price_asc" => query.OrderBy(p => p.UnitPrice),
        "price_desc" => query.OrderByDescending(p => p.UnitPrice),
        "stock" => query.OrderByDescending(p => p.StockQuantity),
        _ => query.OrderBy(p => p.ProductName)
    };
    
    return await query
        .Select(p => new ProductListDto { ... })
        .ToListAsync();
}
```

---

## ?? Best Practices Ð? Áp D?ng

? **Repository Pattern** - Service layer tách bi?t logic
? **DTO Pattern** - Không expose entity tr?c ti?p
? **Async/Await** - Non-blocking operations
? **Include Strategy** - Eager loading ð? tránh N+1
? **Projection** - Select ch? fields c?n thi?t
? **Error Handling** - Try-catch v?i response chu?n
? **Public API** - AllowAnonymous cho catalog
? **Consistent Response** - Format response chu?n

---

## ?? T?ng K?t

API ð? hoàn ch?nh v?i các tính nãng:
- ? 4 endpoints ð?y ð?
- ? Public access (không c?n JWT)
- ? Include category information
- ? Optimized queries
- ? Error handling
- ? Chu?n RESTful

**S?n sàng ð? test ngay!** ??
