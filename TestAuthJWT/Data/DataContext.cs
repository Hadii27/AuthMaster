using AuthMaster.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAuthJWT.Model;

namespace TestAuthJWT.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Define your DbSet properties here
        public DbSet<CategoryModel> categories { get; set; }
        public DbSet<ProductModel> products { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<Cart> carts { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderItems> ordersItems { get; set; }

    }
}
