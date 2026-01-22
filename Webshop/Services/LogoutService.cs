namespace Webshop.Services
{
    internal class LogoutService(WebshopApplication app)
    {
        public WebshopApplication App { get; } = app;
        public async Task LogoutAsync()
        {
            Console.Clear();
            Console.WriteLine("""
            Är du säker på att du vill logga ut?
            
            Y/n

            """);

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                App.CurrentUser = new()
                {
                    FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
                    IsGuest = true
                };
                await App.ReturnToMainMenuAsync();
            }
            return; 
        }
    }
}