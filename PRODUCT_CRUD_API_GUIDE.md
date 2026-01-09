# ??? Product CRUD API - Complete Guide

## ? Ð? Hoàn Thành!

API CRUD ð?y ð? cho Product v?i các operations:
- ? **Create** - T?o s?n ph?m m?i
- ? **Read** - L?y danh sách, chi ti?t s?n ph?m
- ? **Update** - C?p nh?t s?n ph?m
- ? **Delete** - Xóa s?n ph?m

---

## ?? Authorization

| Operation | Endpoint | Authorization |
|-----------|----------|---------------|
| **GET** All Products | `GET /api/Products` | ? Public (AllowAnonymous) |
| **GET** Product by ID | `GET /api/Products/{id}` | ? Public (AllowAnonymous) |
| **GET** By Category | `GET /api/Products/category/{id}` | ? Public (AllowAnonymous) |
| **GET** Categories | `GET /api/Products/categories` | ? Public (AllowAnonymous) |
| **POST** Create | `POST /api/Products` | ? Admin Only |
| **PUT** Update | `PUT /api/Products/{id}` | ? Admin Only |
| **DELETE** Delete | `DELETE /api/Products/{id}` | ? Admin Only |

---

## ?? API Endpoints

### 1?? CREATE - T?o S?n Ph?m M?i

**Endpoint:** `POST /api/Products`

**Authorization:** ?? **Admin Only** (C?n JWT Token v?i role Admin)

**Request Body:**
```json
{
  "productName": "Hoa H?ng Ð? Ecuador",
  "unitPrice": 50000,
  "stockQuantity": 100,
  "imageURL": "https://example.com/images/rose-red.jpg",
  "description": "Hoa h?ng ð? nh?p kh?u t? Ecuador, týõi m?i, ch?t lý?ng cao",
  "categoryId": 1
}
```

**Response Success (201 Created):**
```json
{
  "success": true,
  "message": "T?o s?n ph?m thành công",
  "data": {
    "productId": 11,
    "productName": "Hoa H?ng Ð? Ecuador",
    "unitPrice": 50000.00,
    "stockQuantity": 100,
    "imageURL": "https://example.com/images/rose-red.jpg",
    "description": "Hoa h?ng ð? nh?p kh?u t? Ecuador, týõi m?i, ch?t lý?ng cao",
    "categoryId": 1,
    "categoryName": "Hoa Týõi"
  }
}
```

**Response Error (400 Bad Request):**
```json
{
  "success": false,
  "message": "Không th? t?o s?n ph?m. Vui l?ng ki?m tra CategoryId có t?n t?i không."
}
```

**Response Unauthorized (401):**
```json
{
  "success": false,
  "message": "Unauthorized"
}
```

---

### 2?? UPDATE - C?p Nh?t S?n Ph?m

**Endpoint:** `PUT /api/Products/{id}`

**Authorization:** ?? **Admin Only**

**Example:** `PUT /api/Products/11`

**Request Body:**
```json
{
  "productName": "Hoa H?ng Ð? Ecuador Premium",
  "unitPrice": 55000,
  "stockQuantity": 80,
  "imageURL": "https://example.com/images/rose-red-premium.jpg",
  "description": "Hoa h?ng ð? nh?p kh?u t? Ecuador, h?ng premium, kích thý?c l?n",
  "categoryId": 1
}
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "message": "C?p nh?t s?n ph?m thành công",
  "data": {
    "productId": 11,
    "productName": "Hoa H?ng Ð? Ecuador Premium",
    "unitPrice": 55000.00,
    "stockQuantity": 80,
    "imageURL": "https://example.com/images/rose-red-premium.jpg",
    "description": "Hoa h?ng ð? nh?p kh?u t? Ecuador, h?ng premium, kích thý?c l?n",
    "categoryId": 1,
    "categoryName": "Hoa Týõi"
  }
}
```

**Response Not Found (404):**
```json
{
  "success": false,
  "message": "Không t?m th?y s?n ph?m v?i ID 11 ho?c CategoryId không h?p l?"
}
```

---

### 3?? DELETE - Xóa S?n Ph?m

**Endpoint:** `DELETE /api/Products/{id}`

**Authorization:** ?? **Admin Only**

**Example:** `DELETE /api/Products/11`

**Response Success (200 OK):**
```json
{
  "success": true,
  "message": "Xóa s?n ph?m thành công"
}
```

**Response Not Found (404):**
```json
{
  "success": false,
  "message": "Không t?m th?y s?n ph?m v?i ID 11"
}
```

---

### 4?? READ - L?y T?t C? S?n Ph?m

**Endpoint:** `GET /api/Products`

**Authorization:** ? **Public** (Không c?n token)

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
    }
  ],
  "total": 1
}
```

---

### 5?? READ - L?y Chi Ti?t S?n Ph?m

**Endpoint:** `GET /api/Products/{id}`

**Authorization:** ? **Public**

**Example:** `GET /api/Products/1`

**Response:**
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
    "description": "Hoa h?ng ð? týõi nh?p kh?u Ecuador",
    "categoryId": 1,
    "categoryName": "Hoa Týõi"
  }
}
```

---

## ?? Testing v?i Swagger

### Bý?c 1: Ch?y Application
```sh
dotnet run --launch-profile https
```

Truy c?p: `https://localhost:7000/swagger`

---

### Bý?c 2: Test Read Operations (Public)

**Không c?n login**, test tr?c ti?p:

1. `GET /api/Products` - L?y t?t c? s?n ph?m
2. `GET /api/Products/{id}` - L?y chi ti?t
3. `GET /api/Products/category/{categoryId}` - L?c theo category
4. `GET /api/Products/categories` - L?y categories

---

### Bý?c 3: Login ð? l?y Admin Token

#### 3.1. T?o Admin Account

**Option 1: Sign up r?i update role trong database**
```sql
-- Sau khi sign up v?i email admin@flowershop.com
UPDATE USERS 
SET RoleID = 1 
WHERE Email = 'admin@flowershop.com';
```

**Option 2: Insert tr?c ti?p**
```sql
-- Password: "admin123" (ð? hash b?ng BCrypt)
INSERT INTO USERS (FullName, Email, Password, Phone, RoleID)
VALUES (
    'Administrator', 
    'admin@flowershop.com', 
    '$2a$11$XYZ...', -- Hash c?a "admin123"
    '0900000000', 
    1
);
```

#### 3.2. Login v?i Admin Account

**Endpoint:** `POST /api/Auth/login`

**Request:**
```json
{
  "email": "admin@flowershop.com",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "fullname": "Administrator",
    "email": "admin@flowershop.com",
    "phone": "0900000000",
    "roleName": "Admin"
  }
}
```

#### 3.3. Authorize trên Swagger

1. Copy **token** t? response
2. Click nút **"Authorize"** ?? trên Swagger UI
3. Nh?p: `Bearer {your_token}`
   - Ví d?: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
4. Click **"Authorize"**
5. Gi? b?n có th? test các Admin endpoints

---

### Bý?c 4: Test Create Product

**Endpoint:** `POST /api/Products`

**Request Body:**
```json
{
  "productName": "Test Product",
  "unitPrice": 100000,
  "stockQuantity": 50,
  "imageURL": "https://example.com/test.jpg",
  "description": "This is a test product",
  "categoryId": 1
}
```

**Click "Execute"** ? Ki?m tra response 201 Created

---

### Bý?c 5: Test Update Product

**Endpoint:** `PUT /api/Products/{id}`

Nh?p ID c?a product v?a t?o (ví d?: 11)

**Request Body:**
```json
{
  "productName": "Test Product Updated",
  "unitPrice": 120000,
  "stockQuantity": 30,
  "imageURL": "https://example.com/test-updated.jpg",
  "description": "This is an updated test product",
  "categoryId": 1
}
```

**Click "Execute"** ? Ki?m tra response 200 OK

---

### Bý?c 6: Test Delete Product

**Endpoint:** `DELETE /api/Products/{id}`

Nh?p ID c?a product (ví d?: 11)

**Click "Execute"** ? Ki?m tra response 200 OK

---

## ?? Test v?i Postman/cURL

### CREATE Product

```bash
curl -X POST "https://localhost:7000/api/Products" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "productName": "Hoa Tulip Vàng",
    "unitPrice": 75000,
    "stockQuantity": 50,
    "imageURL": "https://example.com/tulip.jpg",
    "description": "Hoa tulip vàng nh?p kh?u Hà Lan",
    "categoryId": 2
  }'
```

---

### UPDATE Product

```bash
curl -X PUT "https://localhost:7000/api/Products/11" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "productName": "Hoa Tulip Vàng Premium",
    "unitPrice": 85000,
    "stockQuantity": 40,
    "imageURL": "https://example.com/tulip-premium.jpg",
    "description": "Hoa tulip vàng nh?p kh?u Hà Lan, h?ng premium",
    "categoryId": 2
  }'
```

---

### DELETE Product

```bash
curl -X DELETE "https://localhost:7000/api/Products/11" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## ?? JWT Token trong Claims

Khi login v?i Admin account, JWT token ch?a:

```json
{
  "nameid": "1",                    // UserId
  "unique_name": "Administrator",   // Fullname
  "email": "admin@flowershop.com",  // Email
  "role": "Admin",                  // Role - QUAN TR?NG!
  "sub": "admin@flowershop.com",
  "jti": "unique-guid",
  "exp": 1234567890                 // Expiration time
}
```

**`role: "Admin"`** là claim quy?t ð?nh authorization cho Create/Update/Delete endpoints.

---

## ?? Validation Rules

### CreateProductDto & UpdateProductDto

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| `productName` | string | ? Yes | Không ðý?c r?ng |
| `unitPrice` | decimal | ? Yes | > 0 |
| `stockQuantity` | int | ? Yes | >= 0 |
| `imageURL` | string | ? Yes | URL h?p l? |
| `description` | string | ? Yes | Không ðý?c r?ng |
| `categoryId` | int | ? Yes | Ph?i t?n t?i trong b?ng CATEGORY |

**Add validation attributes (optional improvement):**

```csharp
using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required(ErrorMessage = "Tên s?n ph?m là b?t bu?c")]
    [StringLength(200, ErrorMessage = "Tên s?n ph?m không ðý?c vý?t quá 200 k? t?")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá là b?t bu?c")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá ph?i l?n hõn 0")]
    public decimal UnitPrice { get; set; }

    [Required(ErrorMessage = "S? lý?ng là b?t bu?c")]
    [Range(0, int.MaxValue, ErrorMessage = "S? lý?ng không ðý?c âm")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "URL h?nh ?nh là b?t bu?c")]
    [Url(ErrorMessage = "URL h?nh ?nh không h?p l?")]
    [StringLength(500)]
    public string ImageURL { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mô t? là b?t bu?c")]
    [StringLength(1000, ErrorMessage = "Mô t? không ðý?c vý?t quá 1000 k? t?")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "CategoryId là b?t bu?c")]
    public int CategoryId { get; set; }
}
```

---

## ?? Error Handling

### Common Errors

#### 1. **401 Unauthorized**
```json
{
  "status": 401,
  "title": "Unauthorized"
}
```
**Nguyên nhân:**
- Không có JWT token
- Token h?t h?n
- Token không h?p l?

**Gi?i pháp:** Login l?i ð? l?y token m?i

---

#### 2. **403 Forbidden**
```json
{
  "status": 403,
  "title": "Forbidden"
}
```
**Nguyên nhân:**
- User có token nhýng không ph?i Admin role
- Token có role "Customer" thay v? "Admin"

**Gi?i pháp:** 
- Login v?i Admin account
- Ho?c update RoleID trong database

---

#### 3. **404 Not Found**
```json
{
  "success": false,
  "message": "Không t?m th?y s?n ph?m v?i ID 999"
}
```
**Nguyên nhân:** ProductId không t?n t?i

**Gi?i pháp:** Ki?m tra ID ðúng chýa

---

#### 4. **400 Bad Request**
```json
{
  "success": false,
  "message": "Không th? t?o s?n ph?m. Vui l?ng ki?m tra CategoryId có t?n t?i không."
}
```
**Nguyên nhân:** CategoryId không t?n t?i trong database

**Gi?i pháp:** 
```sql
-- Ki?m tra categories có s?n
SELECT * FROM CATEGORY;

-- Ho?c dùng API
GET /api/Products/categories
```

---

#### 5. **500 Internal Server Error**
```json
{
  "success": false,
  "message": "Có l?i x?y ra khi t?o s?n ph?m",
  "error": "Chi ti?t l?i..."
}
```
**Nguyên nhân:** L?i server (database, connection, etc.)

**Gi?i pháp:** Ki?m tra logs, database connection

---

## ?? Complete CRUD Workflow

### Workflow 1: Customer (Read Only)

```
1. Customer truy c?p website
   ?
2. GET /api/Products
   ? Xem danh sách s?n ph?m (Public, không c?n login)
   ?
3. GET /api/Products/{id}
   ? Xem chi ti?t s?n ph?m
   ?
4. GET /api/Products/category/{categoryId}
   ? L?c s?n ph?m theo danh m?c
```

---

### Workflow 2: Admin (Full CRUD)

```
1. Admin login
   POST /api/Auth/login
   ? Nh?n JWT token v?i role "Admin"
   ?
2. Authorize v?i token
   ? Click "Authorize" trên Swagger
   ?
3. CREATE s?n ph?m m?i
   POST /api/Products
   ?
4. UPDATE s?n ph?m
   PUT /api/Products/{id}
   ?
5. DELETE s?n ph?m
   DELETE /api/Products/{id}
```

---

## ?? Business Logic trong Service

### CreateProductAsync

1. ? Validate CategoryId exists
2. ? Create new Product entity
3. ? Add to repository
4. ? Save changes
5. ? Load category information
6. ? Return ProductDto with category name

### UpdateProductAsync

1. ? Get existing product
2. ? Check product exists
3. ? Validate new CategoryId exists
4. ? Update all properties
5. ? Save changes
6. ? Return updated ProductDto

### DeleteProductAsync

1. ? Get product by ID
2. ? Check exists
3. ? Remove from repository
4. ? Save changes
5. ? Return success/failure

---

## ?? Testing Checklist

### ? Create Product

- [ ] Create v?i data h?p l? ? 201 Created
- [ ] Create v?i CategoryId không t?n t?i ? 400 Bad Request
- [ ] Create không có token ? 401 Unauthorized
- [ ] Create v?i Customer token ? 403 Forbidden
- [ ] Create v?i Admin token ? 201 Created

### ? Update Product

- [ ] Update v?i ProductId h?p l? ? 200 OK
- [ ] Update v?i ProductId không t?n t?i ? 404 Not Found
- [ ] Update v?i CategoryId không t?n t?i ? 404 Not Found
- [ ] Update không có token ? 401 Unauthorized
- [ ] Update v?i Customer token ? 403 Forbidden

### ? Delete Product

- [ ] Delete v?i ProductId h?p l? ? 200 OK
- [ ] Delete v?i ProductId không t?n t?i ? 404 Not Found
- [ ] Delete không có token ? 401 Unauthorized
- [ ] Delete v?i Customer token ? 403 Forbidden

---

## ?? T?ng K?t

### ? Ð? Implement:

| Feature | Status |
|---------|--------|
| Read All Products | ? Public |
| Read Product Detail | ? Public |
| Read By Category | ? Public |
| Read Categories | ? Public |
| Create Product | ? Admin Only |
| Update Product | ? Admin Only |
| Delete Product | ? Admin Only |
| JWT Authorization | ? Role-based |
| Validation | ? Service layer |
| Error Handling | ? Try-catch |
| Clean Architecture | ? Service ? UnitOfWork ? Repository |

---

## ?? Next Steps

### Có th? m? r?ng thêm:

1. **Pagination** cho GetAllProducts
2. **Search** s?n ph?m theo tên
3. **Filter** theo giá, stock
4. **Sort** theo giá, tên, ngày t?o
5. **Bulk operations** (create/update nhi?u s?n ph?m)
6. **Image upload** thay v? URL
7. **Soft delete** thay v? hard delete
8. **Audit logs** (track who created/updated)

**API CRUD hoàn ch?nh và production-ready!** ??
