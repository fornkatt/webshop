namespace Webshop.Helpers;

internal class CustomerRegistrationHelper()
{
    internal (string firstName, string lastName, string region, string city, int postalCode, string street, string houseNumber, string phone, string email) CollectAddressInput()
    {
        Console.Clear();

        MessageHelper.ShowHeader("Ange ny adress");

        var firstName = InputHelper.GetTextInput("Förnamn");
        var lastName = InputHelper.GetTextInput("Efternamn");
        var region = InputHelper.GetTextInput("Län");
        var city = InputHelper.GetTextInput("Stad");
        var postalCode = InputHelper.GetIntInput("Postkod");
        var street = InputHelper.GetTextInput("Gata");
        var houseNumber = InputHelper.GetTextInput("Husnummer");
        var phone = InputHelper.GetTextInput("Telefonnummer");
        var email = InputHelper.GetTextInput("Mejl");

        return (firstName, lastName, region, city, postalCode, street, houseNumber, phone, email);
    }
    internal (string username, string password) GetNewUsernameAndPassword()
    {
        var username = InputHelper.GetTextInput("Ange användarnamn");

        var password = "";

        while (true)
        {
            password = InputHelper.GetTextInput("Ange lösenord");

            if (password.Length < 6)
            {
                MessageHelper.ShowError("Vänligen ange ett lösenord med fler än 6 tecken");
                continue;
            }

            return (username.Trim(), password.Trim());
        }
    }
}