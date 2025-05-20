:

📞 Phone Orders Management System
A lightweight internal web application for managing phone orders in a tools & hardware retail environment.

🧱 Built with ASP.NET Core MVC, Entity Framework Core, and SQL Server.

🛠 Features
✅	Description
👥 Role-based login	Separate access for Operators, Warehouse, and Managers
🧾 Smart order creation	Add up to 5 custom products per order with dynamic fields
🔍 Product search & autocomplete	Live search from synced product database
💰 Auto-calculated totals	Quantity × Unit Price, plus grand total calculation
🧾 Print-ready invoices	Invoice view with proper layout for printing
📦 Warehouse panel	Approve/reject incoming orders with required reasons
🔁 Rejected order follow-up	Operators can resubmit or cancel rejected orders
📊 Manager dashboard	Daily stats, per-operator order breakdown, warehouse summary
🧠 Smart customer handling	Suggests address if customer already exists by phone number
🔐 Session-based role enforcement	Only authorized roles can access certain pages
🎨 Custom home banner	Public landing page with integrated login button

🔐 Roles and Access
Role	Access Rights
Operator	Create & view own orders, follow up rejected
Warehouse	View all pending/rejected orders, approve/reject
Manager	Dashboard view, analytics, operator performance tracking



🔹 Custom Order Page (dynamic product fields + total calculator)

🔹 Warehouse Panel with Approval/Reject flow

🔹 Rejected Orders resubmission form

🔹 Manager Dashboard with per-user and warehouse stats

💾 Tech Stack
ASP.NET Core MVC (.NET 6+)

Entity Framework Core

SQL Server (localdb or full instance)

Razor Pages (cshtml)

Bootstrap 5

Session-based authentication (no Identity)

🚀 Getting Started
Clone the repository:

bash
Copy
Edit
git clone https://github.com/yasiGm/PhoneOrdersApp.git
cd PhoneOrdersApp
Configure database connection in appsettings.json

Apply migrations (if applicable):

bash
Copy
Edit
dotnet ef database update
Run the project:

bash
Copy
Edit
dotnet run
✅ Default Users (for testing)
Role	Username	Password
Operator	sara	1234
Warehouse	reza	1234
Manager	mohsen	1234

(Set in DB seeding or create manually)

📂 Folder Structure (simplified)
vbnet
Copy
Edit
PhoneOrdersApp/
├── Controllers/
├── Views/
│   ├── Orders/
│   ├── Warehouse/
│   └── Shared/
├── Models/
├── wwwroot/
├── appsettings.json
└── Program.cs
📌 License
This project is for educational and internal business purposes only.

