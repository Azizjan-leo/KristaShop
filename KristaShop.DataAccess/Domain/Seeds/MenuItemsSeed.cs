using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using KristaShop.DataAccess.Domain.Seeds.Common;

namespace KristaShop.DataAccess.Domain.Seeds
{
    internal class MenuItemsSeed : ISeeds {
        public ModelBuilder Seed(ModelBuilder builder) {
            builder.Entity<MenuItem>()
                .HasData(
                    new MenuItem {
                        Id = new Guid("56fba3aa-25d8-4de6-b166-5da83192be99"),
                        Title = "Главная страница",
                        AreaName = "Admin",
                        ControllerName = "Home",
                        ActionName = "Index",
                        Icon = "krista-home",
                        MenuType = MenuType.Left,
                        Order = 1
                    },
                    new MenuItem {
                        Id = new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b"),
                        Title = "Клиенты",
                        AreaName = "Admin",
                        ControllerName = "Identity",
                        ActionName = "Index",
                        Icon = "krista-user",
                        BadgeTarget = "clientActionsTotal",
                        MenuType = MenuType.Left,
                        Order = 10
                    },
                    new MenuItem {
                        Id = new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"),
                        Title = "Отчеты",
                        AreaName = "Admin",
                        ControllerName = "Cart",
                        ActionName = "UserCartsReport",
                        Icon = "krista-doc",
                        MenuType = MenuType.Left,
                        Order = 15
                    },
                    new MenuItem {
                        Id = new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"),
                        Title = "Партнеры",
                        AreaName = "Admin",
                        ControllerName = "Partnership",
                        ActionName = "Index",
                        Icon = "krista-crown",
                        BadgeTarget = "requestsTotal",
                        MenuType = MenuType.Left,
                        Order = 17
                    },
                    new MenuItem {
                        Id = new Guid("cedfe1cf-4c35-4ed0-a576-f05ab5e01414"),
                        Title = "Связь с клиентами",
                        AreaName = "Admin",
                        ControllerName = "Feedback",
                        ActionName = "Index",
                        Icon = "krista-chat",
                        BadgeTarget = "feedbackTotal",
                        MenuType = MenuType.Left,
                        Order = 20
                    },
                    new MenuItem {
                        Id = new Guid("c66fe088-7df4-4739-bedb-147b843cb834"),
                        Title = "Мультимедия",
                        AreaName = "Admin",
                        ControllerName = "MBody",
                        ActionName = "Index",
                        Icon = "krista-media",
                        MenuType = MenuType.Left,
                        Order = 30
                    },
                    new MenuItem {
                        Id = new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"),
                        Title = "Работа с моделями",
                        AreaName = "Admin",
                        ControllerName = "Catalog",
                        ActionName = "Index",
                        Icon = "krista-hanger",
                        MenuType = MenuType.Left,
                        Order = 40
                    },
                    new MenuItem {
                        Id = new Guid("8f1ff3c9-8f48-499d-ac33-240a3216f721"),
                        Title = "Воронка",
                        AreaName = "Admin",
                        ControllerName = "Faq",
                        ActionName = "Index",
                        Icon = "krista-info",
                        MenuType = MenuType.Left,
                        Order = 70
                    },
                    new MenuItem {
                        Id = new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"),
                        Title = "Настройки системы",
                        AreaName = "Admin",
                        ControllerName = "Settings",
                        ActionName = "Index",
                        Icon = "krista-settings",
                        MenuType = MenuType.Left,
                        Order = 80
                    },
                    new MenuItem {
                        Id = new Guid("2f7c02f6-3b20-4c61-a371-946d9a2b5764"),
                        Title = "Персонал",
                        AreaName = "Admin",
                        ControllerName = "Staff",
                        ActionName = "Index",
                        Icon = "krista-staff",
                        MenuType = MenuType.Left,
                        Order = 75
                    }
                );
            #region Orders
            builder.Entity<MenuItem>()
               .HasData(
                   new MenuItem {
                       Id = new Guid("b185169c-2738-4664-b5ce-0c8a98f0f227"),
                       Title = "Сводная заявка",
                       AreaName = "Admin",
                       ControllerName = "Orders",
                       ActionName = "ConsolidatedRequest",
                       Icon = "",
                       BadgeTarget = "",
                       MenuType = MenuType.Top,
                       Order = 26,
                       ParentId = new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b")
                   });
            #endregion

                    #region Partners

       builder.Entity<MenuItem>()
                .HasData(
                    new MenuItem {
                        Id = new Guid("2B102B13-986E-422A-A058-C2DD4EE4D713"),
                        Title = "Партнеры",
                        AreaName = "Admin",
                        ControllerName = "Partnership",
                        ActionName = "Index",
                        Icon = "",
                        BadgeTarget = "",
                        MenuType = MenuType.Top,
                        Order = 1,
                        ParentId = new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69")
                    },
                    new MenuItem {
                        Id = new Guid("68D7F82C-8A8D-4650-A47B-7725C5E56D86"),
                        Title = "История взаиморасчетов",
                        AreaName = "Admin",
                        ControllerName = "Partnership",
                        ActionName = "PaymentsHistory",
                        Icon = "",
                        BadgeTarget = "",
                        MenuType = MenuType.Top,
                        Order = 10,
                        ParentId = new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69")
                    },
                    new MenuItem {
                        Id = new Guid("326c52ed-1831-49ab-8a9b-c449f7bcfaba"),
                        Title = "Отчет по продажам",
                        AreaName = "Admin",
                        ControllerName = "Partnership",
                        ActionName = "SalesReport",
                        Icon = "",
                        BadgeTarget = "",
                        MenuType = MenuType.Top,
                        Order = 20,
                        ParentId = new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69")
                    });
            #endregion

            #region Clients

            builder.Entity<MenuItem>()
                .HasData(new MenuItem {
                        Id = new Guid("aefe9999-19b7-457c-babc-c96eee8d3dc0"),
                        Title = "Клиенты",
                        AreaName = "Admin",
                        ControllerName = "Identity",
                        ActionName = "Index",
                        Icon = "",
                        BadgeTarget = "newUsers",
                        MenuType = MenuType.Top,
                        Order = 1,
                        ParentId = new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b")
                    });
            #endregion

            #region Staff
            builder.Entity<MenuItem>()
                .HasData(new MenuItem {
                    Id = new Guid("647c76d8-9d6a-4167-928f-fdcc0e3e3ab2"),
                    Title = "Менеджеры",
                    AreaName = "Admin",
                    ControllerName = "Staff",
                    ActionName = "Index",
                    Icon = "",
                    BadgeTarget = "",
                    MenuType = MenuType.Top,
                    Order = 1,
                    ParentId = new Guid("2f7c02f6-3b20-4c61-a371-946d9a2b5764")
                });
            #endregion

            #region Feedback

            builder.Entity<MenuItem>()
                .HasData(new MenuItem {
                    Id = new Guid("df4f2c8a-8414-49c2-a4a3-31454b55936c"),
                    Title = "Сообщения",
                    AreaName = "Admin",
                    ControllerName = "Feedback",
                    ActionName = "Index",
                    Icon = "",
                    BadgeTarget = "newFeedback",
                    MenuType = MenuType.Top,
                    Order = 1,
                    ParentId = new Guid("cedfe1cf-4c35-4ed0-a576-f05ab5e01414")
                });

            #endregion

            #region Multimedia

            builder.Entity<MenuItem>()
                .HasData(
                    new MenuItem {
                        Id = new Guid("0f0bd978-3a1a-4fcd-b710-70b0b90e0ff7"),
                        Title = "Контент страниц",
                        AreaName = "Admin",
                        ControllerName = "MBody",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 1,
                        ParentId = new Guid("c66fe088-7df4-4739-bedb-147b843cb834")
                    },
                    new MenuItem {
                        Id = new Guid("ffc7d8ff-b78f-4c99-9bdd-8084d1a7c098"),
                        Title = "Блог",
                        AreaName = "Admin",
                        ControllerName = "Blog",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 10,
                        ParentId = new Guid("c66fe088-7df4-4739-bedb-147b843cb834")
                    },
                    new MenuItem {
                        Id = new Guid("b525d487-7d6a-4b1c-8776-380a6b695462"),
                        Title = "Галерея",
                        AreaName = "Admin",
                        ControllerName = "Gallery",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 20,
                        ParentId = new Guid("c66fe088-7df4-4739-bedb-147b843cb834")
                    },
                    new MenuItem {
                        Id = new Guid("9521365b-2f09-4138-b5b5-fe15d1dc2b70"),
                        Title = "Баннер",
                        AreaName = "Admin",
                        ControllerName = "Banner",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 30,
                        ParentId = new Guid("c66fe088-7df4-4739-bedb-147b843cb834")
                    },
                    new MenuItem {
                        Id = new Guid("76f02dd3-31bf-4910-ba35-8ffdeb4398a7"),
                        Title = "Видеогалерея",
                        AreaName = "Admin",
                        ControllerName = "VideoGallery",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 40,
                        ParentId = new Guid("c66fe088-7df4-4739-bedb-147b843cb834")
                    });

            #endregion

            #region Models

            builder.Entity<MenuItem>()
                .HasData(new MenuItem {
                        Id = new Guid("9898ef9a-8e22-4fb4-ac98-a20911ffa5f3"),
                        Title = "Каталоги",
                        AreaName = "Admin",
                        ControllerName = "Catalog",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 1,
                        ParentId = new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79")
                    },
                    new MenuItem {
                        Id = new Guid("c9ef8cb8-ff17-4b9d-aa51-4da8c7e3d055"),
                        Title = "Категории",
                        AreaName = "Admin",
                        ControllerName = "Category",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 10,
                        ParentId = new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79")
                    },
                    new MenuItem {
                        Id = new Guid("cc811614-fdf0-4d4a-89c8-e23f9be12dc7"),
                        Title = "Модели",
                        AreaName = "Admin",
                        ControllerName = "ModelsCatalog",
                        ActionName = "Index",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 20,
                        ParentId = new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79")
                    },
                    new MenuItem {
                        Id = new Guid("71c09087-a52b-4506-86ec-2677bd824ebf"),
                        Title = "Все модели",
                        AreaName = "Admin",
                        ControllerName = "ModelsCatalog",
                        ActionName = "History",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 25,
                        ParentId = new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79")
                    });

            #endregion

            #region Reports

            builder.Entity<MenuItem>()
                .HasData(
                    new MenuItem {
                        Id = new Guid("07f0cd59-50a0-405b-9d3a-9a7134da7ef4"),
                        Title = "Отчет по корзинам клиентов",
                        AreaName = "Admin",
                        ControllerName = "Cart",
                        ActionName = "UserCartsReport",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 1,
                        ParentId = new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c")
                    },
                    new MenuItem {
                        Id = new Guid("db0581b2-bdcd-400a-96ed-8fbfed3bcef4"),
                        Title = "Сводный отчет по необработанным заказам",
                        AreaName = "Admin",
                        ControllerName = "OrderReports",
                        ActionName = "UserUnprocessedOrdersReport",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 10,
                        ParentId = new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c")
                    },
                    new MenuItem {
                        Id = new Guid("b7b8bc14-43ce-40d6-b728-cedaa80386b1"),
                        Title = "Отчет по городам",
                        AreaName = "Admin",
                        ControllerName = "OrderReports",
                        ActionName = "CitiesOrdersReport",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 15,
                        ParentId = new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c")
                    },
                    new MenuItem {
                        Id = new Guid("15aa05a9-6c1c-4a04-b403-90f1ad7a4a66"),
                        Title = "Сводный отчет по заказам",
                        AreaName = "Admin",
                        ControllerName = "OrderReports",
                        ActionName = "TotalOrdersReport",
                        Icon = "",
                        MenuType = MenuType.Top,
                        Order = 20,
                        ParentId = new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c")
                    });

            #endregion

            #region Settings top items

            builder.Entity<MenuItem>()
                .HasData(new MenuItem {
                    Id = new Guid("f79ed663-09f4-42f8-9add-ac8551df6124"),
                    Title = "Настройки",
                    AreaName = "Admin",
                    ControllerName = "Settings",
                    ActionName = "Index",
                    Icon = "",
                    MenuType = MenuType.Top,
                    Order = 1,
                    ParentId = new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24")
                },
                new MenuItem {
                    Id = new Guid("f8949e4a-a06c-4955-94a5-c833968ea2a2"),
                    Title = "Пункты меню",
                    AreaName = "Admin",
                    ControllerName = "Menu",
                    ActionName = "Index",
                    Icon = "",
                    MenuType = MenuType.Top,
                    Order = 10,
                    ParentId = new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24")
                },
                new MenuItem {
                    Id = new Guid("6f8df031-accf-4c6d-90d0-52ebb21c6bff"),
                    Title = "Права доступов",
                    AreaName = "Admin",
                    ControllerName = "Access",
                    ActionName = "Index",
                    Icon = "",
                    MenuType = MenuType.Top,
                    Order = 20,
                    ParentId = new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24")
                },
                new MenuItem {
                    Id = new Guid("4b8dec48-c770-4c56-8bb0-5c0409d4abef"),
                    Title = "Импорт",
                    AreaName = "Admin",
                    ControllerName = "Import",
                    ActionName = "Execute",
                    Icon = "",
                    MenuType = MenuType.Top,
                    Order = 30,
                    ParentId = new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24")
                });

            #endregion

            return builder;
        }
    }
}