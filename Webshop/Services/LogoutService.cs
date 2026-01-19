namespace Webshop.Services
{
    internal class LogoutService(WebshopApplication app)
    {
        public WebshopApplication App { get; } = app;
        public void Logout()
        {
            App.CurrentUser = new()
            {
                Name = "guestUser" + Random.Shared.Next(0, 10000000),
                IsGuest = true
            };
            App.ReturnToMainMenu();
        }
    }
}
