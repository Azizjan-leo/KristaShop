using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Module.Catalogs.Business.Models
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public int CategoryId1C { get; set; }
        public string Category1CName { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
        public bool IsVisible { get; set; }

        public void AssignCategory1CName(IEnumerable<Category1CDTO> allCategories) {

            var cetegory1C = allCategories.Where<Category1CDTO>(i => i.Id == CategoryId1C).FirstOrDefault();
            if (cetegory1C == null) {
                Category1CName = "---";
            } else {
                Category1CName = cetegory1C.Name;
            }
        }
    }
}