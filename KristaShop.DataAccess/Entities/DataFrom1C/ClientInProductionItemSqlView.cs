using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    public class ClientInProductionItemSqlView {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorValue { get; set; }
        public string SizeValue { get; set; }
        public int PartsCount { get; set; }
        public int InStageKroy { get; set; }
        public int InStageKroyDone { get; set; }
        public int InStageZapusk { get; set; }
        public int InStageVposhive { get; set; }
        public int InStageSkladGP { get; set; }
    }
}
