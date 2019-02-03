using System;
using Microsoft.AspNetCore.Identity;

namespace SpendCA.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

    }
}