namespace Webshop.Services;

internal class CheckoutService(WebshopApplication app)
{
    protected WebshopApplication App = app;

    internal async Task<bool> CompleteOrder(Models.ShippingMethod shippingMethod, Models.PaymentMethod paymentMethod)
    {
        try
        {
            using var db = new Models.WebshopDbContext();

            if (App.CurrentUser.Id == 0)
            {
                db.Customers.Add(App.CurrentUser);
                await db.SaveChangesAsync();
            }

            var basketItems = App.Basket.GetItems();

            if (App.CurrentUser.Address!.Id == 0)
            {
                //App.CurrentUser.Address.Country = null;
                db.Addresses.Add(App.CurrentUser.Address);
                await db.SaveChangesAsync();
            }

            var order = new Models.Order
            {
                CustomerId = App.CurrentUser.Id,
                OrderDate = DateTime.Now,
                Status = Models.OrderStatus.Pending,
                ShippingAddressId = App.CurrentUser.Address!.Id,
                ShippingMethodId = shippingMethod.Id,
                ShippingCost = shippingMethod.Price,
                PaymentMethodId = paymentMethod.Id,
                PaymentStatus = Models.PaymentStatus.Pending,
                OrderItems = []
            };

            decimal subtotal = 0;
            foreach (var item in basketItems)
            {
                var orderItem = new Models.OrderItem
                {
                    Order = order,
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name!,
                    Quantity = item.Quantity,
                    UnitPrice = (decimal)item.Product.Price!,
                    Subtotal = ((decimal)item.Product.Price!) * item.Quantity
                };

                subtotal += orderItem.Subtotal;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = subtotal 
                + shippingMethod.Price 
                + (paymentMethod.TransactionFee);

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            App.Basket.Clear();

            return true;
        }
        catch (Exception e)
        {
            Console.Clear();
            Console.WriteLine("Ett fel uppstod vid beställningen:");
            Console.WriteLine(e.InnerException);
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri tangent för att fortsätta.");

            Console.ReadKey(true);

            return false;
        }
    }

    private T? SelectFromList<T>(List<T> items, string prompt, Func<T, string> displayFormatter) where T : class
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Console.WriteLine();

            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {displayFormatter(items[i])}");
                Console.WriteLine();
            }

            var choice = Console.ReadKey(true).KeyChar.ToString();

            if (choice.ToUpper() == "Q")
            {
                return null;
            }

            if (int.TryParse(choice, out int choiceInt) && choiceInt >= 1 && choiceInt <= items.Count)
            {
                return items[choiceInt - 1];
            }
        }
    }

    internal async Task<Models.PaymentMethod?> GetPaymentMethodAsync()
    {
        var paymentMethods = await App.DatabaseService.GetPaymentMethodsAsync();

        return SelectFromList(
            paymentMethods,
            "Välj en betalmetod. Q för att avsluta.",
            pm => $"""
                {pm.Name}
                Leverantör: {pm.Provider}
                Eventuell avgift: {pm.TransactionFee:C}
                """
        );
    }

    internal async Task<Models.ShippingMethod?> GetShippingMethodAsync()
    {
        var shippingMethods = await App.DatabaseService.GetShippingMethodsAsync();

        return SelectFromList(
            shippingMethods,
            "Välj fraktalternativ. Q för att avsluta.",
            sm => $"""
                {sm.Name} - {sm.Price:C}
                {sm.Description}
                Leveranstid: {sm.EstimatedDaysMin}-{sm.EstimatedDaysMax} dagar
                """
        );
    }

    internal void SetCustomerAddress(string city,
        int postalCode,
        string street,
        string houseNumber,
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        var address = new Models.Address()
        {
            CustomerId = App.CurrentUser.Id,
            CountryId = 1,
            City = city,
            PostalCode = postalCode,
            Street = street,
            HouseNumber = houseNumber,
        };

        App.CurrentUser.Address = address;

        App.CurrentUser.FirstName = firstName;
        App.CurrentUser.LastName = lastName;
        App.CurrentUser.Phone = phone;
        App.CurrentUser.Email = email;
    }
}
