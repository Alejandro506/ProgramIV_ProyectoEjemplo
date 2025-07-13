using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Models;

namespace myFirstProject.Data;

public partial class AdventureWorksContext : DbContext
{
    public AdventureWorksContext()
    {
    }

    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
    }

    public DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }

    public virtual DbSet<ProductModel> ProductModels { get; set; }

    public virtual DbSet<ProductModelProductDescription> ProductModelProductDescriptions { get; set; }

    public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

    public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }

    public virtual DbSet<vGetAllCategory> vGetAllCategories { get; set; }

    public virtual DbSet<vProductAndDescription> vProductAndDescriptions { get; set; }

    public virtual DbSet<vProductModelCatalogDescription> vProductModelCatalogDescriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PrograAvanzada202502User15");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressID).HasName("PK_Address_AddressID");

            entity.ToTable("Address", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_Address_rowguid").IsUnique();

            entity.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.StateProvince, e.PostalCode, e.CountryRegion }, "IX_Address_AddressLine1_AddressLine2_City_StateProvince_PostalCode_CountryRegion");

            entity.HasIndex(e => e.StateProvince, "IX_Address_StateProvince");

            entity.Property(e => e.AddressLine1).HasMaxLength(60);
            entity.Property(e => e.AddressLine2).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.CountryRegion).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PostalCode).HasMaxLength(15);
            entity.Property(e => e.StateProvince).HasMaxLength(50);
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID).HasName("PK_Customer_CustomerID");

            entity.ToTable("Customer", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_Customer_rowguid").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "IX_Customer_EmailAddress");

            entity.Property(e => e.CompanyName).HasMaxLength(128);
            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasMaxLength(25);
            entity.Property(e => e.SalesPerson).HasMaxLength(256);
            entity.Property(e => e.Suffix).HasMaxLength(10);
            entity.Property(e => e.Title).HasMaxLength(8);
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => new { e.CustomerID, e.AddressID }).HasName("PK_CustomerAddress_CustomerID_AddressID");

            entity.ToTable("CustomerAddress", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_CustomerAddress_rowguid").IsUnique();

            entity.Property(e => e.AddressType).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Address).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.AddressID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK_Product_ProductID");

            entity.ToTable("Product", "SalesLT");

            entity.HasIndex(e => e.Name, "AK_Product_Name").IsUnique();

            entity.HasIndex(e => e.ProductNumber, "AK_Product_ProductNumber").IsUnique();

            entity.HasIndex(e => e.rowguid, "AK_Product_rowguid").IsUnique();

            entity.Property(e => e.Color).HasMaxLength(15);
            entity.Property(e => e.DiscontinuedDate).HasColumnType("datetime");
            entity.Property(e => e.ListPrice).HasColumnType("money");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductNumber).HasMaxLength(25);
            entity.Property(e => e.SellEndDate).HasColumnType("datetime");
            entity.Property(e => e.SellStartDate).HasColumnType("datetime");
            entity.Property(e => e.Size).HasMaxLength(5);
            entity.Property(e => e.StandardCost).HasColumnType("money");
            entity.Property(e => e.ThumbnailPhotoFileName).HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products).HasForeignKey(d => d.ProductCategoryID);

            entity.HasOne(d => d.ProductModel).WithMany(p => p.Products).HasForeignKey(d => d.ProductModelID);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryID).HasName("PK_ProductCategory_ProductCategoryID");

            entity.ToTable("ProductCategory", "SalesLT");

            entity.HasIndex(e => e.Name, "AK_ProductCategory_Name").IsUnique();

            entity.HasIndex(e => e.rowguid, "AK_ProductCategory_rowguid").IsUnique();

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ParentProductCategory).WithMany(p => p.InverseParentProductCategory)
                .HasForeignKey(d => d.ParentProductCategoryID)
                .HasConstraintName("FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");
        });

        modelBuilder.Entity<ProductDescription>(entity =>
        {
            entity.HasKey(e => e.ProductDescriptionID).HasName("PK_ProductDescription_ProductDescriptionID");

            entity.ToTable("ProductDescription", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_ProductDescription_rowguid").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ProductModel>(entity =>
        {
            entity.HasKey(e => e.ProductModelID).HasName("PK_ProductModel_ProductModelID");

            entity.ToTable("ProductModel", "SalesLT");

            entity.HasIndex(e => e.Name, "AK_ProductModel_Name").IsUnique();

            entity.HasIndex(e => e.rowguid, "AK_ProductModel_rowguid").IsUnique();

            entity.Property(e => e.CatalogDescription).HasColumnType("xml");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ProductModelProductDescription>(entity =>
        {
            entity.HasKey(e => new { e.ProductModelID, e.ProductDescriptionID, e.Culture }).HasName("PK_ProductModelProductDescription_ProductModelID_ProductDescriptionID_Culture");

            entity.ToTable("ProductModelProductDescription", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_ProductModelProductDescription_rowguid").IsUnique();

            entity.Property(e => e.Culture)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ProductDescription).WithMany(p => p.ProductModelProductDescriptions)
                .HasForeignKey(d => d.ProductDescriptionID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductModel).WithMany(p => p.ProductModelProductDescriptions)
                .HasForeignKey(d => d.ProductModelID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SalesOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.SalesOrderID, e.SalesOrderDetailID }).HasName("PK_SalesOrderDetail_SalesOrderID_SalesOrderDetailID");

            entity.ToTable("SalesOrderDetail", "SalesLT");

            entity.HasIndex(e => e.rowguid, "AK_SalesOrderDetail_rowguid").IsUnique();

            entity.HasIndex(e => e.ProductID, "IX_SalesOrderDetail_ProductID");

            entity.Property(e => e.SalesOrderDetailID).ValueGeneratedOnAdd();
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("(isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0)))", false)
                .HasColumnType("numeric(38, 6)");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("money");
            entity.Property(e => e.UnitPriceDiscount).HasColumnType("money");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderDetails).HasForeignKey(d => d.SalesOrderID);
        });

        modelBuilder.Entity<SalesOrderHeader>(entity =>
        {
            entity.HasKey(e => e.SalesOrderID).HasName("PK_SalesOrderHeader_SalesOrderID");

            entity.ToTable("SalesOrderHeader", "SalesLT");

            entity.HasIndex(e => e.SalesOrderNumber, "AK_SalesOrderHeader_SalesOrderNumber").IsUnique();

            entity.HasIndex(e => e.rowguid, "AK_SalesOrderHeader_rowguid").IsUnique();

            entity.HasIndex(e => e.CustomerID, "IX_SalesOrderHeader_CustomerID");

            entity.Property(e => e.SalesOrderID).HasDefaultValueSql("(NEXT VALUE FOR [SalesLT].[SalesOrderNumber])");
            entity.Property(e => e.AccountNumber).HasMaxLength(15);
            entity.Property(e => e.CreditCardApprovalCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OnlineOrderFlag).HasDefaultValue(true);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(25);
            entity.Property(e => e.SalesOrderNumber)
                .HasMaxLength(25)
                .HasComputedColumnSql("(isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID],(0)),N'*** ERROR ***'))", false);
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.ShipMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.TaxAmt).HasColumnType("money");
            entity.Property(e => e.TotalDue)
                .HasComputedColumnSql("(isnull(([SubTotal]+[TaxAmt])+[Freight],(0)))", false)
                .HasColumnType("money");
            entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.BillToAddress).WithMany(p => p.SalesOrderHeaderBillToAddresses)
                .HasForeignKey(d => d.BillToAddressID)
                .HasConstraintName("FK_SalesOrderHeader_Address_BillTo_AddressID");

            entity.HasOne(d => d.Customer).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ShipToAddress).WithMany(p => p.SalesOrderHeaderShipToAddresses)
                .HasForeignKey(d => d.ShipToAddressID)
                .HasConstraintName("FK_SalesOrderHeader_Address_ShipTo_AddressID");
        });

        modelBuilder.Entity<vGetAllCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vGetAllCategories", "SalesLT");

            entity.Property(e => e.ParentProductCategoryName).HasMaxLength(50);
            entity.Property(e => e.ProductCategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<vProductAndDescription>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vProductAndDescription", "SalesLT");

            entity.Property(e => e.Culture)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductModel).HasMaxLength(50);
        });

        modelBuilder.Entity<vProductModelCatalogDescription>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vProductModelCatalogDescription", "SalesLT");

            entity.Property(e => e.Color).HasMaxLength(256);
            entity.Property(e => e.Copyright).HasMaxLength(30);
            entity.Property(e => e.Crankset).HasMaxLength(256);
            entity.Property(e => e.MaintenanceDescription).HasMaxLength(256);
            entity.Property(e => e.Material).HasMaxLength(256);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NoOfYears).HasMaxLength(256);
            entity.Property(e => e.Pedal).HasMaxLength(256);
            entity.Property(e => e.PictureAngle).HasMaxLength(256);
            entity.Property(e => e.PictureSize).HasMaxLength(256);
            entity.Property(e => e.ProductLine).HasMaxLength(256);
            entity.Property(e => e.ProductModelID).ValueGeneratedOnAdd();
            entity.Property(e => e.ProductPhotoID).HasMaxLength(256);
            entity.Property(e => e.ProductURL).HasMaxLength(256);
            entity.Property(e => e.RiderExperience).HasMaxLength(1024);
            entity.Property(e => e.Saddle).HasMaxLength(256);
            entity.Property(e => e.Style).HasMaxLength(256);
            entity.Property(e => e.WarrantyDescription).HasMaxLength(256);
            entity.Property(e => e.WarrantyPeriod).HasMaxLength(256);
            entity.Property(e => e.Wheel).HasMaxLength(256);
        });
        modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
