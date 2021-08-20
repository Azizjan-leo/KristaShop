using System;
using KristaShop.Common.Interfaces;
using KristaShop.DataAccess.Domain.Seeds.Common;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Domain.Seeds {
    public class SettingsSeed : ISeeds {
        public ModelBuilder Seed(ModelBuilder builder) {
            builder.Entity<Settings>().HasData(
                new Settings { Id = new Guid("afd36311-a384-4105-946d-e2d388ab072c"), Key = nameof(IAppSettings.KristaInstagram), Value = "https://www.instagram.com/krista.fashion/", Description = "Ссылка на оптовый аккаунт инстаграм", OnlyRootAccess = false},
                new Settings { Id = new Guid("7f1c2461-2d69-4af4-9f77-15991cc420bd"), Key = nameof(IAppSettings.KristaFacebook), Value = "https://www.facebook.com/kristafashion-101281188115170/", Description = "Ссылка на оптовый аккаунт фэйсбук", OnlyRootAccess = false },
                new Settings { Id = new Guid("e509c422-9e1c-4372-bfc6-a0641ab65a55"), Key = nameof(IAppSettings.KristaYoutube), Value = "https://www.youtube.com/channel/UCXftbG5dwIDgWGR_WKOj5CQ", Description = "Ссылка на оптовый аккаунт ютуб", OnlyRootAccess = false },
                new Settings { Id = new Guid("6a2467aa-13de-45bf-9772-8d1a53f76541"), Key = nameof(IAppSettings.KristaYoutubeSubscribe), Value = "https://www.youtube.com/channel/UCXftbG5dwIDgWGR_WKOj5CQ?sub_confirmation=1", Description = "Ссылка на подписку на оптовый аккаунт ютуб", OnlyRootAccess = false },
                new Settings { Id = new Guid("f7c7016c-2a60-4e51-b6c5-9db7e61e1aa0"), Key = nameof(IAppSettings.KristaVk), Value = "https://www.vk.com/", Description = "Ссылка на оптовый аккаунт в контакте", OnlyRootAccess = false },
                new Settings { Id = new Guid("d5f1181d-89e6-4e6a-900e-2d0b5017f4f4"), Key = nameof(IAppSettings.TermsOfUse), Value = "/Privacy/Index", Description = "Путь к политике конфиденциальности", OnlyRootAccess = false },
                new Settings { Id = new Guid("5e3da824-8a53-4028-af75-f270bec049d0"), Key = nameof(IAppSettings.DeliveryDetails), Value = "/Cooperation/Delivery", Description = "Путь данным о доставке", OnlyRootAccess = false },
                new Settings { Id = new Guid("adee4fd9-878d-45a6-aed7-2cff2df6b123"), Key = nameof(IAppSettings.PaymentDetails), Value = "/Cooperation/Payment", Description = "Путь к данным об оплате", OnlyRootAccess = false },
                new Settings { Id = new Guid("b1a20574-b8db-41d0-b007-d77efa9219ee"), Key = nameof(IAppSettings.FooterContacts), Value = "/Footer/Contacts", Description = "Путь к контактам в футере", OnlyRootAccess = false },
                new Settings { Id = new Guid("d7863668-5d04-490d-b357-4c4aba7eb6d5"), Key = nameof(IAppSettings.CategoriesDescription), Value = "/Category/Index", Description = "Путь к описанию на странице категориий", OnlyRootAccess = false },
                new Settings { Id = new Guid("f4538f14-00df-4616-a3f7-d3edcf622fb2"), Key = nameof(IAppSettings.OpenCatalogSearchDescription), Value = "/Search/OpenCatalog", Description = "Путь к дополнительному описанию открытого каталога при поиске", OnlyRootAccess = false },
                new Settings { Id = new Guid("dfc70e24-ad7d-4283-9ad1-e9580af64ada"), Key = nameof(IAppSettings.CartSuccess), Value = "Спасибо за покупку", Description = "Сообщение при успешном совершениии покупки", OnlyRootAccess = false },
                new Settings { Id = new Guid("eac3bc5a-63d7-47c0-9e1c-55eb2a5ec864"), Key = nameof(IAppSettings.IsRegistrationActive), Value = "True", Description = "Открытие регистрация. Значения: True - открыта. False - закрыта.", OnlyRootAccess = false },
                new Settings { Id = new Guid("ff028c22-ed10-4fc0-b48c-db3efdeb7ffa"), Key = nameof(IAppSettings.InactiveRegistrationMessage), Value = "В данный момент регистрация на сайте закрыта", Description = "Сообщение для пользователя, когда регистрация закрыта", OnlyRootAccess = false },
                new Settings { Id = new Guid("2909fb6c-fb4a-4954-9057-774aa8f4e922"), Key = nameof(IAppSettings.PartnershipRequestAcceptedToProcess), Value = "Ваша заявка принята в обработку. В ближайшее время наши менеджеры свяжутс с вами. Ожидайте пожалуйста", Description = "Сообщение для клиента, при отправке запроса на партнерство", OnlyRootAccess = false },
                new Settings { Id = new Guid("a2695933-31c4-4a63-8088-4e2383e80c5c"), Key = nameof(IAppSettings.PartnershipRequestRejected), Value = "В данный момент мы не нуждаемся в партнерах в вашем регионе (городе). Но мы приняли вашу заявку и в случае необходимости свяжемся с вами", Description = "Сообщение для клиента, если партнерство в его городе закрыто", OnlyRootAccess = false },
                new Settings { Id = new Guid("bc492ac9-f0e0-4b7a-94c1-f628df3da268"), Key = nameof(IAppSettings.PartnershipRequstActiveRequest), Value = "Вы уже подавали заявку. Пожалуйста, ожидайте связи с менеджером", Description = "Сообщение для клиента, если он уже подавал заявку на партнерство.", OnlyRootAccess = false },
                new Settings { Id = new Guid("0f24ac33-cec2-4d84-be9f-8e0209fd5244"), Key = nameof(IAppSettings.DefaultPartnerPaymentRate), Value = "15.0", Description = "Для партнера - сумма к выплате поставщику за единицу", OnlyRootAccess = false}
            );

            return builder;
        }
    }
}