using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.App.Admin.Admin.Models {
    public class PromoLinkViewModel {
        public Guid Id { get; set; }

        [DisplayName("Ссылка")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Заполните поле {0}")]
        public string Link { get; set; }

        public int ManagerId { get; set; }

        [DisplayName("Тип")]
        public OrderFormType OrderForm { get; set; }
        public SelectList OrderForms { get; set; }

        [DisplayName("Дата создания")]
        public DateTimeOffset CreateDate { get; set; }

        [DisplayName("Действительна до")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public DateTimeOffset DeactivateTime { get; set; }

        [DisplayName("Заголовок")]
        [Required(ErrorMessage = "Заполните поле {0}")]
        public string Title { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Ссылка на видео")]
        public string VideoPath { get; set; }

        [DisplayName("Превью видео")]
        public string VideoPreviewPath { get; set; }

        [DisplayName("Превью видео")]
        public IFormFile VideoPreview { get; set; }

        public string OrderFormName => OrderForm.ToReadableString();
        public string DeactivateTimeFormatted => DeactivateTime.ToOffset(DateTimeOffset.Now.Offset).ToFormattedString();
    }
}
