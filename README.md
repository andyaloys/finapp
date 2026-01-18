# FinApp - Finance Application

Aplikasi Finance Management dengan fitur Surat Tanda Pertanggungjawaban Belanja (STPB).

## 🏗️ Tech Stack

### Frontend
- **Angular 20** (Standalone Components)
- **NG ZORRO** (Ant Design UI)
- **TypeScript**
- **RxJS**
- **SCSS**

### Backend
- **.NET 8** Web API
- **Entity Framework Core** (Code-First)
- **MySQL 8.0**
- **JWT Authentication**
- **AutoMapper**
- **FluentValidation**
- **BCrypt** for password hashing

### DevOps
- **Docker & Docker Compose**
- **Nginx** (Frontend reverse proxy)

## 📁 Project Structure

```
finapp/
├── frontend/              # Angular 20 Application
├── backend/               # .NET 8 Clean Architecture
│   ├── FinApp.Domain/     # Entities & Domain Interfaces
│   ├── FinApp.Core/       # Business Logic & DTOs
│   ├── FinApp.Infrastructure/ # Data Access & EF Core
│   └── FinApp.API/        # Web API Controllers
├── database/              # MySQL initialization scripts
└── docker-compose.yml     # Docker orchestration
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 20+ & npm
- MySQL 8.0
- Docker & Docker Compose (optional)

### Development Setup

#### 1. Clone Repository
```bash
git clone https://github.com/YOUR_USERNAME/finapp.git
cd finapp
```

#### 2. Backend Setup
```bash
cd backend

# Restore packages
dotnet restore

# Update database connection string in appsettings.json
# ConnectionString: Server=localhost;Port=3306;Database=finapp;User=root;Password=P@ssw0rd;

# Apply migrations
dotnet ef database update --project FinApp.Infrastructure --startup-project FinApp.API

# Run backend
cd FinApp.API
dotnet run
```

Backend akan berjalan di: `http://localhost:5000`

#### 3. Frontend Setup
```bash
cd frontend

# Install dependencies
npm install

# Run development server
npm start
```

Frontend akan berjalan di: `http://localhost:4200`

### 🐳 Docker Setup

```bash
# Build and run all services (MySQL + Backend + Frontend)
docker-compose up --build

# Run in detached mode
docker-compose up -d

# Stop all services
docker-compose down
```

**Services:**
- Frontend: http://localhost:4200
- Backend API: http://localhost:5000
- MySQL: localhost:3306

## 📊 Database

### Connection String
```
Server=localhost;Port=3306;Database=finapp;User=root;Password=P@ssw0rd;
```

### Default Admin Account
- **Username:** admin
- **Password:** Admin123!

### Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project FinApp.Infrastructure --startup-project FinApp.API

# Update database
dotnet ef database update --project FinApp.Infrastructure --startup-project FinApp.API

# Remove last migration
dotnet ef migrations remove --project FinApp.Infrastructure --startup-project FinApp.API
```

## 🔐 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - Register new user

### STPB
- `GET /api/stpb` - Get all STPB (with pagination)
- `GET /api/stpb/{id}` - Get STPB by ID
- `POST /api/stpb` - Create new STPB
- `PUT /api/stpb/{id}` - Update STPB
- `DELETE /api/stpb/{id}` - Delete STPB

## 🛠️ Development

### Backend Commands
```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Publish for production
dotnet publish -c Release
```

### Frontend Commands
```bash
# Development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Linting
npm run lint
```

## 📝 Environment Variables

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=finapp;User=root;Password=P@ssw0rd;"
  },
  "Jwt": {
    "Secret": "YourSuperSecretKeyForJWTToken123456789",
    "Issuer": "FinAppAPI",
    "Audience": "FinAppClient",
    "ExpirationInMinutes": 60
  }
}
```

### Frontend (environment.ts)
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👨‍💻 Author

Your Name - [@yourhandle](https://github.com/yourhandle)

## 🙏 Acknowledgments

- Angular Team
- .NET Team
- NG ZORRO Team
