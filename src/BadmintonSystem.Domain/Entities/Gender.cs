using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Gender : AuditableEntity<Guid>
{
    // Ở ngoài không thể gọi lại sử dụng
    // Can't set field
    // Tự tạo ra chính nó
    public string Name { get; private set; }

    // Tạo ra Entities
    // Command call Gender.CreateGender(request here)
    public static Gender CreateGender(Guid id, string name)
    {
        return new Gender(id, name);
    }

    // Can Config format class request
    // Private ==> Public để test
    // Unprocessed
    public Gender(Guid id, string name)
    {
        // Handler exception here
        // Example validator
        Id = id;
        Name = name;
    }

    #region ==================================== EXAMPLE DOMAIN DRIVEN DESIGN ==================================

    // This is  Example Domain Driven Desgin
    // It can Handler check and set new data
    // Here, Example Product
    // var product = product.ReduceStock(quantity)
    // This is example

    // ============================= EXAMPLE USE FUNC =====================================
    //public class OrderService
    //{
    //    public void PlaceOrder(Product product, int quantity)
    //    {
    //        try
    //        {
    //            // Reduce stock when placing an order
    //            product.ReduceStock(quantity);

    //            // Proceed with other order logic (e.g., saving order details, calculating total price)
    //            Console.WriteLine($"Order placed successfully for {quantity} units of {product.Name}. " +
    //                              $"Remaining stock: {product.StockQuantity}");
    //        }
    //        catch (InvalidOperationException ex)
    //        {
    //            // Handle exception when stock is insufficient
    //            Console.WriteLine(ex.Message);
    //        }
    //    }
    //}

    // ======================================= THIS IS PRODUCT =========================================
    //public class Product
    //{
    //    public Guid Id { get; private set; }
    //    public string Name { get; private set; }
    //    public decimal Price { get; private set; }
    //    public int StockQuantity { get; private set; }

    //    public Product(Guid id, string name, decimal price, int stockQuantity)
    //    {
    //        Id = id;
    //        Name = name;
    //        Price = price;
    //        StockQuantity = stockQuantity;
    //    }

    //    // Reduce stock when order is placed
    //    public void ReduceStock(int quantity)
    //    {
    //        if (quantity > StockQuantity)
    //            throw new InvalidOperationException("Insufficient stock.");

    //        StockQuantity -= quantity;
    //    }

    //    // Add stock when restocking
    //    public void AddStock(int quantity)
    //    {
    //        if (quantity <= 0)
    //            throw new ArgumentException("Quantity must be greater than zero.");

    //        StockQuantity += quantity;
    //    }
    //}

    // ==================================== THIS IS ORDER ==============================
    //public class Order
    //{
    //    public Guid Id { get; private set; }
    //    public DateTime OrderDate { get; private set; }
    //    public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

    //    public Order(Guid id)
    //    {
    //        Id = id;
    //        OrderDate = DateTime.UtcNow;
    //    }

    //    public void AddItem(Product product, int quantity)
    //    {
    //        if (product == null)
    //            throw new ArgumentNullException(nameof(product));

    //        if (quantity <= 0)
    //            throw new ArgumentException("Quantity must be greater than zero.");

    //        // Add product to order and reduce stock
    //        var orderItem = new OrderItem(product, quantity);
    //        Items.Add(orderItem);

    //        // Handle stock logic in Product entity
    //        product.ReduceStock(quantity);
    //    }

    //    public decimal GetTotalPrice()
    //    {
    //        return Items.Sum(item => item.GetTotalPrice());
    //    }
    //}

    //public class OrderItem
    //{
    //    public Product Product { get; private set; }
    //    public int Quantity { get; private set; }

    //    public OrderItem(Product product, int quantity)
    //    {
    //        Product = product;
    //        Quantity = quantity;
    //    }

    //    public decimal GetTotalPrice()
    //    {
    //        return Product.Price * Quantity;
    //    }
    //}

    #endregion
}
