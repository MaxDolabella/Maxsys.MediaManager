Seeding Data
=====

EF Core does not provide similar APIs, and database initializers also no longer exist in EF Core. To seed the database, you would put the database initialization code in the application startup. If you are using migrations, call `context.Database.Migrate()`, otherwise use `context.Database.EnsureCreated()` / `EnsureDeleted()`.

The patterns for seeding the database are discussed in issue 3070 in the Entity Framework Core repository on GitHub. **The recommended approach is to run the seeding code within a service scope in** `Startup.Configure()`:


```csharp
using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
       var context = serviceScope.ServiceProvider.GetService<MyContext>();       
       context.Database.Migrate();
       context.EnsureSeedData();
}
```
<br/>

An example of database initialization that uses migrations, along with an implementation example of `EnsureSeedData()` method:
```csharp
public static class UnicornStoreExtensions
{
    public static void EnsureSeedData(this UnicornStoreContext context)
    {
        if (context.AllMigrationsApplied())
        {
            if (!context.Products.Any())
            {
                var clothing = context.Categories.Add(new Category { DisplayName = "Clothing" }).Entity;
                var mensClothing = context.Categories.Add(new Category { DisplayName = "Mens Clothing", ParentCategory = clothing }).Entity;
                var mensShirts = context.Categories.Add(new Category { DisplayName = "Mens Shirts", ParentCategory = mensClothing }).Entity;

                var homeAndGarden = context.Categories.Add(new Category { DisplayName = "Home & Garden" }).Entity;
                var kitchenAndDining = context.Categories.Add(new Category { DisplayName = "Kitchen & Dining", ParentCategory = homeAndGarden }).Entity;

                context.Products.AddRange(
                    new Product { SKU = "MUG001", DisplayName = "Unicorn Coffee Mug (Blue)", MSRP = 12.95M, CurrentPrice = 12.95M, Category = kitchenAndDining, ImageUrl = "/images/products/CoffeeMug_Blue.png", Description = "Coffee and unicorns... what else could you need! Our flagship unicorn printed on a high quality coffee mug." },
                    new Product { SKU = "MUG002", DisplayName = "Unicorn Coffee Mug (Green)", MSRP = 12.95M, CurrentPrice = 10.95M, Category = kitchenAndDining, ImageUrl = "/images/products/CoffeeMug_Green.png", Description = "Coffee and unicorns... what else could you need! Our flagship unicorn printed on a high quality coffee mug." },
                    new Product { SKU = "MUG003", DisplayName = "Unicorn Coffee Mug (Pink)", MSRP = 12.95M, CurrentPrice = 12.95M, Category = kitchenAndDining, ImageUrl = "/images/products/CoffeeMug_Pink.png", Description = "Coffee and unicorns... what else could you need! Our flagship unicorn printed on a high quality coffee mug." },
                    new Product { SKU = "TEE201", DisplayName = "Unicorn Coffee Mug (White)", MSRP = 12.95M, CurrentPrice = 12.95M, Category = kitchenAndDining, ImageUrl = "/images/products/CoffeeMug_White.png", Description = "Coffee and unicorns... what else could you need! Our flagship unicorn printed on a high quality coffee mug." },
                    new Product { SKU = "TEE202", DisplayName = "Mens Unicorn Tee (Blue)", MSRP = 19.95M, CurrentPrice = 19.95M, Category = mensShirts, ImageUrl = "/images/products/MensTee_Blue.png", Description = "Share your love of unicorns with the world. Quality cotton t-shirt with a long lasting print." },
                    new Product { SKU = "TEE203", DisplayName = "Mens Unicorn Tee (Grey)", MSRP = 19.95M, CurrentPrice = 17.95M, Category = mensShirts, ImageUrl = "/images/products/MensTee_Grey.png", Description = "Share your love of unicorns with the world. Quality cotton t-shirt with a long lasting print." },
                    new Product { SKU = "TEE204", DisplayName = "Mens Unicorn Tee (Red/Black Stripe)", MSRP = 24.95M, CurrentPrice = 19.95M, Category = mensShirts, ImageUrl = "/images/products/MensTee_RedBlackStripe.png", Description = "Share your love of unicorns with the world. Quality cotton t-shirt with a long lasting print." });

                context.SaveChanges();
            }
        }
    }
}
```
<br/>

Please note that, in general, it is recommended to apply these operations manually (rather than performing migrations and seeding automatically on startup), to avoid racing conditions when there are multiple servers, and unintentional changes.


