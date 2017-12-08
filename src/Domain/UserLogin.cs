using System;

namespace Preenactos.Domain
{
    public class UserLogin
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ProviderDisplay { get; set; }

        public Guid UserId { get; set; }

        #region Navigation

        public virtual User User { get; set; }

        #endregion Navigation
    }
}