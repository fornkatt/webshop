using Webshop.Views.Customer;

namespace Webshop;

internal sealed class WebshopApplication
{
    internal Services.MongoLogService MongoLogService { get; }
    internal Services.BasketService Basket { get; }
    internal Models.Customer CurrentUser { get; set; }
    internal Services.CustomerDatabaseService Database { get; }
    private MainMenuView MainMenu { get; }
    internal bool IsLoggedIn => !CurrentUser.IsGuest;

    internal WebshopApplication()
    {
        CurrentUser = new Models.Customer
        {
            Id = 0,
            FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        MongoLogService = new Services.MongoLogService();
        Database = new Services.CustomerDatabaseService();
        Basket = new Services.BasketService(MongoLogService);
        MainMenu = new MainMenuView("Datorhallen - Sveriges bästa butik inom PC och hårdvara!", this);
    }

    internal async Task GoToMainMenuAsync()
    {
        await MainMenu.ActivateAsync();
    }
}