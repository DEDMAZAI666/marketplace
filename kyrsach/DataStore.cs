using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using kyrsach.Models;

namespace kyrsach
{
    public static class DataStore
    {
        private static readonly string connString = "server=localhost;port=3306;database=mango_db;user=root;password=12345;charset=utf8mb4;";

        public static User CurrentUser { get; set; } = null;

        public static bool TestConnection()
        {
            try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch { return false; }
        }

        public static User FindUser(string phone, string password)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT * FROM Users WHERE Phone = @p AND Password = @pass";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", phone);
                    cmd.Parameters.AddWithValue("@pass", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                Phone = reader.GetString("Phone"),
                                Password = reader.GetString("Password"),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "Не указан" : reader.GetString("Address"),
                                RegDate = reader.GetDateTime("RegDate"),
                                Role = reader.GetString("Role")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static void AddUser(User user)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "INSERT INTO Users (Phone, Password, Address, Role) VALUES (@p, @pass, @a, @r)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", user.Phone);
                    cmd.Parameters.AddWithValue("@pass", user.Password);
                    cmd.Parameters.AddWithValue("@a", user.Address ?? "Не указан");
                    cmd.Parameters.AddWithValue("@r", user.Role);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- ОБНОВЛЕНИЕ ТЕЛЕФОНА ---
        public static void UpdateUserPhone(int userId, string newPhone)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "UPDATE Users SET Phone = @p WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", newPhone);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetProducts(string status = null)
        {
            var list = new List<Product>();
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT * FROM Products";
                if (status != null) sql += " WHERE Status = @s";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (status != null) cmd.Parameters.AddWithValue("@s", status);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Product
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Category = reader.GetString("Category"),
                                Price = reader.GetDecimal("Price"),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl"),
                                Status = (ProductStatus)Enum.Parse(typeof(ProductStatus), reader.GetString("Status")),
                                OwnerId = reader.IsDBNull(reader.GetOrdinal("OwnerId")) ? 0 : reader.GetInt32("OwnerId")
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static void AddProduct(Product product)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "INSERT INTO Products (Name, Category, Price, ImageUrl, Status, OwnerId) VALUES (@n, @c, @p, @i, @s, @o)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@n", product.Name);
                    cmd.Parameters.AddWithValue("@c", product.Category);
                    cmd.Parameters.AddWithValue("@p", product.Price);
                    cmd.Parameters.AddWithValue("@i", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@s", product.Status.ToString());
                    cmd.Parameters.AddWithValue("@o", product.OwnerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProductStatus(int productId, ProductStatus status)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "UPDATE Products SET Status = @s WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@s", status.ToString());
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProduct(Product product)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "UPDATE Products SET Name = @n, Category = @c, Price = @p, ImageUrl = @i WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@n", product.Name);
                    cmd.Parameters.AddWithValue("@c", product.Category);
                    cmd.Parameters.AddWithValue("@p", product.Price);
                    cmd.Parameters.AddWithValue("@i", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@id", product.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteProduct(int productId)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "DELETE FROM Products WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void CreateOrder(int userId, decimal total, List<Product> cartProducts)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sqlOrder = "INSERT INTO Orders (UserId, TotalPrice) VALUES (@uid, @total); SELECT LAST_INSERT_ID();";
                int orderId = 0;
                using (var cmd = new MySqlCommand(sqlOrder, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@total", total);
                    orderId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (var p in cartProducts)
                {
                    string sqlItem = "INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, PriceAtPurchase) VALUES (@oid, @pid, @pname, 1, @price)";
                    using (var cmd = new MySqlCommand(sqlItem, conn))
                    {
                        cmd.Parameters.AddWithValue("@oid", orderId);
                        cmd.Parameters.AddWithValue("@pid", p.Id);
                        cmd.Parameters.AddWithValue("@pname", p.Name);
                        cmd.Parameters.AddWithValue("@price", p.Price);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<Order> GetUserOrders(int userId)
        {
            var orders = new List<Order>();
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sqlOrders = "SELECT * FROM Orders WHERE UserId = @uid ORDER BY OrderDate DESC";
                using (var cmd = new MySqlCommand(sqlOrders, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = reader.GetInt32("Id"),
                                UserId = reader.GetInt32("UserId"),
                                TotalPrice = reader.GetDecimal("TotalPrice"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Status = reader.GetString("Status")
                            });
                        }
                    }
                }

                foreach (var order in orders)
                {
                    string sqlItems = "SELECT * FROM OrderItems WHERE OrderId = @oid";
                    using (var cmd = new MySqlCommand(sqlItems, conn))
                    {
                        cmd.Parameters.AddWithValue("@oid", order.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                order.Items.Add(new OrderItem
                                {
                                    Id = reader.GetInt32("Id"),
                                    OrderId = reader.GetInt32("OrderId"),
                                    ProductId = reader.GetInt32("ProductId"),
                                    ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? "Товар удален" : reader.GetString("ProductName"),
                                    Quantity = reader.GetInt32("Quantity"),
                                    PriceAtPurchase = reader.GetDecimal("PriceAtPurchase")
                                });
                            }
                        }
                    }
                }
            }
            return orders;
        }
    }
}