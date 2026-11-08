# Система доставки еды (Food Delivery System)

Учебный проект по дисциплине «Разработка корпоративных систем» на платформе .NET 10.

## Описание проекта
Проект представляет собой предметную область **сервиса доставки еды**. Система позволяет управлять информацией о клиентах, ресторанах, блюдах и заказах, а также выполнять аналитические выборки.

Система предоставляет REST API для управления объектами предметной области (рестораны, клиенты, заказы), поддерживает базовые CRUD-операции и предоставляет сложную аналитику по продажам, клиентам и категориям блюд.

На текущем этапе проект использует реляционную базу данных **MySQL** (через EF Core) и разворачивается с помощью оркестратора **.NET Aspire**. Архитектура поддерживает переключение между хранением в памяти и в базе данных.

---

## Технологический стек
- Платформа: .NET 10.0
- Веб-фреймворк: ASP.NET Core Web API
- Документация API: OpenAPI (`Microsoft.AspNetCore.OpenApi`) с UI на базе Scalar
- ORM: Entity Framework Core (Pomelo MySQL Provider)
- База данных: MySQL (в Docker-контейнере)
- Оркестрация: .NET Aspire (AppHost, ServiceDefaults)
- Фреймворк тестирования: xUnit v3
- Continuous Integration: GitHub Actions

---

## Структура проекта

```text
enterprise-development/
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml                    # CI/CD пайплайн GitHub Actions
│
├── FoodDelivery.Domain/                     # Домен
│   ├── Entities/                            # Модели предметной области (Client, Restaurant, Order и др.)
│   └── DataSeeder.cs                        # Генератор начального датасета
│
├── FoodDelivery.Contracts/                  # DTO
│   └── Dtos/                                # Объекты передачи данных (разложены по папкам)
│       ├── Analytics/
│       ├── Clients/
│       ├── Dishes/
│       ├── Orders/
│       └── Restaurants/
│
├── FoodDelivery.Application/                # Бизнес-логика
│   ├── Interfaces/
│   │   ├── Repositories/                    # Интерфейсы репозиториев
│   │   └── Services/                        # Интерфейсы сервисов
│   ├── Mappers/                             # Ручные мапперы (Domain <-> DTO)
│   └── Services/                            # Сервисы бизнес-логики (Client, Order, Analytics)
│
├── FoodDelivery.Infrastructure.InMemory/    # In-Memory инфраструктура (альтернативная реализация)
│   └── Repositories/                        # Реализации репозиториев поверх DataSeeder
│
├── FoodDelivery.Infrastructure.EFCore/      # Инфраструктура EF Core
│   ├── Migrations/                          # Миграции базы данных
│   ├── Repositories/                        # Реализации репозиториев для EF Core
│   ├── FoodDeliveryDbContext.cs             # Контекст базы данных и конфигурация сущностей
│   └── FoodDeliveryDbContextFactory.cs      # Фабрика для design-time (миграции)
│
├── FoodDelivery.Api/                        # REST API
│   ├── Controllers/                         # Контроллеры
│   └── Program.cs                           # Конфигурация DI, OpenAPI, Scalar, EF Core
│
├── FoodDelivery.ServiceDefaults/            # Общие настройки Aspire (телеметрия, health checks)
│
├── FoodDelivery.AppHost/                    # Оркестратор Aspire (запуск БД и API)
│   └── Program.cs                           # Конфигурация ресурсов (MySQL, API)
│
├── FoodDelivery.Tests/                      # Unit-тесты
│   ├── AnalyticsTests.cs                    # Тесты бизнес-логики и аналитики
│   └── DatabaseFixture.cs                   # xUnit fixture для изоляции данных
│
└── FoodDeliverySystem.slnx                  # Solution файл
```

## Запуск и тестирование

### Предварительные требования
- .NET 10.0 SDK
- Docker Desktop (для запуска MySQL через Aspire)

### Клонирование и запуск через Aspire (Рекомендуемый способ)

```bash
# Клонирование репозитория
git clone https://github.com/catchtimings/enterprise-development.git

# Переход в директорию
cd enterprise-development

# Запуск оркестратора Aspire (поднимет контейнер MySQL и API)
dotnet run --project FoodDelivery.AppHost