using Webshop.Views.Admin;

namespace Webshop;

internal sealed class AdminApplication
{
    internal Services.MongoLogService MongoLogService { get; }
    internal Services.AdminDatabaseService Database { get; }
    private AdminView AdminView { get; }
    private bool _shouldExitToWebshop = false;

    internal AdminApplication()
    {
        MongoLogService = new Services.MongoLogService();
        Database = new Services.AdminDatabaseService();
        AdminView = new AdminView("Adminpanelen", this);
    }

    internal async Task GoToAdminViewAsync()
    {
        _shouldExitToWebshop = false;
        await AdminView.ActivateAsync();
    }

    internal void ExitToWebshop()
    {
        _shouldExitToWebshop = true;
    }

    internal bool ShouldExitToWebshop() => _shouldExitToWebshop;
}