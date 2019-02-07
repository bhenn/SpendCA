using Microsoft.AspNetCore.Identity;

namespace SpendCA.Infrastructure.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
