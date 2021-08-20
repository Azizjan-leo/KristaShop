using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Media.Admin.Admin.Models {
    public class DynamicPageLayout {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsDefault { get; set; }

        public DynamicPageLayout(string name, string title, bool isDefault) {
            Name = name;
            Title = title;
            IsDefault = isDefault;
        }

        private static Dictionary<string, DynamicPageLayout> _layouts;

        public static Dictionary<string, DynamicPageLayout> Layouts {
            get => _layouts ??= GetLayouts().ToDictionary(k => k.Name, v => v);
            protected set => _layouts = value;
        }

        public static List<DynamicPageLayout> GetLayouts() {
            return new() {
                new("_ContentContainerLayout", "С ограничением по ширине страницы", true),
                new("_ContentWideContainerLayout", "С ограничением по ширине страницы (широкий)", false),
                new("_NoContainerLayout", "Без ограничений по ширине страницы", false),
                new("_GridLayout", "Сетка", false),
                new("_GridTwoColsLayout", "Сетка в 2 столбца", false)
            };
        }

        public static string TryGetLayoutTitle(string name) {
            try {
                if (Layouts.ContainsKey(name)) return Layouts[name].Title;
            } catch (Exception) {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}