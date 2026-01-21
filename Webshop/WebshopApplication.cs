using Webshop.Views;

namespace Webshop;

internal class WebshopApplication
{
    public Services.BasketService Basket { get; }
    public Models.Customer CurrentUser { get; set; }
    public Services.DatabaseServices DatabaseService { get; }
    private MainMenuView MainMenu { get; }
    public bool IsLoggedIn => !CurrentUser.IsGuest;

    public WebshopApplication()
    {
        CurrentUser = new Models.Customer
        { 
            FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        DatabaseService = new Services.DatabaseServices();
        Basket = new Services.BasketService();
        MainMenu = new MainMenuView("Sveriges bästa butik inom PC och hårdvara!", this);
    }

    internal void Run()
    {
        MainMenu.Activate();
    }

    internal void ReturnToMainMenu()
    {
        MainMenu.Activate();
    }
}
