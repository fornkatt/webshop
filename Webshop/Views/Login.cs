using Webshop.Services;

namespace Webshop.Views;

internal class Login(string headerText)
{
    private string HeaderText { get; set; } = headerText;

    public void Activate()
    {
        Console.Clear();
        Console.WriteLine(HeaderText);
        Console.WriteLine();
    }
}
