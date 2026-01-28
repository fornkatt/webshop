using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Services;

internal class DatabaseSeederService
{
    public async Task SeedDatabaseAsync()
    {
        using var db = new WebshopDbContext();

        // Seed Country (Sweden)
        if (!db.Countries.Any())
        {
            var sweden = new Country { Name = "Sverige", Code = "SE" };
            db.Countries.Add(sweden);
            await db.SaveChangesAsync();
        }

        // Seed Categories (4 categories - under limit of 5)
        if (!db.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Datorkomponenter", IsActive = true },
                new Category { Name = "Mjukvara", IsActive = true },
                new Category { Name = "Datortillbehör", IsActive = true },
                new Category { Name = "Bildskärmar", IsActive = true }
            };
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync();
        }

        // Seed Suppliers
        if (!db.Suppliers.Any())
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { Name = "Nordic Hardware AB" },
                new Supplier { Name = "Software Solutions Sweden" },
                new Supplier { Name = "PC Accessories Nordic" },
                new Supplier { Name = "Display Tech Scandinavia" }
            };
            db.Suppliers.AddRange(suppliers);
            await db.SaveChangesAsync();
        }

        // Seed Shipping Methods
        if (!db.ShippingMethods.Any())
        {
            var shippingMethods = new List<ShippingMethod>
            {
                new ShippingMethod { Name = "PostNord", Description = "Standard", Price = 49m, EstimatedDaysMin = 2, EstimatedDaysMax = 5, IsActive = true },
                new ShippingMethod { Name = "DHL Express", Description = "Express", Price = 149m, EstimatedDaysMin = 1, EstimatedDaysMax = 1, IsActive = true },
                new ShippingMethod { Name = "Butik", Description = "Hämta i butik", Price = 79m, EstimatedDaysMin = 1, EstimatedDaysMax = 3, IsActive = true }
            };
            db.ShippingMethods.AddRange(shippingMethods);
            await db.SaveChangesAsync();
        }

        // Seed Payment Methods
        if (!db.PaymentMethods.Any())
        {
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod { Name = "Swish", Provider = "Swish AB", TransactionFee = 0m, IsActive = true },
                new PaymentMethod { Name = "Bankkort", Provider = "Klarna", TransactionFee = 15m, IsActive = true },
                new PaymentMethod { Name = "Faktura", Provider = "Klarna", TransactionFee = 49m, IsActive = true }
            };
            db.PaymentMethods.AddRange(paymentMethods);
            await db.SaveChangesAsync();
        }

        // Seed Products - Only 3 with IsSelectedItem = true
        if (!db.Products.Any())
        {
            var products = new List<Product>
            {
                // Datorkomponenter
                new Product { Name = "AMD Ryzen 9 7950X", ShortDescription = "16-kärnig processor", DetailedDescription = "Högpresterande processor med 16 kärnor och 32 trådar, 5.7 GHz boost", CategoryId = 1, SupplierId = 1, Stock = 15, Price = 6499m, OriginalPrice = 6999m, IsActive = true, IsSelectedItem = true, IsSaleItem = true, IsDiscontinued = false },
                new Product { Name = "NVIDIA GeForce RTX 4080", ShortDescription = "Kraftfullt grafikkort", DetailedDescription = "16GB GDDR6X, perfekt för gaming och AI", CategoryId = 1, SupplierId = 1, Stock = 8, Price = 13999m, OriginalPrice = 13999m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Corsair Vengeance DDR5 32GB", ShortDescription = "Höghasighets-RAM", DetailedDescription = "32GB (2x16GB) DDR5 6000MHz RGB", CategoryId = 1, SupplierId = 1, Stock = 42, Price = 1899m, OriginalPrice = 1899m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Samsung 990 PRO 2TB", ShortDescription = "NVMe SSD", DetailedDescription = "M.2 NVMe PCIe 4.0 SSD, upp till 7450 MB/s", CategoryId = 1, SupplierId = 1, Stock = 35, Price = 2299m, OriginalPrice = 2799m, IsActive = true, IsSelectedItem = false, IsSaleItem = true, IsDiscontinued = false },
                new Product { Name = "ASUS ROG Strix Z790-E", ShortDescription = "Gaming moderkort", DetailedDescription = "ATX moderkort för Intel 13th/14th gen, WiFi 6E", CategoryId = 1, SupplierId = 1, Stock = 12, Price = 5499m, OriginalPrice = 5499m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Noctua NH-D15", ShortDescription = "CPU-kylare", DetailedDescription = "Premium luftkylare med dubbla fläktar, mycket tyst", CategoryId = 1, SupplierId = 1, Stock = 28, Price = 1299m, OriginalPrice = 1299m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },

                // Mjukvara
                new Product { Name = "Windows 11 Pro", ShortDescription = "Operativsystem", DetailedDescription = "Windows 11 Professional, fullversion licens", CategoryId = 2, SupplierId = 2, Stock = 100, Price = 2499m, OriginalPrice = 2499m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Microsoft Office 365", ShortDescription = "Kontorspaket", DetailedDescription = "1 års prenumeration, Word, Excel, PowerPoint m.m.", CategoryId = 2, SupplierId = 2, Stock = 150, Price = 899m, OriginalPrice = 1099m, IsActive = true, IsSelectedItem = true, IsSaleItem = true, IsDiscontinued = false },
                new Product { Name = "Adobe Creative Cloud", ShortDescription = "Kreativ svit", DetailedDescription = "1 års licens, Photoshop, Illustrator, Premiere Pro", CategoryId = 2, SupplierId = 2, Stock = 80, Price = 7499m, OriginalPrice = 7499m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Malwarebytes Premium", ShortDescription = "Antivirusprogram", DetailedDescription = "1 års licens, skydd mot malware och ransomware", CategoryId = 2, SupplierId = 2, Stock = 120, Price = 449m, OriginalPrice = 449m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },

                // Datortillbehör
                new Product { Name = "Logitech MX Master 3S", ShortDescription = "Trådlös mus", DetailedDescription = "Ergonomisk mus med 8K DPI sensor, tysta knappar", CategoryId = 3, SupplierId = 3, Stock = 55, Price = 1199m, OriginalPrice = 1199m, IsActive = true, IsSelectedItem = true, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Keychron K8 Pro", ShortDescription = "Mekaniskt tangentbord", DetailedDescription = "Trådlöst mekaniskt tangentbord, hot-swappable", CategoryId = 3, SupplierId = 3, Stock = 32, Price = 1799m, OriginalPrice = 1799m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "HyperX Cloud III", ShortDescription = "Gaming headset", DetailedDescription = "7.1 surroundljud, bekväma öronkuddar", CategoryId = 3, SupplierId = 3, Stock = 48, Price = 1299m, OriginalPrice = 1299m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Logitech C920 Pro", ShortDescription = "Webbkamera", DetailedDescription = "Full HD 1080p webbkamera med autofokus", CategoryId = 3, SupplierId = 3, Stock = 65, Price = 899m, OriginalPrice = 1199m, IsActive = true, IsSelectedItem = false, IsSaleItem = true, IsDiscontinued = false },
                new Product { Name = "Blue Yeti", ShortDescription = "USB-mikrofon", DetailedDescription = "Professionell USB-mikrofon för streaming/podcasts", CategoryId = 3, SupplierId = 3, Stock = 22, Price = 1599m, OriginalPrice = 1599m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },

                // Bildskärmar
                new Product { Name = "Dell UltraSharp U2723DE", ShortDescription = "27\" 4K skärm", DetailedDescription = "27\" IPS 4K (3840x2160), USB-C 90W, 99% sRGB", CategoryId = 4, SupplierId = 4, Stock = 18, Price = 6499m, OriginalPrice = 6499m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "ASUS ROG Swift", ShortDescription = "27\" 240Hz gaming", DetailedDescription = "27\" QHD (2560x1440), 240Hz, G-SYNC, IPS", CategoryId = 4, SupplierId = 4, Stock = 12, Price = 7999m, OriginalPrice = 7999m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "Samsung Odyssey G9", ShortDescription = "49\" ultrawide", DetailedDescription = "49\" QLED 5120x1440, 240Hz, 1000R kurva", CategoryId = 4, SupplierId = 4, Stock = 6, Price = 14999m, OriginalPrice = 14999m, IsActive = true, IsSelectedItem = false, IsSaleItem = false, IsDiscontinued = false },
                new Product { Name = "LG 27UP850-W", ShortDescription = "27\" 4K USB-C", DetailedDescription = "27\" IPS 4K, HDR400, USB-C 96W, ergonomisk", CategoryId = 4, SupplierId = 4, Stock = 25, Price = 5499m, OriginalPrice = 6499m, IsActive = true, IsSelectedItem = false, IsSaleItem = true, IsDiscontinued = false }
            };
            db.Products.AddRange(products);
            await db.SaveChangesAsync();
        }

        // Seed Customers
        if (!db.Customers.Any())
        {
            var swedishCities = new[] { "Stockholm", "Göteborg", "Malmö", "Uppsala", "Västerås", "Örebro", "Linköping", "Helsingborg", "Jönköping", "Norrköping" };
            var streets = new[] { "Storgatan", "Kungsgatan", "Drottninggatan", "Vasagatan", "Sveavägen", "Birger Jarlsgatan", "Hamngatan", "Östergatan", "Norra vägen", "Södra vägen" };

            var users = new List<(string FirstName, string LastName, string Username, int Age)>
            {
                ("Erik", "Andersson", "erik.a", 38),
                ("Anna", "Johansson", "anna.j", 31),
                ("Lars", "Karlsson", "lars.k", 45),
                ("Maria", "Nilsson", "maria.n", 35),
                ("Per", "Eriksson", "per.e", 28),
                ("Karin", "Larsson", "karin.l", 40),
                ("Johan", "Olsson", "johan.o", 33),
                ("Emma", "Persson", "emma.p", 26),
                ("Anders", "Svensson", "anders.s", 42),
                ("Linda", "Gustafsson", "linda.g", 30)
            };

            var random = new Random(42);
            var customers = new List<Customer>();

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                var customer = new Customer
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Password = "password123",
                    Email = $"{user.Username}@example.se",
                    Phone = $"070-{random.Next(100, 999)}{random.Next(10, 99)}{random.Next(10, 99)}",
                    Age = user.Age,
                    IsActive = true,
                    IsGuest = false,
                    Address = new Address
                    {
                        CountryId = 1,
                        City = swedishCities[random.Next(swedishCities.Length)],
                        PostalCode = random.Next(10000, 99999),
                        Street = streets[random.Next(streets.Length)],
                        HouseNumber = random.Next(2) == 0
                            ? $"{random.Next(1, 150)}"
                            : $"{random.Next(1, 150)}{(char)('A' + random.Next(4))}"
                    }
                };
                customers.Add(customer);
            }

            db.Customers.AddRange(customers);
            await db.SaveChangesAsync();

            // Seed orders (1-3 per customer)
            var allProducts = await db.Products.ToListAsync();
            var shippingMethods = await db.ShippingMethods.ToListAsync();
            var paymentMethods = await db.PaymentMethods.ToListAsync();

            foreach (var customer in customers)
            {
                int orderCount = random.Next(1, 4);

                for (int orderIndex = 0; orderIndex < orderCount; orderIndex++)
                {
                    var daysAgo = random.Next(30, 365);
                    var orderDate = DateTime.Now.AddDays(-daysAgo);
                    var shippingMethod = shippingMethods[random.Next(shippingMethods.Count)];
                    var paymentMethod = paymentMethods[random.Next(paymentMethods.Count)];

                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        OrderDate = orderDate,
                        Status = OrderStatus.Delivered,
                        ShippingAddressId = customer.Address!.Id,
                        ShippingMethodId = shippingMethod.Id,
                        ShippingCost = shippingMethod.Price,
                        PaymentMethodId = paymentMethod.Id,
                        PaymentStatus = PaymentStatus.Completed,
                        OrderItems = new List<OrderItem>()
                    };

                    int itemCount = random.Next(1, 4);
                    decimal subtotal = 0;

                    for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                    {
                        var product = allProducts[random.Next(allProducts.Count)];
                        var quantity = random.Next(1, 3);
                        var itemSubtotal = product.Price!.Value * quantity;

                        order.OrderItems.Add(new OrderItem
                        {
                            Order = order,
                            ProductId = product.Id,
                            ProductName = product.Name!,
                            Quantity = quantity,
                            UnitPrice = product.Price.Value,
                            Subtotal = itemSubtotal
                        });
                        subtotal += itemSubtotal;
                    }

                    order.TotalAmount = subtotal + shippingMethod.Price + paymentMethod.TransactionFee;
                    db.Orders.Add(order);
                }
            }
            await db.SaveChangesAsync();
        }

        Console.WriteLine("✓ Database seeded successfully!");
        Console.WriteLine($"✓ Categories: {db.Categories.Count()}");
        Console.WriteLine($"✓ Suppliers: {db.Suppliers.Count()}");
        Console.WriteLine($"✓ Products: {db.Products.Count()}");
        Console.WriteLine($"✓ Customers: {db.Customers.Count()}");
        Console.WriteLine($"✓ Orders: {db.Orders.Count()}");
    }
}