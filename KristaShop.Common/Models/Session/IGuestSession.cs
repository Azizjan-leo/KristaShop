using System;

namespace KristaShop.Common.Models.Session {
    public interface IGuestSession : IBaseSession  {
        public Guid Id { get; set; }
        public GuestAccessInfo GuestAccessIngo { get; set; }
    }
}