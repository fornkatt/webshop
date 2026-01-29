using Webshop.Views.Admin.Management;

namespace Webshop;

internal class AdminApplication
{
    internal Services.MongoLogService MongoLogService { get; }
    internal Services.AdminDatabaseService Database { get; }
    private AdminView AdminView { get; }

    internal AdminApplication()
    {
        MongoLogService = new Services.MongoLogService();
        Database = new Services.AdminDatabaseService();
        AdminView = new AdminView("Adminpanelen", this);
    }

    internal async Task GoToAdminViewAsync()
    {
        await AdminView.ActivateAsync();
    }
}