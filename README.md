# WebApplication4 - Job Seekers API

Проект реалізує RESTful API для управління даними про шукачів роботи, їх резюме, освіту та досвід роботи.

## Архітектура

Проект побудований за принципом багатошарової архітектури:

- **DAL (Data Access Layer)** - робота з базою даних через репозиторії та Unit of Work
- **BLL (Business Logic Layer)** - бізнес-логіка, сервіси, валідація
- **Web/API** - REST API контролери, обробка HTTP запитів

## Технології

- .NET 10.0
- ASP.NET Core Web API
- PostgreSQL
- ADO.NET (Npgsql)
- Dapper
- AutoMapper
- FluentValidation
- OpenAPI/Swagger

## Вимоги

- .NET 10.0 SDK
- PostgreSQL 12+
- IDE (Visual Studio, Rider, VS Code)

## Кроки розгортання

### 1. Клонування репозиторію

```bash
git clone <repository-url>
cd WebApplication4
```

### 2. Налаштування бази даних

Створіть базу даних PostgreSQL:

```sql
CREATE DATABASE jobseekers;
```

Створіть таблиці:

```sql
-- Таблиця шукачів роботи
CREATE TABLE jobseekers (
    id SERIAL PRIMARY KEY,
    fullname VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    phone VARCHAR(50),
    experience INTEGER,
    skills TEXT
);

-- Таблиця освіти
CREATE TABLE education (
    id SERIAL PRIMARY KEY,
    jobseekerid INTEGER NOT NULL REFERENCES jobseekers(id) ON DELETE CASCADE,
    institution VARCHAR(255) NOT NULL,
    degree VARCHAR(255) NOT NULL,
    year INTEGER NOT NULL
);

-- Таблиця досвіду роботи
CREATE TABLE experiencerecord (
    id SERIAL PRIMARY KEY,
    jobseekerid INTEGER NOT NULL REFERENCES jobseekers(id) ON DELETE CASCADE,
    companyname VARCHAR(255) NOT NULL,
    position VARCHAR(255) NOT NULL,
    years INTEGER NOT NULL
);

-- Таблиця резюме
CREATE TABLE cv (
    id SERIAL PRIMARY KEY,
    jobseekerid INTEGER NOT NULL REFERENCES jobseekers(id) ON DELETE CASCADE,
    filelink VARCHAR(500) NOT NULL,
    description TEXT
);
```

### 3. Налаштування підключення до бази даних

Відредагуйте файл `WebApplication4/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=jobseekers;Username=postgres;Password=your_password"
  }
}
```

Замініть `your_password` на ваш пароль PostgreSQL.

### 4. Відновлення залежностей

```bash
dotnet restore
```

### 5. Запуск проекту

```bash
cd WebApplication4
dotnet run
```

Або через Visual Studio: натисніть F5 або виберіть "Debug" → "Start Debugging".

API буде доступний за адресою:
- HTTP: `http://localhost:5220`
- HTTPS: `https://localhost:7186`

## OpenAPI/Swagger

Після запуску проекту в режимі Development, документацію API можна переглянути:

- **Swagger UI**: `http://localhost:5220/swagger` - інтерактивна документація API
- **OpenAPI JSON**: `http://localhost:5220/swagger/v1/swagger.json` - JSON специфікація API

Swagger UI дозволяє:
- Переглядати всі доступні endpoints
- Тестувати API безпосередньо в браузері
- Переглядати схеми моделей даних
- Бачити приклади запитів та відповідей

## Приклади перевірки API

### 1. Отримати всіх шукачів роботи

```bash
GET http://localhost:5220/api/jobseekers
```

### 2. Створити нового шукача роботи

```bash
POST http://localhost:5220/api/jobseekers
Content-Type: application/json

{
  "fullName": "Іван Петренко",
  "email": "ivan.petrenko@example.com",
  "phone": "+380501234567",
  "experience": 5,
  "skills": "C#, .NET, SQL, PostgreSQL"
}
```

Очікуваний результат: HTTP 201 Created з Location заголовком

### 3. Отримати шукача роботи за ID

```bash
GET http://localhost:5220/api/jobseekers/1
```

Очікуваний результат: HTTP 200 OK з даними шукача

### 4. Оновити шукача роботи

```bash
PUT http://localhost:5220/api/jobseekers/1
Content-Type: application/json

{
  "fullName": "Іван Петренко",
  "email": "ivan.petrenko@example.com",
  "phone": "+380501234567",
  "experience": 6,
  "skills": "C#, .NET, SQL, PostgreSQL, Docker"
}
```

Очікуваний результат: HTTP 200 OK з оновленими даними

### 5. Пошук шукачів за ім'ям або навичками

```bash
GET http://localhost:5220/api/jobseekers/search?query=C#
```

Очікуваний результат: HTTP 200 OK зі списком знайдених шукачів

### 6. Видалити шукача роботи

```bash
DELETE http://localhost:5220/api/jobseekers/1
```

Очікуваний результат: HTTP 204 No Content

### 7. Створити резюме для шукача

```bash
POST http://localhost:5220/api/cvs
Content-Type: application/json

{
  "jobSeekerId": 1,
  "summary": "https://example.com/cv/ivan-petrenko.pdf",
  "additionalInfo": "Старший розробник з 5+ роками досвіду"
}
```

Очікуваний результат: HTTP 201 Created

### 8. Отримати всі резюме шукача

```bash
GET http://localhost:5220/api/cvs/jobseeker/1
```

Очікуваний результат: HTTP 200 OK зі списком резюме

### 9. Створити освіту

```bash
POST http://localhost:5220/api/educations
Content-Type: application/json

{
  "jobSeekerId": 1,
  "institution": "Київський національний університет",
  "degree": "Магістр комп'ютерних наук",
  "year": 2018
}
```

Очікуваний результат: HTTP 201 Created

### 10. Створити досвід роботи

```bash
POST http://localhost:5220/api/experiences
Content-Type: application/json

{
  "jobSeekerId": 1,
  "company": "Tech Company",
  "position": "Senior Software Developer",
  "years": 3
}
```

Очікуваний результат: HTTP 201 Created

## Обробка помилок

API повертає стандартизовані помилки у форматі ProblemDetails:

- **400 Bad Request** - помилка валідації (ValidationException)
- **404 Not Found** - ресурс не знайдено (NotFoundException)
- **409 Conflict** - бізнес-конфлікт (BusinessConflictException)
- **422 Unprocessable Entity** - помилка валідації FluentValidation
- **500 Internal Server Error** - внутрішня помилка сервера

Приклад відповіді з помилкою:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "detail": "Validation failed",
  "instance": "/api/jobseekers",
  "errors": {
    "email": ["Email is required"]
  }
}
```

## Структура проекту

```
WebApplication4/
├── Dal/                    # Data Access Layer
│   ├── Database/          # Контекст підключення до БД
│   ├── Dto/               # DTO для DAL
│   ├── Entities/          # Сутності бази даних
│   ├── Interfaces/        # Інтерфейси репозиторіїв
│   ├── Repositories/      # Реалізації репозиторіїв
│   └── UoW/              # Unit of Work
├── Bll/                   # Business Logic Layer
│   ├── Dto/              # DTO для BLL
│   ├── Exceptions/       # Доменні винятки
│   ├── Interfaces/       # Інтерфейси сервісів
│   ├── Mapping/          # AutoMapper профілі
│   ├── Services/         # Реалізації сервісів
│   └── Validators/       # FluentValidation валідатори
└── WebApplication4/      # Web API
    ├── Controllers/      # API контролери
    ├── Middleware/       # Middleware (обробка помилок)
    └── Program.cs        # Точка входу
```

## Особливості реалізації

### DAL (Data Access Layer)

- **JobSeekerRepositoryAdo** - реалізація на чистому ADO.NET (NpgsqlCommand)
- **CvRepository, EducationRepository, ExperienceRecordRepository** - реалізації на Dapper
- Всі SQL запити параметризовані для захисту від SQL-ін'єкцій
- Unit of Work забезпечує транзакційність операцій
- Підтримка CancellationToken для асинхронних операцій

### BLL (Business Logic Layer)

- Сервіси інкапсулюють бізнес-логіку
- AutoMapper для мапінгу між DTO та сутностями
- FluentValidation для валідації даних
- Доменні винятки для обробки бізнес-помилок

### API

- Thin контролери - тільки виклики сервісів
- Атрибутна маршрутизація
- Асинхронні методи
- Коректні HTTP статус коди
- ProblemDetails для уніфікованих помилок
- OpenAPI документація

## Тестування

Для тестування API можна використовувати:

- **Postman** - імпорт OpenAPI специфікації
- **Swagger UI** - інтерактивна документація (після додавання Swashbuckle)
- **curl** - командний рядок
- **HttpClient** - .NET клієнт

## Ліцензія

Цей проект створений в навчальних цілях.

