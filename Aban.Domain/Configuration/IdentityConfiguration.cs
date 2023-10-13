using Aban.Domain.Enumerations;

namespace Aban.Domain.Configuration
{
    public class IdentityConfiguration
    {
        public string? LoginPath { get; set; }

        public string? AfterLoginRedirect { get; set; }

        public string? RegisterPath { get; set; }

        public string? AfterRegisterRedirect { get; set; }

        public string? LogOutPath { get; set; }

        public string? AfterLogOutPath { get; set; }

        public string? CookieName { get; set; }

        public string? AccessDeniedPath { get; set; }

        public int RequiredUniqueChars { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public int RequiredLength { get; set; }

        public string? AllowedUserNameCharacters { get; set; }
        public bool RequireUniqueEmail { get; set; }

        public bool SlidingExpiration { get; set; }

        public Enumeration.ExpireTimeType DefaultLockoutTimeSpanType { get; set; }

        public int ExpiredefaultLockoutTimeSpan { get; set; }


        public Enumeration.ExpireTimeType ExpireTimeType { get; set; }

        public int ExpireTimeSpan { get; set; }

        public string? OptionalParamiters { get; set; }

        public bool RequireConfirmedAccount { get; set; }
        public bool RequireConfirmedEmail { get; set; }
        public bool RequireConfirmedPhoneNumber { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool isPersistent { get; set; }
    }
}
