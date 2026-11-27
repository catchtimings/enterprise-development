# Система доставки еды (Food Delivery System)

Учебный проект по дисциплине «Разработка корпоративных систем» на платформе .NET 10.

## Описание проекта
Проект представляет собой предметную область **сервиса доставки еды**. Система позволяет управлять информацией о клиентах, ресторанах, блюдах и заказах, а также выполнять аналитические выборки.

Система предоставляет REST API для управления объектами предметной области (рестораны, клиенты, заказы), поддерживает базовые CRUD-операции и предоставляет сложную аналитику по продажам, клиентам и категориям блюд.

На текущем этапе проект использует реляционную базу данных **MySQL** (через EF Core), брокер сообщений **RabbitMQ** для асинхронной генерации заказов и разворачивается с помощью оркестратора **.NET Aspire**. Архитектура поддерживает переключение между хранением в памяти и в базе данных.

---

## Технологический стек
- Платформа: .NET 10.0
- Веб-фреймворк: ASP.NET Core Web API
- Документация API: OpenAPI (`Microsoft.AspNetCore.OpenApi`) с UI на базе Scalar
- ORM: Entity Framework Core (Pomelo MySQL Provider)
- База данных: MySQL (в Docker-контейнере)
- Брокер сообщений: RabbitMQ (Management Plugin)
- Генерация данных: Bogus
- Оркестрация: .NET Aspire (AppHost, ServiceDefaults)
- Фреймворк тестирования: xUnit v3
- Continuous Integration: GitHub Actions

## Структура проекта
```
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
│   └── Services/                            # Сервисы бизнес-логики
│
├── FoodDelivery.Infrastructure.InMemory/    # In-Memory инфраструктура
│   └── Repositories/                        # Реализации репозиториев поверх DataSeeder
│
├── FoodDelivery.Infrastructure.EFCore/      # Инфраструктура EF Core
│   ├── Migrations/                          # Миграции базы данных
│   ├── Repositories/                        # Реализации репозиториев для EF Core
│   ├── FoodDeliveryDbContext.cs             # Контекст базы данных и конфигурация сущностей
│   └── FoodDeliveryDbContextFactory.cs      # Фабрика для design-time (миграции)
│
├── FoodDelivery.Infrastructure.RabbitMq/    # Консьюмер RabbitMQ
│   └── RabbitMqOrderConsumer.cs             # BackgroundService для приёма заказов из очереди
│
├── FoodDelivery.Api/                        # REST API
│   ├── Controllers/                         # Контроллеры
│   └── Program.cs                           # Конфигурация DI, OpenAPI, Scalar, EF Core, RabbitMQ Consumer
│
├── FoodDelivery.Generator/                  # Логика генератора
│   ├── Application/                         # Служба генерации заказов
│   ├── Domain/                              # BogusOrderGenerator
│   ├── Infrastructure/                      # IProducerService
│   └── Presentation/                        # OrdersGeneratorController
│
├── FoodDelivery.Generator.Rabbitmq/         # Хост продюсера RabbitMQ
│   ├── Infrastructure/                      # RabbitMqProducer
│   ├── Program.cs                           # Регистрация генератора и RabbitMQ
│   └── appsettings.json                     # Настройки RabbitMQ и параметров генерации
│
├── FoodDelivery.ServiceDefaults/            # Общие настройки Aspire
│
├── FoodDelivery.AppHost/                    # Оркестратор Aspire (запуск БД, RabbitMQ, API и генератора)
│   └── AppHost.cs                           # Конфигурация ресурсов (MySQL, RabbitMQ, API, Generator)
│
├── FoodDelivery.Tests/                      # Unit-тесты
│   ├── AnalyticsTests.cs                    # Тесты бизнес-логики и аналитики
│   └── DatabaseFixture.cs                   # xUnit fixture для изоляции данных
│
└── FoodDeliverySystem.slnx                  # Solution файл
```


---

## Архитектура и поток данных

Проект реализует сервисно-ориентированную архитектуру с использованием брокера сообщений для асинхронного взаимодействия между сервисом-генератором и сервером.

```
[Generator] --(RabbitMQ topic exchange)--> [API Consumer] --> [MySQL]
```

- **Генератор** (FoodDelivery.Generator.Rabbitmq) - отдельное приложение, которое генерирует контракты заказов с помощью Bogus и отправляет их в RabbitMQ.
- **API** (FoodDelivery.Api) - принимает контракты из очереди через фоновый консьюмер, валидирует их и сохраняет в БД.
- **Консьюмер** (FoodDelivery.Infrastructure.RabbitMq) - библиотека с реализацией BackgroundService, которая хостится внутри API.
- **Валидация** - невалидные заказы (с несуществующими ClientId, RestaurantId, DishId) мягко отсеиваются на принимающей стороне, не роняя консьюмер.

## Запуск и тестирование

### Предварительные требования
- .NET 10.0
- Docker (для запуска MySQL и RabbitMQ через Aspire)

### Клонирование и запуск через Aspire (Рекомендуемый способ)

```
git clone https://github.com/catchtimings/enterprise-development.git
cd enterprise-development
dotnet run --project FoodDelivery.AppHost
```

После запуска Aspire Dashboard откроется в браузере. Все четыре ресурса (MySQL, rabbitmq, api, generator-rabbitmq) должны перейти в статус Running.

### Проверка работы генератора и шины
1. Автоматический режим: При запуске генератор сам отправит батч заказов в RabbitMQ. В логах generator-rabbitmq появятся сообщения об отправке, а в логах api - сообщения о приёме и сохранении (например: "Принят батч из 5 заказов, сохранено 1, отсеяно 4");
2. Ручной режим: Открыть URL генератора из дашборда Aspire (например, `http://localhost:xxxx`) и добавь путь: `/api/generator?batchSize=5&payloadLimit=20&waitTime=2`. Это запустит генерацию и отправку 20 контрактов;
3. Проверка API: Открыть Scalar и отправить запрос `GET /api/orders`. Количество заказов должно увеличиться;
4. Проверка аналитики: В Scalar отправить запрос `GET /api/analytics/category-summary?startDate=<сегодня>&endDate=<сегодня>`. Запрос должен вернуть данные по категориям блюд за текущий день;
5. Проверка RabbitMQ: В интерфейсе RabbitMQ на вкладке **Queues and Streams** можно увидеть очередь `fooddelivery-orders`.

## Тестирование

```
dotnet test
```

Unit-тесты проверяют бизнес-логику аналитики (топ-5 ресторанов, минимальное время доставки, клиенты ресторана, сводка по категориям, топ трат).