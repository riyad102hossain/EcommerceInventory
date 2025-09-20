
# EcommerceInventory

An **E-commerce Inventory Management API** built with **ASP.NET Core**, **Entity Framework Core**, and **JWT Authentication**.  
This project provides APIs to manage **Products, Categories**, and authentication/authorization for secure access.

---

## 🚀 Features
- User Authentication using **JWT**
- CRUD for **Categories** and **Products**
- Image upload support (via Base64 or file)
- Repository & Unit of Work pattern
- Entity Framework Core with **Code First Migrations**

---

## 🛠️ Tech Stack
- **ASP.NET Core 7 / 8**
- **Entity Framework Core**
- **SQL Server (MSSQL)** with SQL Server Management Studio (SSMS)
- **JWT Authentication**
- **Swagger** for API documentation

---

## 📦 Setup Instructions

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/riyad102hossain/EcommerceInventory.git
cd EcommerceInventory
```
# ⚙️ Update Database Connection

Edit your **`appsettings.json`** with your database connection string:

```json
"ConnectionStrings": {
   "DefaultConnection": "Server=RIYAD\\SQLEXPRESS01;Database=ecomm_db;Trusted_Connection=True;TrustServerCertificate=True;"
 }
 ```

### 🛠️ Apply Migrations
```bash
dotnet ef migrations add InitialCreate2 -p EcommerceInventory.Infrastructure/EcommerceInventory.Infrastructure.csproj -s EcommerceInventoryAPI/EcommerceInventoryAPI.csproj
dotnet ef database update -p EcommerceInventory.Infrastructure/EcommerceInventory.Infrastructure.csproj -s EcommerceInventoryAPI/EcommerceInventoryAPI.csproj
```

### Run the Application
```bash
dotnet run --project EcommerceInventoryAPI/EcommerceInventoryAPI.csproj
```

### Access Swagger Docs

Once running, open: 

https://localhost:7031/swagger
