namespace Webshop;

internal class WebshopApplication
{
    public Services.BasketService Basket { get; }
    public Models.Customer CurrentUser { get; set; }
    public Services.DatabaseServices DatabaseService { get; }
    private Views.MainMenuView MainMenu { get; }
    public bool IsLoggedIn => !CurrentUser.IsGuest;

    internal WebshopApplication()
    {
        CurrentUser = new Models.Customer
        {
            FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
            IsGuest = true
        };
        DatabaseService = new Services.DatabaseServices();
        Basket = new Services.BasketService();
        MainMenu = new Views.MainMenuView("Sveriges bästa butik inom PC och hårdvara!", this);
    }

    internal async Task GoToMainMenuAsync()
    {
        await MainMenu.ActivateAsync();
    }
}