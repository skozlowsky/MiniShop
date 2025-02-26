# Mini Shop

## Zadanie

Chcemy dodać kolejny zasób do naszego projektu w .NET Aspire w celu wsparcia naszego wewnętrznego zespołu.

Dodaj projekt ShopManagement do modelu aplikacji i upewnij się, że uruchamia się razem z naszą rozproszoną aplikacją. Klient HTTP w projekcie ShopManagement powinien używać service discovery do konfiguracji adresu projektu Inventory.

Działanie aplikacji możesz sprawdzić poprzez odpytanie GET **/api/stock**

<details>
<summary>Rozwiązanie:</summary>

1. Dodaj wspracie dla orkiestratora .NET Aspire do projektu ShopManagement.

2. Dodaj referencję do projektu ShopManagement w modelu aplikacji.

```
var management = builder.AddProject<Projects.ShopManagement>("management")
    .WithReference(inventory)
    .WaitFor(inventory);
```

3. Użyj nazwy zasobu projektu Inventory w konfiguracji adresu bazowego klienta HTTP.

```
builder.Services.AddHttpClient("inventoryClient",
    static client => client.BaseAddress = new("https+http://inventory"));
```

</details>

## Test data
```sql

-- CATALOG DB

INSERT INTO "Categories" ("Id", "Name", "Description") VALUES
                                                           ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'Electronics', 'Electronic devices and accessories'),
                                                           ('f47ac10b-58cc-4372-a567-0e02b2c3d480', 'Books', 'Physical and digital books'),
                                                           ('f47ac10b-58cc-4372-a567-0e02b2c3d481', 'Home & Garden', 'Home improvement and garden supplies'),
                                                           ('f47ac10b-58cc-4372-a567-0e02b2c3d482', 'Sports', 'Sports equipment and accessories');

-- Products
INSERT INTO "Products" ("Id", "Name", "Description", "Price", "IsActive", "CategoryId", "Tags") VALUES
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d479', 'Smartphone X', 'Latest generation smartphone with advanced features', 999.99, true, 'f47ac10b-58cc-4372-a567-0e02b2c3d479', ARRAY['electronics', 'mobile', 'smartphone']),
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d480', 'Laptop Pro', 'Professional laptop for demanding users', 1499.99, true, 'f47ac10b-58cc-4372-a567-0e02b2c3d479', ARRAY['electronics', 'computer', 'laptop']),
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d481', 'Python Programming', 'Comprehensive guide to Python programming', 49.99, true, 'f47ac10b-58cc-4372-a567-0e02b2c3d480', ARRAY['programming', 'education', 'software']),
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d482', 'Garden Tools Set', 'Complete set of essential garden tools', 129.99, true, 'f47ac10b-58cc-4372-a567-0e02b2c3d481', ARRAY['garden', 'tools', 'outdoor']),
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d483', 'Tennis Racket Pro', 'Professional grade tennis racket', 199.99, true, 'f47ac10b-58cc-4372-a567-0e02b2c3d482', ARRAY['sports', 'tennis', 'equipment']),
                                                                                                    ('a47ac10b-58cc-4372-a567-0e02b2c3d484', 'Tablet Y', 'Compact tablet for entertainment', 299.99, false, 'f47ac10b-58cc-4372-a567-0e02b2c3d479', ARRAY['electronics', 'tablet', 'mobile']);

-- INVENTORY DB

-- Items
INSERT INTO "Items" ("Id", "Sku", "Name", "AvailableQuantity", "ReservedQuantity", "Price", "CreatedAt", "UpdatedAt") VALUES
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d479', 'PHONE-001', 'Smartphone X - 128GB Black', 50, 5, 999.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00'),
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d480', 'LAPTOP-001', 'Laptop Pro - 16GB/512GB', 20, 2, 1499.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00'),
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d481', 'BOOK-001', 'Python Programming - Hardcover', 100, 10, 49.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00'),
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d482', 'GARDEN-001', 'Garden Tools Set - Basic', 30, 0, 129.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00'),
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d483', 'TENNIS-001', 'Tennis Racket Pro - Adult', 25, 2, 199.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00'),
                                                                                                                          ('a47ac10b-58cc-4372-a567-0e02b2c3d484', 'TABLET-001', 'Tablet Y - 64GB', 0, 0, 299.99, '2024-02-18 10:00:00+00', '2024-02-18 10:00:00+00');

```