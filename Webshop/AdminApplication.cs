namespace Webshop;

internal class AdminApplication
{
    internal Services.DatabaseService Database { get; }
    private Views.AdminView AdminView { get; }

    internal AdminApplication()
    {

        Database = new Services.DatabaseService();
        AdminView = new Views.AdminView("Adminpanelen", this);
    }

    internal async Task GoToAdminViewAsync()
    {
        await AdminView.ActivateAsync();
    }
}