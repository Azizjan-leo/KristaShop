using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Module.Common.Business.Models.Adapters;

namespace Module.Order.Business.Models.Adapters {
    public class ManufactureAdapter : IEntityToDtoAdapter<IEnumerable<ClientManufacturingItemSqlView>, IEnumerable<ManufacturingItemDTO>> {
        public IEnumerable<ManufacturingItemDTO> Convert(IEnumerable<ClientManufacturingItemSqlView> source) {
            var result = new List<ManufacturingItemDTO>();
            foreach (var item in source) {
                if (item.InKroyAmount > 0) {
                    result.Add(_toManufacturingItemDTO(item, ManufactureState.Kroy, item.InKroyAmount));
                }

                if (item.InKroyGotovAmount > 0) {
                    result.Add(_toManufacturingItemDTO(item, ManufactureState.KroyComplete, item.InKroyGotovAmount));
                }

                if (item.InZapuskAmount > 0) {
                    result.Add(_toManufacturingItemDTO(item, ManufactureState.Zapusk, item.InZapuskAmount));
                }

                if (item.InVPoshiveAmount > 0) {
                    result.Add(_toManufacturingItemDTO(item, ManufactureState.VPoshive, item.InVPoshiveAmount));
                }

                if (item.InSkladGPAmount > 0) {
                    result.Add(_toManufacturingItemDTO(item, ManufactureState.SkladGP, item.InSkladGPAmount));
                }
            }

            return result;
        }

        private static ManufacturingItemDTO _toManufacturingItemDTO(ClientManufacturingItemSqlView itemSrc, ManufactureState state, int amount) {
            return new() {
                Id = itemSrc.Id,
                State = state,
                Amount = amount,
                Articul = itemSrc.Articul,
                MainPhoto = itemSrc.MainPhoto,
                ModelId = itemSrc.ModelId,
                ColorId = itemSrc.ColorId,
                ColorName = itemSrc.ColorName,
                ColorValue = itemSrc.ColorValue,
                ColorPhoto = itemSrc.ColorPhoto,
                Size = itemSrc.Size,
                Price = itemSrc.Price,
                PriceInRub = itemSrc.PriceInRub,
                CollectionId = itemSrc.CollectionId,
                CollectionName = itemSrc.CollectionName,
                CollectionPrepayPercent = itemSrc.CollectionPrepayPercent,
                PrepayPercent = itemSrc.PrepayPercent
            };
        }
    }
}