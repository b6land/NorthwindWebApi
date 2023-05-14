using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace NorthwindWebApi.Models
{
    public partial class NorthwindContext : DbContext
    {
        IConfigurationRoot configuration;
        public NorthwindContext()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public virtual DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; } = null!;
        public virtual DbSet<CurrentProductList> CurrentProductLists { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; } = null!;
        public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; } = null!;
        public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; } = null!;
        public virtual DbSet<OrdersQry> OrdersQries { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; } = null!;
        public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; } = null!;
        public virtual DbSet<ProductsByCategory> ProductsByCategories { get; set; } = null!;
        public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<SalesByCategory> SalesByCategories { get; set; } = null!;
        public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;
        public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; } = null!;
        public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Territory> Territories { get; set; } = null!;
        public virtual DbSet<OrderCustomer> OrderCustomers { get; set; } = null!;
        public virtual DbSet<SalesByYear> SalesByYears { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 連線字串儲存在 appsettings.json 內
                var connectionString = configuration.GetConnectionString("NorthwindDatabase");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlphabeticalListOfProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Alphabetical list of products");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CategoryName, "CategoryName");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<CategorySalesFor1997>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Category Sales for 1997");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.CategorySales).HasColumnType("money");
            });

            modelBuilder.Entity<CurrentProductList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Current Product List");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(40);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.CompanyName, "CompanyName");

                entity.HasIndex(e => e.PostalCode, "PostalCode");

                entity.HasIndex(e => e.Region, "Region");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(5)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.ContactName).HasMaxLength(30);

                entity.Property(e => e.ContactTitle).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Fax).HasMaxLength(24);

                entity.Property(e => e.Phone).HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.HasMany(d => d.CustomerTypes)
                    .WithMany(p => p.Customers)
                    .UsingEntity<Dictionary<string, object>>(
                        "CustomerCustomerDemo",
                        l => l.HasOne<CustomerDemographic>().WithMany().HasForeignKey("CustomerTypeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerCustomerDemo"),
                        r => r.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CustomerCustomerDemo_Customers"),
                        j =>
                        {
                            j.HasKey("CustomerId", "CustomerTypeId").IsClustered(false);

                            j.ToTable("CustomerCustomerDemo");

                            j.IndexerProperty<string>("CustomerId").HasMaxLength(5).HasColumnName("CustomerID").IsFixedLength();

                            j.IndexerProperty<string>("CustomerTypeId").HasMaxLength(10).HasColumnName("CustomerTypeID").IsFixedLength();
                        });
            });

            modelBuilder.Entity<CustomerAndSuppliersByCity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Customer and Suppliers by City");

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.ContactName).HasMaxLength(30);

                entity.Property(e => e.Relationship)
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerDemographic>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeId)
                    .IsClustered(false);

                entity.Property(e => e.CustomerTypeId)
                    .HasMaxLength(10)
                    .HasColumnName("CustomerTypeID")
                    .IsFixedLength();

                entity.Property(e => e.CustomerDesc).HasColumnType("ntext");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.LastName, "LastName");

                entity.HasIndex(e => e.PostalCode, "PostalCode");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Extension).HasMaxLength(4);

                entity.Property(e => e.FirstName).HasMaxLength(10);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.HomePhone).HasMaxLength(24);

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.PhotoPath).HasMaxLength(255);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");

                entity.HasMany(d => d.Territories)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeTerritory",
                        l => l.HasOne<Territory>().WithMany().HasForeignKey("TerritoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeTerritories_Territories"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeTerritories_Employees"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "TerritoryId").IsClustered(false);

                            j.ToTable("EmployeeTerritories");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                            j.IndexerProperty<string>("TerritoryId").HasMaxLength(20).HasColumnName("TerritoryID");
                        });
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Invoices");

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(5)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.CustomerName).HasMaxLength(40);

                entity.Property(e => e.ExtendedPrice).HasColumnType("money");

                entity.Property(e => e.Freight).HasColumnType("money");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.Salesperson).HasMaxLength(31);

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.ShipperName).HasMaxLength(40);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "CustomerID");

                entity.HasIndex(e => e.CustomerId, "CustomersOrders");

                entity.HasIndex(e => e.EmployeeId, "EmployeeID");

                entity.HasIndex(e => e.EmployeeId, "EmployeesOrders");

                entity.HasIndex(e => e.OrderDate, "OrderDate");

                entity.HasIndex(e => e.ShipPostalCode, "ShipPostalCode");

                entity.HasIndex(e => e.ShippedDate, "ShippedDate");

                entity.HasIndex(e => e.ShipVia, "ShippersOrders");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(5)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("FK_Orders_Shippers");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_Order_Details");

                entity.ToTable("Order Details");

                entity.HasIndex(e => e.OrderId, "OrderID");

                entity.HasIndex(e => e.OrderId, "OrdersOrder_Details");

                entity.HasIndex(e => e.ProductId, "ProductID");

                entity.HasIndex(e => e.ProductId, "ProductsOrder_Details");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Products");
            });

            modelBuilder.Entity<OrderDetailsExtended>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Order Details Extended");

                entity.Property(e => e.ExtendedPrice).HasColumnType("money");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<OrderSubtotal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Order Subtotals");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Subtotal).HasColumnType("money");
            });

            modelBuilder.Entity<OrdersQry>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Orders Qry");

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(5)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Freight).HasColumnType("money");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

                entity.HasIndex(e => e.CategoryId, "CategoryID");

                entity.HasIndex(e => e.ProductName, "ProductName");

                entity.HasIndex(e => e.SupplierId, "SupplierID");

                entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);

                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<ProductSalesFor1997>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Product Sales for 1997");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.ProductSales).HasColumnType("money");
            });

            modelBuilder.Entity<ProductsAboveAveragePrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Products Above Average Price");

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<ProductsByCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Products by Category");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
            });

            modelBuilder.Entity<QuarterlyOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Quarterly Orders");

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(5)
                    .HasColumnName("CustomerID")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .IsClustered(false);

                entity.ToTable("Region");

                entity.Property(e => e.RegionId)
                    .ValueGeneratedNever()
                    .HasColumnName("RegionID");

                entity.Property(e => e.RegionDescription)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<SalesByCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Sales by Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(15);

                entity.Property(e => e.ProductName).HasMaxLength(40);

                entity.Property(e => e.ProductSales).HasColumnType("money");
            });

            modelBuilder.Entity<SalesTotalsByAmount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Sales Totals by Amount");

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.SaleAmount).HasColumnType("money");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.Phone).HasMaxLength(24);
            });

            modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Quarter");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.Subtotal).HasColumnType("money");
            });

            modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Year");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.Subtotal).HasColumnType("money");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasIndex(e => e.CompanyName, "CompanyName");

                entity.HasIndex(e => e.PostalCode, "PostalCode");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName).HasMaxLength(40);

                entity.Property(e => e.ContactName).HasMaxLength(30);

                entity.Property(e => e.ContactTitle).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Fax).HasMaxLength(24);

                entity.Property(e => e.HomePage).HasColumnType("ntext");

                entity.Property(e => e.Phone).HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);
            });

            modelBuilder.Entity<Territory>(entity =>
            {
                entity.HasKey(e => e.TerritoryId)
                    .IsClustered(false);

                entity.Property(e => e.TerritoryId)
                    .HasMaxLength(20)
                    .HasColumnName("TerritoryID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.TerritoryDescription)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Territories_Region");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// 查詢訂單的產品與客戶相關資料
        /// </summary>
        /// <param name="id"> 訂單 ID </param>
        /// <returns> 查詢結果 </returns>
        public async Task<List<OrderCustomer>> QueryOrderCustomer(int id)
        {
            List<OrderCustomer> result = await Task.Run(() => QueryOrderCustomerBySQL(id));
            return result;
        }

        /// <summary>
        /// 查詢訂單的產品與客戶相關資料
        /// </summary>
        /// <param name="start"> 起始日期 </param>
        /// <param name="end"> 結束日期 </param>
        /// <returns> 查詢結果 </returns>
        public async Task<List<SalesByYear>> QuerySalesByYear(DateTime start, DateTime end)
        {
            List<SalesByYear> result = await Task.Run(() => QuerySalesByYearSQL(start, end));
            return result;
        }

        /// <summary>
        /// 查詢特定分頁的訂單
        /// </summary>
        /// <param name="page"> 分頁編號 </param>
        /// <returns> 訂單清單 </returns>
        public async Task<List<Order>> QueryOrdersByPage(int page)
        {
            List<Order> result = await Task.Run(() => QueryOrdersByPageSQL(page));
            return result;
        }

        /// <summary>
        /// 產品搜尋 (使用 Dapper)
        /// </summary>
        /// <param name="keyword"> 關鍵字 </param>
        /// <returns> 產品列表 </returns>
        public async Task<List<Product>> QueryProducts(string keyword)
        {
            var connectionString = configuration.GetConnectionString("NorthwindDatabase");

            using (var conn = new SqlConnection(connectionString))
            {
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("Keyword", "%" + keyword + "%");
                var result = await Task.Run(() => conn.Query<Product>("SELECT * FROM Products WHERE ProductName LIKE @Keyword", dynamicParams));
                return result.ToList();
            }
        }

        /// <summary>
        /// 用 SQL 查詢訂單的產品與客戶相關資料
        /// </summary>
        /// <param name="id"> 訂單 ID </param>
        /// <returns> 多筆的產品與客戶相關資料 </returns>
        private List<OrderCustomer> QueryOrderCustomerBySQL(int id)
        {
            var OrderID = new SqlParameter("ID", id);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT O.OrderID, O.OrderDate, C.ContactName, C.Phone, OD.Quantity, OD.UnitPrice, P.ProductName ");
            sql.AppendLine("FROM Orders O");
            sql.AppendLine("INNER JOIN Customers C ON O.CustomerID = C.CustomerID");
            sql.AppendLine("INNER JOIN [Order Details] OD ON O.OrderID = OD.OrderID");
            sql.AppendLine("INNER JOIN Products P ON OD.ProductID = P.ProductID");
            sql.AppendLine($"WHERE O.OrderID = @ID");
            var orderCustomers = this.OrderCustomers.FromSqlRaw(sql.ToString(), OrderID).ToList();
            return orderCustomers;
        }

        /// <summary>
        /// 用 SQL 查詢特定日期間的訂單銷售額
        /// </summary>
        /// <remarks> SQL 改寫自 https://www.geeksengine.com/database/problem-solving/northwind-queries-part-1.php </remarks>
        /// <returns> 訂單銷售額清單 </returns>
        private List<SalesByYear> QuerySalesByYearSQL(DateTime start, DateTime end)
        {
            var startParam = new SqlParameter("StartDate", start.ToString("yyyy-MM-dd"));
            var endParam = new SqlParameter("EndDate", end.ToString("yyyy-MM-dd"));
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT DISTINCT a.ShippedDate AS ShippedDate, ");
            sql.AppendLine("    a.OrderID, b.Subtotal, year(a.ShippedDate) AS Year");
            sql.AppendLine("FROM Orders a");
            sql.AppendLine("INNER JOIN");
            sql.AppendLine("("); // 取得每個訂單的銷售額
            sql.AppendLine("    SELECT DISTINCT OrderID, ");
            sql.AppendLine("        CAST(sum(UnitPrice * Quantity * (1 - Discount)) AS VARCHAR) AS Subtotal");
            sql.AppendLine("    FROM [Order Details]");
            sql.AppendLine("    GROUP BY OrderID");
            sql.AppendLine(") b ON a.OrderID = b.OrderID");
            sql.AppendLine("WHERE a.ShippedDate IS NOT NULL"); // 訂單需在特定時間範圍內
            sql.AppendLine("    AND a.ShippedDate between @StartDate AND @EndDate");
            sql.AppendLine("ORDER BY a.ShippedDate;");
            var SalesByYearList = this.SalesByYears.FromSqlRaw(sql.ToString(), startParam, endParam).ToList();
            return SalesByYearList;
        }

        /// <summary>
        /// 用 SQL 查詢特定分頁的訂單 (由小到大)
        /// </summary>
        /// <param name="page"> 分頁編號 </param>
        /// <returns> 訂單清單 </returns>
        private List<Order> QueryOrdersByPageSQL(int page)
        {
            var pageParam = new SqlParameter("PageNo", page);
            var pageSizeParam = new SqlParameter("PageSize", 10);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM (");
            sql.AppendLine("    SELECT ROW_NUMBER() OVER (ORDER BY OrderId ASC) AS RowNum, ");
            sql.AppendLine("        OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, ");
            sql.AppendLine("        Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry");
            sql.AppendLine("    FROM Orders) O");
            sql.AppendLine("WHERE O.RowNum BETWEEN ((@PageNo - 1) * @PageSize + 1) AND (@PageNo * @PageSize)");

            var OrderList = this.Orders.FromSqlRaw(sql.ToString(), pageParam, pageSizeParam).ToList();
            return OrderList;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
