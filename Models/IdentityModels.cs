using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SAILI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SailiDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SAILI.Models.Postcode> Postcodes { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Home> Homes { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Contact> Contacts { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Owner> Owners { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.SaltOwner> SaltOwners { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.TraderAccount> TraderAccounts { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.StockPrice> StockPrices { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Sector> Sectors { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Portfolio> Portfolios  { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Buy> Buys  { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.Sell> Sells { get; set; }

        public System.Data.Entity.DbSet<SAILI.Models.OlympicBoard> OlympicBoards { get; set; }

    }
}