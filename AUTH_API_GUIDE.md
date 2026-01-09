# API Login và Sign Up - Hý?ng d?n s? d?ng

## ? Ð? hoàn thành!

### Các thành ph?n ð? t?o:

1. **DTOs** (`DTOs/AuthDTOs.cs`):
   - `LoginRequestDto` - Request ðãng nh?p
   - `LoginResponseDto` - Response ðãng nh?p
   - `SignUpRequestDto` - Request ðãng k?
   - `UserDto` - Thông tin user

2. **Service** (`Services/AuthService.cs`):
   - `LoginAsync()` - X? l? ðãng nh?p
   - `SignUpAsync()` - X? l? ðãng k?
   - `IsEmailExistsAsync()` - Ki?m tra email
   - Password hashing v?i BCrypt

3. **Controller** (`Controllers/AuthController.cs`):
   - `POST /api/Auth/login` - Ðãng nh?p
   - `POST /api/Auth/signup` - Ðãng k?
   - `GET /api/Auth/check-email` - Ki?m tra email

---

## ?? Cách s? d?ng

### Bý?c 1: T?o Migration và Update Database

```bash
# T?o migration
dotnet ef migrations add AddUserAndRole

# Update database
dotnet ef database update
```

### Bý?c 2: Ch?y ?ng d?ng

```bash
dotnet run --launch-profile https
```

Truy c?p Swagger: `https://localhost:7000/swagger`

---

## ?? API Endpoints

### 1. Ðãng k? tài kho?n m?i

**Endpoint:** `POST /api/Auth/signup`

**Request Body:**
```json
{
  "fullname": "Nguy?n Vãn A",
  "email": "nguyenvana@example.com",
  "password": "123456",
  "phone": "0901234567"
}
```

**Response Success (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "fullname": "Nguy?n Vãn A",
    "email": "nguyenvana@example.com",
    "phone": "0901234567",
    "roleId": 2,
    "roleName": "Customer"
  }
}
```

**Response Error (400):**
```json
{
  "message": "Email ð? ðý?c s? d?ng"
}
```

---

### 2. Ðãng nh?p

**Endpoint:** `POST /api/Auth/login`

**Request Body:**
```json
{
  "email": "nguyenvana@example.com",
  "password": "123456"
}
```

**Response Success (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "fullname": "Nguy?n Vãn A",
    "email": "nguyenvana@example.com",
    "phone": "0901234567",
    "roleId": 2,
    "roleName": "Customer"
  }
}
```

**Response Error (401):**
```json
{
  "message": "Email ho?c m?t kh?u không ðúng"
}
```

---

### 3. Ki?m tra email ð? t?n t?i

**Endpoint:** `GET /api/Auth/check-email?email=test@example.com`

**Response:**
```json
{
  "exists": true
}
```

---

## ?? S? d?ng JWT Token

Sau khi ðãng nh?p ho?c ðãng k? thành công, b?n s? nh?n ðý?c JWT token.

### Cách s? d?ng trong Swagger:

1. Copy token t? response
2. Click nút **"Authorize"** trên Swagger UI
3. Nh?p: `Bearer {your_token}`
4. Click "Authorize"

### Cách s? d?ng trong code/API client:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## ?? Test Flow

### Test 1: Ðãng k? tài kho?n m?i

```bash
curl -X POST https://localhost:7000/api/Auth/signup \
  -H "Content-Type: application/json" \
  -d '{
    "fullname": "Test User",
    "email": "test@example.com",
    "password": "123456",
    "phone": "0901234567"
  }'
```

### Test 2: Ðãng nh?p

```bash
curl -X POST https://localhost:7000/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "123456"
  }'
```

### Test 3: S? d?ng token ð? truy c?p API ðý?c b?o v?

```bash
curl -X GET https://localhost:7000/api/Products \
  -H "Authorization: Bearer {your_token}"
```

---

## ?? B?o m?t

### Password Hashing
- S? d?ng **BCrypt** ð? hash password
- Password không bao gi? lýu dý?i d?ng plain text
- BCrypt t? ð?ng thêm salt ng?u nhiên

### JWT Token Claims
Token ch?a các thông tin:
- `UserId` - ID ngý?i dùng
- `Name` - Tên ð?y ð?
- `Email` - Email
- `Role` - Vai tr? (Admin/Customer)
- `Jti` - Unique token ID
- `Exp` - Th?i gian h?t h?n (24 gi?)

---

## ?? Roles

H? th?ng có 2 roles m?c ð?nh:

| RoleId | RoleName | Mô t? |
|--------|----------|-------|
| 1 | Admin | Qu?n tr? viên |
| 2 | Customer | Khách hàng |

- **Sign Up t? ð?ng:** User m?i ðý?c t?o v?i role **Customer** (RoleId = 2)
- **Admin account:** C?n t?o th? công trong database

---

## ??? T?o Admin Account

### Cách 1: Thông qua SQL

```sql
-- Hash password "admin123" b?ng BCrypt
INSERT INTO USERS (FullName, Email, Password, Phone, RoleID)
VALUES ('Administrator', 'admin@flowershop.com', '$2a$11$...', '0900000000', 1);
```

### Cách 2: Sign Up r?i update RoleId

1. Ðãng k? account b?nh thý?ng
2. Update RoleId trong database:

```sql
UPDATE USERS 
SET RoleID = 1 
WHERE Email = 'youremail@example.com';
```

---

## ?? Validation Rules

### Email:
- B?t bu?c
- Ph?i ðúng ð?nh d?ng email
- Không ðý?c trùng trong database

### Password:
- B?t bu?c
- T?i thi?u 6 k? t?

### Fullname:
- B?t bu?c
- T?i ða 100 k? t?

### Phone:
- B?t bu?c
- Ph?i ðúng ð?nh d?ng s? ði?n tho?i

---

## ?? Error Codes

| Status Code | Mô t? |
|-------------|-------|
| 200 | Success |
| 400 | Bad Request (validation error, email exists) |
| 401 | Unauthorized (wrong credentials) |
| 500 | Internal Server Error |

---

## ?? Next Steps

### Tính nãng có th? m? r?ng:

1. **Forgot Password**
   - Send email reset password
   - Reset password v?i token

2. **Refresh Token**
   - T? ð?ng làm m?i token khi h?t h?n

3. **Email Verification**
   - Xác th?c email sau khi ðãng k?

4. **Two-Factor Authentication (2FA)**
   - B?o m?t tãng cý?ng

5. **Social Login**
   - Google, Facebook login

6. **Account Management**
   - Update profile
   - Change password
   - Deactivate account

---

## ?? Support

N?u g?p v?n ð?, ki?m tra:
1. Database connection string ðúng chýa
2. Ð? ch?y migration chýa
3. BCrypt package ð? ðý?c cài chýa
4. JWT settings trong appsettings.json ðúng chýa
