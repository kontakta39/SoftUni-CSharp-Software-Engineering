# BookWebStore 📖
📚 Reference for my ASP.NET Advanced project, part of the [*C# Web - June 2025 @ SoftUni*](https://softuni.bg/trainings/4954/asp-net-advanced-june-2025) course, prepared for an assessment.

Welcome to **BookWebStore** – a web application designed to demonstrate my core web development skills through an online bookstore system.

---

## ✨ Features
- 🔐 User registration and login (ASP.NET Identity)
- 🔍 Search across books, genres, and authors
- 🛒 Shopping cart with a dynamic book count
- ✍️ Blog section with role-based permissions
- ⭐ Book ratings and reviews
- 📦 Order creation and management

---

## 👥 Roles & Permissions

### User Roles
- **Guest**: All newly registered users are assigned the Guest role by default. Can write and edit reviews, and place orders.  
- **Moderator**: Can edit certain fields of Books and Blogs. Can also write reviews and place orders.  
- **Administrator**: Can create Genres, Authors, Books, and Blogs. Can edit and delete Blogs and Books. Can also write reviews and place orders.  
- **Master Admin (kontakta39)**: Has all administrator privileges. Can additionally change roles of other users. Stored in the database as an Administrator.  

---

### Blog Permissions

| Role                  | Create | Edit | Delete |
|-----------------------|--------|------|--------|
| Master Admin          | ✅      | ✅    | ✅      |
| Administrator (Owner) | ✅      | ✅    | ✅      |
| Administrator (Not Owner) | ❌      | ❌    | ❌      |
| Moderator             | ❌      | ✅    | ❌      |
| Guest                 | ❌      | ❌    | ❌      |

---

### Summary of Actions by Role

| Action                                  | Guest | Moderator | Administrator | Master Admin |
|----------------------------------------|-------|-----------|---------------|--------------|
| Write & Edit Reviews                    | ✅    | ✅        | ✅            | ✅           |
| Place Orders                            | ✅    | ✅        | ✅            | ✅           |
| Create Genres, Authors, Books, Blogs   | ❌    | ❌        | ✅            | ✅           |
| Edit Books (some fields)                | ❌    | ✅        | ✅            | ✅           |
| Edit Blogs (some fields)                | ❌    | ✅        | ✅            | ✅           |
| Delete Blogs                            | ❌    | ❌        | ✅            | ✅           |
| Change User Roles                        | ❌    | ❌        | ❌            | ✅           |

---

## 🛠️ Technologies Used

### Backend
- **ASP.NET Core MVC**: Robust and scalable web framework.  
- **Entity Framework Core**: Efficient database interactions using LINQ and migrations.  
- **SQL Server**: Relational database used for storing all application data.  
- **ASP.NET Identity**: Secure authentication and authorization system using **custom ApplicationUser and ApplicationRole classes**. Business logic for user and role operations is implemented manually, not scaffolded.  

### Frontend
- **Bootstrap 5**: Responsive and modern UI components.

### Testing
- **NUnit**: Unit testing framework.  
- **Moq**: Mocking framework for unit testing.

---

## 🚀 How to Clone and Run the Project

**1. Install Git**
   
If you don't have Git installed, download it from: [*https://git-scm.com/download/win*](https://git-scm.com/download/win)

During the installation, choose:

**Git from the command line and also from 3rd-party software**

After the installation, verify in CMD or PowerShell:
```bash
git --version
```

**2. Clone only the BookWebStore folder using sparse checkout:**
```bash
git clone --no-checkout https://github.com/kontakta39/SoftUni-CSharp-Software-Engineering.git
cd SoftUni-CSharp-Software-Engineering
git sparse-checkout init --cone
git sparse-checkout set "08. ASP.NET Advanced/BookWebStore"
git checkout main
```

**3. Enter the project folder:**
```bash
cd "08. ASP.NET Advanced/BookWebStore/BookWebStore"
```

**4. Set the connection string with your PC name locally using user secrets:**
```bash
dotnet user-secrets set "ConnectionStrings:BookStoreConnectionString" "Server=(PC_NAME)\SQLEXPRESS;Database=BookStoreDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

**5. Restore dependencies, apply migrations, and run:**
```bash
dotnet restore
dotnet run
```

**6. Open in your browser:**
```bash
https://localhost:7031
```

---

## 🔑 Default Admin Account

**Note:** By deafult, the project comes with pre-configured administrator credentials for local use:

- **Username:** Admin
- **Email:** admin@example.com
- **Password:** Admin123!

This allows you to log in immediately without needing to configure secrets.

---

## 📜 License

This project is licensed under the MIT License. See the [*LICENSE*](LICENSE) file for details.

---

Enjoy exploring the BookWebStore project! 📖🛒
