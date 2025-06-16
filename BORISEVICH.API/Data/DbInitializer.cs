using BORISEVICH.Domain.Entities;

namespace BORISEVICH.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
            // Заполнение данными
            if (!context.Categories.Any() && !context.Books.Any())
            {
                var categories = new Category[]
                {
                new Category { Name="Повесть", NormalizedName="story"},
                new Category { Name="Роман", NormalizedName="novel"},
                new Category { Name="Детектив", NormalizedName="detective"},
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var books = new List<Book>
                {
                    new Book {Name="Белые ночи",
                        Author="Достоевский Ф.М.",
                        Page =320,
                        Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("story")),
                        Image=uri+"Images/dostoevski.jpg" },
                    new Book { Name="Морфий",
                        Author="Булгаков М.А.",
                        Page =330,
                        Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("novel")),
                        Image=uri+"Images/bulgakov.jpg" },
                    new Book { Name="Атлант расправил плечи",
                        Author="Рэнд А.",
                        Page =1136,
                        Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("novel")),
                        Image=uri+"Images/rand.jpg" },
                    new Book { Name = "Каласы под сярпом тваiм",
                        Author="Короткевич В.С.",
                        Page =928,
                        Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("novel")),
                        Image=uri + "Images/karatkevich.jpg" },
                    new Book { Name = "Токийский зодиак",
                        Author="Симада С.",
                        Page =352,
                        Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("detective")),
                        Image=uri + "Images/simada.jpg" }
                };
                await context.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }
    }
}
