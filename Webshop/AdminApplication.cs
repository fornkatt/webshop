namespace Webshop;

internal class AdminApplication
{
    public Services.DatabaseServices Database { get; }
    private Views.AdminView AdminView { get; }

    internal AdminApplication()
    {
        Database = new Services.DatabaseServices();
        AdminView = new Views.AdminView("Adminpanelen", this);
    }

    internal async Task GoToAdminViewAsync()
    {
        await AdminView.ActivateAsync();
    }
}