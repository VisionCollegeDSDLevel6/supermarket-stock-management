using SupermarketStockManagement.Models;

namespace SupermarketStockManagement.Data
{
    public static class ProductImageSeeder
    {
        // Default image URLs used when a product has no image yet.
        public static async Task SeedProductImagesAsync(
            IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            if (!context.Products.Any())
            {
                return;
            }

            var productsWithoutImage = context.Products
                .Where(p => string.IsNullOrEmpty(p.ImageUrl))
                .ToList();

            foreach (var product in productsWithoutImage)
            {
                product.ImageUrl = GetSampleImageUrl(product.ProductId);
            }

            if (productsWithoutImage.Count > 0)
            {
                await context.SaveChangesAsync();
            }
        }

        private static string GetSampleImageUrl(int productId)
        {
            // Unsplash images are free to hotlink and stable.
            // Sample products for the customer-facing frontend.
            string[] urls =
            {
                "https://images.unsplash.com/photo-1550583724-b2692b85b150?w=500",
                "https://images.unsplash.com/photo-1586201375761-3fe87e3580ce?w=500",
                "https://images.unsplash.com/photo-1566751551758-8bed057e62c9?w=500",
                "https://images.unsplash.com/photo-1529042410759-befb128a1755?w=500",
            };

            return urls[productId % urls.Length];
        }
    }
}