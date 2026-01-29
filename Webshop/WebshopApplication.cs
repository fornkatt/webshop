using Webshop.Views.Customer;

namespace Webshop;

internal class WebshopApplication
{
    internal Services.BasketService Basket { get; }
    internal Models.Customer CurrentUser { get; set; }
    internal Services.CustomerDatabaseService Database { get; }
    private MainMenuView MainMenu { get; }
    public bool IsLoggedIn => !CurrentUser.IsGuest;

    internal WebshopApplication()
    {
        CurrentUser = new Models.Customer
        {
            Id = 0,
            FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        Database = new Services.CustomerDatabaseService();
        Basket = new Services.BasketService();
        MainMenu = new MainMenuView("Sveriges bästa butik inom PC och hårdvara!", this);
    }

    internal async Task GoToMainMenuAsync()
    {
        await MainMenu.ActivateAsync();
    }
}