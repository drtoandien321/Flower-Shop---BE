# ?? Hý?ng D?n Push Code Lên GitHub

## ? Ð? Hoàn Thành

- ? Kh?i t?o Git repository
- ? T?o `.gitignore` ð? lo?i b? files không c?n thi?t
- ? T?o `README.md` mô t? project
- ? Add t?t c? files vào staging
- ? Commit v?i message: "Initial commit: Ann Flower Shop API with Clean Architecture"

## ?? Bý?c Ti?p Theo

### Bý?c 1: K?t n?i v?i GitHub Repository

**Cách 1: N?u b?n ð? t?o repository trên GitHub (RECOMMENDED)**

```bash
# Thay YOUR_USERNAME và YOUR_REPO_NAME b?ng thông tin c?a b?n
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git

# Ho?c s? d?ng SSH (n?u ð? setup SSH key)
git remote add origin git@github.com:YOUR_USERNAME/YOUR_REPO_NAME.git
```

**Ví d?:**
```bash
git remote add origin https://github.com/tinmai/AnnFlowerShopAPI.git
```

### Bý?c 2: Ð?i tên branch (optional nhýng recommended)

GitHub m?c ð?nh dùng `main` làm branch chính:

```bash
git branch -M main
```

### Bý?c 3: Push code lên GitHub

**L?n ð?u push:**
```bash
git push -u origin main
```

**Các l?n sau:**
```bash
git push
```

---

## ?? X? L? Authentication

### Option 1: HTTPS (D? hõn cho ngý?i m?i)

Khi push l?n ð?u, GitHub s? yêu c?u authentication:

1. **Username**: GitHub username c?a b?n
2. **Password**: **KHÔNG ph?i password GitHub**, mà là **Personal Access Token**

#### T?o Personal Access Token:

1. Truy c?p: https://github.com/settings/tokens
2. Click "Generate new token" ? "Generate new token (classic)"
3. Ð?t tên: `AnnFlowerProject`
4. Ch?n quy?n:
   - ? `repo` (full control)
   - ? `workflow` (n?u dùng GitHub Actions)
5. Click "Generate token"
6. **QUAN TR?NG**: Copy token và lýu l?i (ch? hi?n 1 l?n!)

#### S? d?ng Token:

```bash
git push -u origin main

# Khi ðý?c h?i:
Username: your_github_username
Password: paste_your_token_here
```

### Option 2: SSH (T?t hõn cho lâu dài)

#### T?o SSH Key:

```bash
# T?o SSH key
ssh-keygen -t ed25519 -C "your_email@example.com"

# Nh?n Enter 3 l?n (không c?n passphrase)

# Copy public key
cat ~/.ssh/id_ed25519.pub
```

#### Thêm SSH Key vào GitHub:

1. Truy c?p: https://github.com/settings/keys
2. Click "New SSH key"
3. Title: `Ann Flower Project`
4. Paste public key vào "Key"
5. Click "Add SSH key"

#### Test SSH Connection:

```bash
ssh -T git@github.com
# N?u thành công s? hi?n: "Hi username! You've successfully authenticated..."
```

#### Ð?i remote URL sang SSH:

```bash
git remote set-url origin git@github.com:YOUR_USERNAME/YOUR_REPO_NAME.git
git push -u origin main
```

---

## ?? Các L?nh Git H?u Ích

### Ki?m tra tr?ng thái

```bash
# Xem tr?ng thái hi?n t?i
git status

# Xem l?ch s? commit
git log --oneline

# Xem remote repository
git remote -v
```

### C?p nh?t code

```bash
# Khi b?n thay ð?i code:
git add .
git commit -m "Mô t? thay ð?i"
git push
```

### Pull code t? GitHub

```bash
# L?y code m?i nh?t t? GitHub
git pull origin main
```

### T?o branch m?i

```bash
# T?o và chuy?n sang branch m?i
git checkout -b feature/new-feature

# Push branch m?i lên GitHub
git push -u origin feature/new-feature
```

---

## ?? Lýu ? Quan Tr?ng

### 1. Không commit sensitive data

**.gitignore** ð? ðý?c c?u h?nh ð? lo?i b?:
- ? `appsettings.Development.json` (có connection string)
- ? `bin/` và `obj/` (build artifacts)
- ? `.vs/` (Visual Studio cache)

### 2. Ki?m tra trý?c khi commit

```bash
# Xem nh?ng g? s? ðý?c commit
git status

# Xem chi ti?t thay ð?i
git diff
```

### 3. Connection String

**QUAN TR?NG**: Ð?ng commit connection string th?t!

Trong `appsettings.json`, dùng connection string m?u:
```json
{
  "ConnectionStrings": {
    "FlowerShopDB": "Server=localhost;Database=Flower_Shop;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

Thông tin th?t nên ð? trong:
- `appsettings.Development.json` (ð? b? ignore)
- Ho?c User Secrets

---

## ?? Workflow Th?c T?

### Setup l?n ð?u:

```bash
# 1. K?t n?i v?i GitHub repo
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git

# 2. Ð?i branch sang main
git branch -M main

# 3. Push code
git push -u origin main
```

### Làm vi?c hàng ngày:

```bash
# 1. L?y code m?i nh?t (n?u làm team)
git pull

# 2. Code code code...

# 3. Add changes
git add .

# 4. Commit v?i message r? ràng
git commit -m "Add feature: Product CRUD operations"

# 5. Push lên GitHub
git push
```

---

## ?? Troubleshooting

### L?i: "fatal: remote origin already exists"

```bash
# Xóa remote c?
git remote remove origin

# Thêm l?i
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
```

### L?i: "failed to push some refs"

```bash
# Pull trý?c r?i push
git pull origin main --rebase
git push origin main
```

### L?i: Authentication failed

```bash
# N?u dùng HTTPS: C?n dùng Personal Access Token thay v? password
# N?u dùng SSH: Ki?m tra SSH key ð? add vào GitHub chýa
ssh -T git@github.com
```

### L?i: Large files

```bash
# Xem files l?n
git ls-files | xargs ls -lh | sort -k 5 -h -r | head -20

# N?u có file quá l?n (>100MB), c?n dùng Git LFS ho?c exclude
```

---

## ?? GitHub Repository Settings

### Sau khi push thành công:

1. **Thêm Description**: Mô t? ng?n g?n v? project
2. **Add Topics**: `aspnet-core`, `clean-architecture`, `jwt`, `repository-pattern`
3. **Set Visibility**: Public ho?c Private
4. **Enable Issues**: Cho phép track bugs
5. **Setup Branch Protection** (optional):
   - Settings ? Branches ? Add rule
   - Protect `main` branch
   - Require pull request reviews

---

## ?? Tài Li?u Tham Kh?o

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Connecting to GitHub with SSH](https://docs.github.com/en/authentication/connecting-to-github-with-ssh)
- [Managing Personal Access Tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens)

---

## ? Quick Commands Summary

```bash
# Setup (ch? ch?y 1 l?n)
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
git branch -M main
git push -u origin main

# Daily workflow
git pull                    # L?y code m?i
# ... code ...
git add .                   # Stage changes
git commit -m "message"     # Commit
git push                    # Push lên GitHub

# Check status
git status                  # Xem tr?ng thái
git remote -v              # Xem remote URL
git log --oneline          # Xem history
```

---

## ?? Hoàn Thành!

Sau khi push thành công, truy c?p repository trên GitHub:
```
https://github.com/YOUR_USERNAME/YOUR_REPO_NAME
```

B?n s? th?y:
- ? T?t c? source code
- ? README.md hi?n th? ð?p
- ? Commit history
- ? File structure

**Chúc m?ng b?n ð? push code lên GitHub thành công!** ??
