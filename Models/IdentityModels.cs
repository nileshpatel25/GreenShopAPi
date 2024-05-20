using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace apiGreenShop.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string landmark { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string othercontactno { get; set; }
        public string source { get; set; }
        public string profilepic { get; set; }
        public string readonlypassword { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<productimages> Productimages { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<slider> Sliders { get; set; }
        public DbSet<Emailconfigurations> Emailconfigurations { get; set; }
        public DbSet<smsconfiguration> Smsconfigurations { get; set; }
        public DbSet<ordermaster> Ordermasters { get; set; }
        public DbSet<orderdetails> Orderdetails { get; set; }
        public DbSet<cartdetails> Cartdetails { get; set; }
        public DbSet<TaxSlabMaster> TaxSlabMasters { get; set; }
        public DbSet<SMSHistory> SMSHistories { get; set; }
        public DbSet<PromotionalMobileNo> promotionalMobileNos { get; set; }
        public DbSet<DiscountMaster> DiscountMasters { get; set; }
        public DbSet<DiscountMenuSpecific> DiscountMenuSpecifics { get; set; }
        public DbSet<DiscountUserSpecific> DiscountUserSpecifics { get; set; }
        public DbSet<pushnotificationids> Pushnotificationids { get; set; }
        public DbSet<Appversion> appversions { get; set; }

        public DbSet<UnitFactorName> UnitFactorNames { get; set; }
        public DbSet<UnitQuantityFactor> UnitQuantityFactors { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}