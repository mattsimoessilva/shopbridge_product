using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models.Entities;

namespace ProductAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductAppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
                return;

            // Step 1: Seed Products
            var products = new[] {
                new Product { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Name = "Mechanical Keyboard", ShortDescription = "RGB mechanical keyboard", FullDescription = "High-performance mechanical keyboard with customizable RGB lighting.", Price = 349.99m, DiscountPrice = 299.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "Corsair", Category = "Accessories", Tags = "keyboard,mechanical,RGB", ImageUrl = "/images/products/mechanical-keyboard.jpg", ThumbnailUrl = "/images/products/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Performance", Slug = "mechanical-keyboard", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Name = "Gaming Headset", ShortDescription = "Surround sound headset", FullDescription = "Immersive 7.1 surround sound gaming headset with noise-canceling mic.", Price = 249.99m, DiscountPrice = 199.99m, IsActive = true, IsFeatured = true, SKU = "GH-003", StockQuantity = 120, MinimumStockThreshold = 15, AllowBackorder = false, Brand = "Razer", Category = "Audio", Tags = "headset,gaming,surround", ImageUrl = "/images/products/gaming-headset.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-headset.jpg", SeoTitle = "Gaming Headset - Surround Sound", Slug = "gaming-headset", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // Step 2: Retrieve saved products with IDs
            var savedProducts = context.Products.ToList();

            // Step 3: Seed Variants
            var variants = new List<ProductVariant>();

            foreach (var product in savedProducts)
            {
                if (product.Name == "Wireless Mouse")
                {
                    variants.AddRange(
                    [
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Compact Black", Color = "Black", Size = "Small", AdditionalPrice = 0, StockQuantity = 100, ImageUrl = "/images/products/wireless-mouse-black.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Ergo Gray", Color = "Gray", Size = "Medium", AdditionalPrice = 10, StockQuantity = 80, ImageUrl = "/images/products/wireless-mouse-gray.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Silent White", Color = "White", Size = "Large", AdditionalPrice = 5, StockQuantity = 60, ImageUrl = "/images/products/wireless-mouse-white.jpg", IsActive = true }
                    ]);
                }
                else if (product.Name == "Mechanical Keyboard")
                {
                    variants.AddRange(
                    [
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "RGB Black", Color = "Black", Size = "Full", AdditionalPrice = 0, StockQuantity = 50, ImageUrl = "/images/products/mechanical-keyboard-black.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Silent White", Color = "White", Size = "Tenkeyless", AdditionalPrice = 20, StockQuantity = 40, ImageUrl = "/images/products/mechanical-keyboard-white.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Compact Red", Color = "Red", Size = "60%", AdditionalPrice = -10, StockQuantity = 30, ImageUrl = "/images/products/mechanical-keyboard-red.jpg", IsActive = true }
                    ]);
                }
                else if (product.Name == "Gaming Headset")
                {
                    variants.AddRange(
                    [
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", AdditionalPrice = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "Neon Green", Color = "Green", Size = "Large", AdditionalPrice = 15, StockQuantity = 50, ImageUrl = "/images/products/gaming-headset-green.jpg", IsActive = true },
                        new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, VariantName = "White Noise", Color = "White", Size = "Standard", AdditionalPrice = 10, StockQuantity = 40, ImageUrl = "/images/products/gaming-headset-white.jpg", IsActive = true }
                    ]);
                }
            }

            context.ProductVariants.AddRange(variants);
            context.SaveChanges();

            // Step 4: Seed Reviews
            var reviews = new List<ProductReview>();

            foreach (var product in savedProducts)
            {
                if (product.Name == "Wireless Mouse")
                {
                    reviews.AddRange(new[]
                    {
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "matheus123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "tech_guy", Rating = 4, Comment = "Great mouse, but wish the battery lasted longer.", CreatedAt = DateTime.UtcNow.AddDays(-3), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "gamer_babe", Rating = 5, Comment = "Perfect for gaming and work!", CreatedAt = DateTime.UtcNow.AddDays(-1), IsVerifiedPurchase = false }
                    });
                }
                else if (product.Name == "Mechanical Keyboard")
                {
                    reviews.AddRange(new[]
                    {
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "corsairFan", Rating = 5, Comment = "Clicky keys and stunning RGB!", CreatedAt = DateTime.UtcNow.AddDays(-7), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "quietCoder", Rating = 3, Comment = "Too loud for my taste, but solid build.", CreatedAt = DateTime.UtcNow.AddDays(-4), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "rgbQueen", Rating = 4, Comment = "Love the lighting effects!", CreatedAt = DateTime.UtcNow.AddDays(-2), IsVerifiedPurchase = false }
                    });
                }
                else if (product.Name == "Gaming Headset")
                {
                    reviews.AddRange(new[]
                    {
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "razerMaster", Rating = 5, Comment = "Immersive sound and comfy fit!", CreatedAt = DateTime.UtcNow.AddDays(-6), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "streamerGirl", Rating = 4, Comment = "Mic is clear, but ear cups get warm.", CreatedAt = DateTime.UtcNow.AddDays(-3), IsVerifiedPurchase = true },
                        new ProductReview { Id = Guid.NewGuid(), ProductId = product.Id, Product = product, UserId = "bassHunter", Rating = 5, Comment = "Bass is insane. Highly recommend!", CreatedAt = DateTime.UtcNow.AddDays(-1), IsVerifiedPurchase = false }
                    });
                }
            }

            context.ProductReviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}
