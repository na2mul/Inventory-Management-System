# 📦 Inventory Management System

[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

> An enterprise-grade inventory management solution built with ASP.NET Core, featuring Clean Architecture principles and Domain-Driven Design patterns.

---

## 🌟 Overview

DevSkill Inventory is a comprehensive inventory management system designed for businesses to efficiently manage their products, sales, customers, and financial transactions. Built with modern .NET technologies and cloud-native architecture, this application ensures scalability, security, and maintainability.

### Why This Project?

- **🏗️ Clean Architecture**: Separation of concerns for better maintainability
- **🔐 Enterprise Security**: Role-based and claim-based access control
- **☁️ Cloud-Ready**: AWS S3 and SQS integration
- **📊 Real-Time Insights**: Comprehensive reporting and analytics
- **🐳 Container-First**: Docker support for easy deployment

---

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Technology Stack](#-technology-stack)
- [Getting Started](#-getting-started)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Core Modules](#-core-modules)
- [Docker Deployment](#-docker-deployment)
- [Testing](#-testing)
- [Contributing](#-contributing)
- [License](#-license)
- [Contact](#-contact)

---

## ✨ Features

### 📦 Product Management
- Complete CRUD operations for products
- Category-based product organization
- Stock level tracking with low-stock alerts
- Dual pricing support (MRP, wholesale)
- Image upload with automatic resizing (AWS S3)

### 💰 Sales & Financial Management
- Multi-type sales transactions (MRP, wholesale)
- Payment status tracking
- Customer balance management
- Account transfers (Cash, Bank, Mobile)
- Comprehensive sales reporting

### 👥 Customer Management
- Customer database with purchase history
- Balance tracking
- Customer-specific pricing
- Relationship management

### 🔐 Security & User Management
- Hybrid authentication (role + claim-based)
- Fine-grained permission system
- Employee management
- Department and unit management
- Secure password policies with ASP.NET Core Identity

### ⚡ Advanced Features
- Background job processing for image resizing
- AWS SQS message queuing
- Email notifications
- Real-time dashboard
- Audit logging with Serilog

---

## 🏗️ Architecture

This project implements **Clean Architecture** with **Domain-Driven Design (DDD)** principles, ensuring a clear separation of concerns and high testability.

### Project Structure

```
DevSkill.Inventory/
├── DevSkill.Inventory.Domain/          # Core business logic & entities
│   ├── Entities/                       # Domain models
│   ├── Interfaces/                     # Repository & service contracts
│   └── DTOs/                          # Data transfer objects
│
├── DevSkill.Inventory.Application/     # Use cases & business logic
│   ├── Features/                       # CQRS commands & queries
│   ├── Services/                       # Application services
│   └── Mappings/                       # AutoMapper profiles
│
├── DevSkill.Inventory.Infrastructure/  # External concerns
│   ├── Data/                          # EF Core & repositories
│   ├── Identity/                      # Authentication & authorization
│   └── Services/                      # External service implementations
│
├── DevSkill.Inventory.Web/            # Presentation layer
│   ├── Controllers/                   # MVC controllers
│   ├── Views/                         # Razor views
│   └── wwwroot/                       # Static files
│
└── DevSkill.Inventory.Worker/         # Background processing
    └── Jobs/                          # Scheduled tasks
```

### Design Patterns

| Pattern | Implementation | Benefits |
|---------|---------------|----------|
| 🏛️ **Repository** | Data access abstraction | Testability, persistence ignorance |
| 🔄 **Unit of Work** | Transaction management | Data consistency, atomic operations |
| ⚡ **CQRS** | MediatR implementation | Scalability, separation of concerns |
| 🔌 **Dependency Injection** | Autofac container | Loose coupling, maintainability |

---

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core MVC (.NET 9)
- **ORM**: Entity Framework Core
- **Database**: Microsoft SQL Server (with stored procedures)
- **Authentication**: ASP.NET Core Identity (extended)
- **Patterns**: CQRS via MediatR

### Frontend
- **UI Framework**: AdminLTE
- **Templating**: Razor Pages
- **Styling**: Bootstrap 5 + Custom CSS
- **JavaScript**: Modern ES6+

### Cloud & DevOps
- **Storage**: Amazon S3 (image storage)
- **Messaging**: Amazon SQS (async processing)
- **Containerization**: Docker & Docker Compose
- **Logging**: Serilog

### Development Tools
- **DI Container**: Autofac
- **Object Mapping**: AutoMapper
- **Testing**: NUnit, Moq, Shouldly
- **Code Coverage**: Coverlet

---

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- ✅ [.NET 9 SDK](https://dotnet.microsoft.com/download)
- ✅ [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [Docker](https://www.docker.com/)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- ✅ [EF Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

### Optional (for full features)
- 🪣 AWS Account with S3 bucket
- 📨 AWS SQS Queue configured
- 🔑 AWS IAM credentials

---

## 📥 Installation

### Method 1: Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/DevSkill.Inventory.git
   cd DevSkill.Inventory
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection**
   
   Edit `DevSkill.Inventory.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=InventoryDB;Trusted_Connection=true;"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   cd DevSkill.Inventory.Web
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```
   
   Access at: `https://localhost:5001` or `http://localhost:5000`

### Method 2: Docker Deployment (Recommended)

1. **Clone and navigate**
   ```bash
   git clone https://github.com/your-username/DevSkill.Inventory.git
   cd DevSkill.Inventory
   ```

2. **Configure environment variables**
   
   Create `.env` file:
   ```env
   DB_CONNECTION=Server=sql-server;Database=InventoryDB;User=sa;Password=YourStrong@Passw0rd
   AWS_REGION=us-east-1
   AWS_ACCESS_KEY=your_access_key
   AWS_SECRET_KEY=your_secret_key
   AWS_S3_BUCKET=your-bucket-name
   AWS_SQS_URL=your-sqs-url
   ```

3. **Launch with Docker Compose**
   ```bash
   docker-compose up --build -d
   ```
   
   Access at: `http://localhost:8000`

---

## ⚙️ Configuration

### AWS Configuration (Optional)

For full functionality with image storage and background processing:

```json
{
  "AWS": {
    "Profile": "your-profile",
    "Region": "us-east-1",
    "BucketName": "your-inventory-bucket",
    "SqsUrl": "https://sqs.us-east-1.amazonaws.com/your-account/your-queue",
    "UrlExpiryMinutes": 15
  }
}
```

### Default Admin Credentials

After first run:
- **Email**: `nazmul@gmail.com`
- **Password**: `123456`

⚠️ **Important**: Change these credentials immediately after first login!

---

## 🧩 Core Modules

### Accounts Module
Manages financial accounts and transaction types:
- Cash accounts
- Bank accounts
- Mobile banking accounts
- Balance transfers

### Categories Module
Product categorization system:
- Hierarchical categories
- Category-based filtering
- Bulk category operations

### Customers Module
Customer relationship management:
- Customer profiles
- Purchase history
- Balance tracking
- Customer analytics

### Products Module
Core inventory functionality:
- Product CRUD operations
- Stock management
- Pricing (MRP & wholesale)
- Image management
- Low-stock alerts

### Sales Module
Sales transaction processing:
- Order creation
- Payment tracking
- Sales reports
- Invoice generation

### Users Module
Identity and access management:
- User authentication
- Role management
- Claim-based permissions
- Employee assignments

---

## 🐳 Docker Deployment

The project includes a complete Docker setup with multi-service orchestration.

### Docker Compose Structure

```yaml
services:
  - web: ASP.NET Core web application
  - worker: Background processing service
  - sql-server: Microsoft SQL Server database
```

### Quick Start with Docker

```bash
# Build and start all services
docker-compose up --build -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Stop and remove volumes (⚠️ deletes data)
docker-compose down -v
```

---

## 🧪 Testing

Run the comprehensive test suite:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test project
dotnet test DevSkill.Inventory.Tests
```

### Test Structure
- **Unit Tests**: Domain and application logic
- **Integration Tests**: Database and external services
- **E2E Tests**: Complete user workflows

---

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork** the repository
2. **Create** a feature branch
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. **Commit** your changes
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. **Push** to the branch
   ```bash
   git push origin feature/AmazingFeature
   ```
5. **Open** a Pull Request

### Code Standards
- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 📞 Contact

**Nazmul**

- 💼 **LinkedIn**: [https://linkedin.com/in/md-nazmulhasan](https://www.linkedin.com/in/md-nazmulhasan/)
- 🐙 **GitHub**: [https://github.com/na2mul](https://github.com/na2mul)
- 📧 **Email**: nazmulhasan.cse@outlook.com

---

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- Domain-Driven Design by Eric Evans
- ASP.NET Core community
- All contributors who have helped this project

---

<div align="center">

**⭐ If this project helped you, please consider giving it a star!**

Made with ❤️ using ASP.NET Core

[Report Bug](https://github.com/na2mul/aspnet-b11-nazmul/issues) · [Request Feature](https://github.com/na2mul/aspnet-b11-nazmul/issues)

</div>
