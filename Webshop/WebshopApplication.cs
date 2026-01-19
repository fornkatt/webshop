using Webshop.Models;
using Webshop.Views;

namespace Webshop;

internal class WebshopApplication
{
    public BasketView Basket { get; }
    public Customer CurrentUser { get; set; }
    private MainMenuView MainMenu { get; }
    public bool IsLoggedIn => !CurrentUser.IsGuest;

    public WebshopApplication()
    {
        CurrentUser = new Customer
        { 
            Name = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        Basket = new BasketView("Varukorg", this);
        MainMenu = new MainMenuView("Huvudmeny", this);
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
