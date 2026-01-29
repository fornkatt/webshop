namespace Webshop.Services;

internal sealed class CheckoutService(WebshopApplication app)
{
    private readonly WebshopApplication _app = app;

    internal async Task<bool> CompleteOrder(Models.ShippingMethod shippingMethod, Models.PaymentMethod paymentMethod)
    {
        try
        {
            if (_app.CurrentUser.Id == 0)
            {
                await _app.Database.AddNewCustomerAsync(_app.CurrentUser);
            }

            var basketItems = _app.Basket.GetItems();

            if (_app.CurrentUser.Address!.Id == 0)
            {
                await _app.Database.AddNewAddressAsync(_app.CurrentUser.Address);
            }

            var order = new Models.Order
            {
                CustomerId = _app.CurrentUser.Id,
                OrderDate = DateTime.Now,
                Status = Models.OrderStatus.Pending,
                ShippingAddressId = _app.CurrentUser.Address!.Id,
                ShippingMethodId = shippingMethod.Id,
                ShippingCost = shippingMethod.Price,
                PaymentMethodCost = paymentMethod.TransactionFee,
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

            await _app.Database.AddNewOrderAsync(order);

            _app.Basket.Clear();

            return true;
        }
        catch (Exception e)
        {
            Helpers.MessageHelper.ShowError($"""
                Ett fel uppstod vid beställningen: {e.Message}
                """);
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
                Console.WriteLine($"{i + 1}: {displayFormatter(items[i])}");
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
        var paymentMethods = await _app.Database.GetPaymentMethodsAsync();

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
        var shippingMethods = await _app.Database.GetShippingMethodsAsync();

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
}