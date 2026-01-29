using Webshop.Views.Admin.Management;

namespace Webshop;

internal class AdminApplication
{
    internal Services.AdminDatabaseService Database { get; }
    private AdminView AdminView { get; }

    internal AdminApplication()
    {
        Database = new Services.AdminDatabaseService();
        AdminView = new AdminView("Adminpanelen", this);
    }

    internal async Task GoToAdminViewAsync()
    {
        await AdminView.ActivateAsync();
    }
}