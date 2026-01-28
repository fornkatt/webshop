namespace Webshop;

internal class WebshopApplication
{
    internal Services.BasketService Basket { get; }
    internal Models.Customer CurrentUser { get; set; }
    internal Services.DatabaseService Database { get; }
    private Views.MainMenuView MainMenu { get; }
    public bool IsLoggedIn => !CurrentUser.IsGuest;

    internal WebshopApplication()
    {
        CurrentUser = new Models.Customer
        {
            FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        Database = new Services.DatabaseService();
        Basket = new Services.BasketService();
        MainMenu = new Views.MainMenuView("Sveriges bästa butik inom PC och hårdvara!", this);
    }

    internal async Task GoToMainMenuAsync()
    {
        await MainMenu.ActivateAsync();
    }
}