using Webshop.Models;

namespace Webshop.Services;

internal class DatabaseSeederService
{
    public async Task SeedDatabaseAsync()
    {
        using var db = new WebshopDbContext();

        // Seed Country (Sweden)
        var sweden = new Country { Name = "Sverige", Code = "SE" };
        if (!db.Countries.Any())
        {
            db.Countries.Add(sweden);
            await db.SaveChangesAsync();
        }

        // Seed Categories - Computer Hardware Store
        var categories = new List<Category>
        {
            new Category { Name = "Datorkomponenter" },
            new Category { Name = "Mjukvara" },
            new Category { Name = "Datortillbehör" },
            new Category { Name = "Bildskärmar" }
        };

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync();
        }

        // Seed Suppliers
        var suppliers = new List<Supplier>
        {
            new Supplier { Name = "Nordic Hardware AB" },
            new Supplier { Name = "Software Solutions Sweden" },
            new Supplier { Name = "PC Accessories Nordic" },
            new Supplier { Name = "Display Tech Scandinavia" }
        };

        if (!db.Suppliers.Any())
        {
            db.Suppliers.AddRange(suppliers);
            await db.SaveChangesAsync();
        }

        // Seed Products
        if (!db.Products.Any())
        {
            var products = new List<Product>
            {
                // Datorkomponenter (6 products)
                new Product { Name = "AMD Ryzen 9 7950X", ShortDescription = "16-kärnig processor", DetailedDescription = "Högpresterande processor med 16 kärnor och 32 trådar, 5.7 GHz boost", CategoryId = 1, SupplierId = 1, Stock = 15, Price = 6499m, IsActive = true, IsSelectedItem = true },
                new Product { Name = "NVIDIA GeForce RTX 4080", ShortDescription = "Kraftfullt grafikkort", DetailedDescription = "16GB GDDR6X, perfekt för gaming och AI", CategoryId = 1, SupplierId = 1, Stock = 8, Price = 13999m, IsActive = true },
                new Product { Name = "Corsair Vengeance DDR5 32GB", ShortDescription = "Höghasighets-RAM", DetailedDescription = "32GB (2x16GB) DDR5 6000MHz RGB", CategoryId = 1, SupplierId = 1, Stock = 42, Price = 1899m, IsActive = true },
                new Product { Name = "Samsung 990 PRO 2TB", ShortDescription = "NVMe SSD", DetailedDescription = "M.2 NVMe PCIe 4.0 SSD, upp till 7450 MB/s", CategoryId = 1, SupplierId = 1, Stock = 35, Price = 2299m, IsActive = true, IsSaleItem = true },
                new Product { Name = "ASUS ROG Strix Z790-E", ShortDescription = "Gaming moderkort", DetailedDescription = "ATX moderkort för Intel 13th/14th gen, WiFi 6E", CategoryId = 1, SupplierId = 1, Stock = 12, Price = 5499m, IsActive = true },
                new Product { Name = "Noctua NH-D15", ShortDescription = "CPU-kylare", DetailedDescription = "Premium luftkylare med dubbla fläktar, mycket tyst", CategoryId = 1, SupplierId = 1, Stock = 28, Price = 1299m, IsActive = true },

                // Mjukvara (6 products)
                new Product { Name = "Windows 11 Pro", ShortDescription = "Operativsystem", DetailedDescription = "Windows 11 Professional, fullversion licens", CategoryId = 2, SupplierId = 2, Stock = 100, Price = 2499m, IsActive = true },
                new Product { Name = "Microsoft Office 365", ShortDescription = "Kontorspaket", DetailedDescription = "1 års prenumeration, Word, Excel, PowerPoint m.m.", CategoryId = 2, SupplierId = 2, Stock = 150, Price = 899m, IsActive = true, IsSaleItem = true },
                new Product { Name = "Adobe Creative Cloud", ShortDescription = "Kreativ svit", DetailedDescription = "1 års licens, Photoshop, Illustrator, Premiere Pro", CategoryId = 2, SupplierId = 2, Stock = 80, Price = 7499m, IsActive = true },
                new Product { Name = "Malwarebytes Premium", ShortDescription = "Antivirusprogram", DetailedDescription = "1 års licens, skydd mot malware och ransomware", CategoryId = 2, SupplierId = 2, Stock = 120, Price = 449m, IsActive = true },
                new Product { Name = "Visual Studio Professional", ShortDescription = "Utvecklingsmiljö", DetailedDescription = "Professionell IDE för .NET utveckling", CategoryId = 2, SupplierId = 2, Stock = 50, Price = 4999m, IsActive = true, IsSelectedItem = true },
                new Product { Name = "AutoCAD 2026", ShortDescription = "CAD-program", DetailedDescription = "1 års licens för professionell CAD design", CategoryId = 2, SupplierId = 2, Stock = 30, Price = 18999m, IsActive = true },

                // Datortillbehör (7 products)
                new Product { Name = "Logitech MX Master 3S", ShortDescription = "Trådlös mus", DetailedDescription = "Ergonomisk mus med 8K DPI sensor, tyst knappar", CategoryId = 3, SupplierId = 3, Stock = 55, Price = 1199m, IsActive = true, IsSelectedItem = true },
                new Product { Name = "Keychron K8 Pro", ShortDescription = "Mekaniskt tangentbord", DetailedDescription = "Trådlöst mekaniskt tangentbord, hot-swappable", CategoryId = 3, SupplierId = 3, Stock = 32, Price = 1799m, IsActive = true },
                new Product { Name = "HyperX Cloud III", ShortDescription = "Gaming headset", DetailedDescription = "7.1 surroundljud, bekväma öronkuddar", CategoryId = 3, SupplierId = 3, Stock = 48, Price = 1299m, IsActive = true },
                new Product { Name = "Logitech C920 Pro", ShortDescription = "Webbkamera", DetailedDescription = "Full HD 1080p webbkamera med autofokus", CategoryId = 3, SupplierId = 3, Stock = 65, Price = 899m, IsActive = true, IsSaleItem = true },
                new Product { Name = "Blue Yeti USB-mikrofon", ShortDescription = "Kondensatormikrofon", DetailedDescription = "Professionell USB-mikrofon för streaming/podcasts", CategoryId = 3, SupplierId = 3, Stock = 22, Price = 1599m, IsActive = true },
                new Product { Name = "Anker PowerExpand Hub", ShortDescription = "USB-C Hub 11-i-1", DetailedDescription = "Thunderbolt dock med 3x USB, HDMI, Ethernet", CategoryId = 3, SupplierId = 3, Stock = 38, Price = 799m, IsActive = true },
                new Product { Name = "Trust GXT 278 XXL", ShortDescription = "Gaming musmatta", DetailedDescription = "Stor gaming musmatta 930x300mm, anti-slip", CategoryId = 3, SupplierId = 3, Stock = 75, Price = 299m, IsActive = true },

                // Bildskärmar (6 products)
                new Product { Name = "Dell UltraSharp U2723DE", ShortDescription = "27\" 4K skärm", DetailedDescription = "27\" IPS 4K (3840x2160), USB-C 90W, 99% sRGB", CategoryId = 4, SupplierId = 4, Stock = 18, Price = 6499m, IsActive = true },
                new Product { Name = "ASUS ROG Swift PG279QM", ShortDescription = "27\" 240Hz gaming", DetailedDescription = "27\" QHD (2560x1440), 240Hz, G-SYNC, IPS", CategoryId = 4, SupplierId = 4, Stock = 12, Price = 7999m, IsActive = true, IsSelectedItem = true },
                new Product { Name = "Samsung Odyssey G9", ShortDescription = "49\" ultrawide", DetailedDescription = "49\" QLED 5120x1440, 240Hz, 1000R kurva", CategoryId = 4, SupplierId = 4, Stock = 6, Price = 14999m, IsActive = true },
                new Product { Name = "LG 27UP850-W", ShortDescription = "27\" 4K USB-C", DetailedDescription = "27\" IPS 4K, HDR400, USB-C 96W, ergonomisk", CategoryId = 4, SupplierId = 4, Stock = 25, Price = 5499m, IsActive = true, IsSaleItem = true },
                new Product { Name = "BenQ PD3220U", ShortDescription = "32\" 4K designer", DetailedDescription = "32\" IPS 4K, 99% Adobe RGB, USB-C, hotkey puck", CategoryId = 4, SupplierId = 4, Stock = 10, Price = 9999m, IsActive = true },
                new Product { Name = "AOC 24G2U", ShortDescription = "24\" budget gaming", DetailedDescription = "24\" IPS Full HD, 144Hz, FreeSync, justerbar", CategoryId = 4, SupplierId = 4, Stock = 42, Price = 1999m, IsActive = true },
            };

            db.Products.AddRange(products);
            await db.SaveChangesAsync();
        }

        // Seed Users with Swedish names and addresses
        if (!db.Customers.Any())
        {
            var swedishCities = new[] { "Stockholm", "Göteborg", "Malmö", "Uppsala", "Västerås", "Örebro", "Linköping", "Helsingborg", "Jönköping", "Norrköping" };
            var streets = new[] { "Storgatan", "Kungsgatan", "Drottninggatan", "Vasagatan", "Sveavägen", "Birger Jarlsgatan", "Hamngatan", "Östergatan", "Norra vägen", "Södra vägen" };

            var users = new List<(string FirstName, string LastName, string Username)>
            {
                ("Erik", "Andersson", "erik.a"),
                ("Anna", "Johansson", "anna.j"),
                ("Lars", "Karlsson", "lars.k"),
                ("Maria", "Nilsson", "maria.n"),
                ("Per", "Eriksson", "per.e"),
                ("Karin", "Larsson", "karin.l"),
                ("Johan", "Olsson", "johan.o"),
                ("Emma", "Persson", "emma.p"),
                ("Anders", "Svensson", "anders.s"),
                ("Linda", "Gustafsson", "linda.g"),
                ("Mikael", "Pettersson", "mikael.p"),
                ("Sara", "Jonsson", "sara.j"),
                ("Stefan", "Jansson", "stefan.ja"),
                ("Eva", "Hansson", "eva.h"),
                ("Magnus", "Bengtsson", "magnus.b"),
                ("Annika", "Jönsson", "annika.jo"),
                ("Henrik", "Lindberg", "henrik.l"),
                ("Camilla", "Jakobsson", "camilla.j"),
                ("Fredrik", "Magnusson", "fredrik.m"),
                ("Malin", "Berg", "malin.b")
            };

            var random = new Random(42); // Fixed seed for reproducibility
            var customers = new List<Customer>();

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                var customer = new Customer
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Password = "password123", // Simple password as requested
                    Email = $"{user.Username}@example.se",
                    Phone = $"070-{random.Next(100, 999)}{random.Next(10, 99)}{random.Next(10, 99)}",
                    Age = random.Next(20, 65),
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
        }

        Console.WriteLine("✓ Database seeded successfully!");
        Console.WriteLine($"✓ Categories: {db.Categories.Count()}");
        Console.WriteLine($"✓ Suppliers: {db.Suppliers.Count()}");
        Console.WriteLine($"✓ Products: {db.Products.Count()}");
        Console.WriteLine($"✓ Customers: {db.Customers.Count()}");
    }
}