namespace Blinds.Data.Models
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Contracts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    public class User : IdentityUser, IDeletableEntity
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
