# 📞 Phone Orders Management System

A lightweight web-based internal system for managing phone orders in a tools and hardware retail business.  
Built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.

---

## 🛠 Features

- ✅ **Role-based login** for Operators, Warehouse, and Managers
- 🧾 **Order creation** with custom product entry (name, quantity, price)
- 📋 **Auto-calculated invoice** with total cost
- 📦 **Warehouse approval/rejection system**
- 👤 **Dynamic customer creation** from the order page
- 🔐 **Session-based access control**
- 🖨 **Print-ready invoice view**
- 🎨 Custom landing page with login integration

---

## 🔐 Roles and Access

| Role      | Access                                                                 |
|-----------|------------------------------------------------------------------------|
| Operator  | Create orders, view personal orders                                    |
| Warehouse | View all pending orders, approve/reject them                          |
| Manager   | (Reserved for future: analytics, reporting, user management, etc.)     |

---

## 📸 Screenshots

![Home Banner](./images/welcome.png)

---

## 💾 Technologies Used

- ASP.NET Core MVC (.NET 6+)
- Entity Framework Core
- SQL Server (localdb or full instance)
- Bootstrap 5
- Razor Views (cshtml)

---

## 🚀 Getting Started

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/PhoneOrdersApp.git
   cd PhoneOrdersApp
