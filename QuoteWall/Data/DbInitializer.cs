using QuoteWall.Models;

namespace QuoteWall.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();
            // Check if there are any quotes already in the database
            if (context.Quotes.Any())
            {
                return; // Database has been seeded
            }
            // Seed initial quotes
            var quotes = new Quote[]
            {
                new Quote { Text = "Єдиний спосіб робити велику справу — це любити те, що ти робиш.", Author = "Steve Jobs", Category = CategoryEnum.Motivation },
                new Quote { Text = "Я не сперечаюся, я просто пояснюю, чому я правий.", Author = "Unknown", Category = CategoryEnum.Humor },
                new Quote { Text = "Посеред труднощів лежить можливість.", Author = "Albert Einstein", Category = CategoryEnum.Wisdom },
                new Quote { Text = "Код схожий на гумор. Коли його потрібно пояснювати, він поганий.", Author = "Cory House", Category = CategoryEnum.Tech }
            };
            foreach (var quote in quotes)
            {
                context.Quotes.Add(quote);
            }
            context.SaveChanges();
        }
    }
}
