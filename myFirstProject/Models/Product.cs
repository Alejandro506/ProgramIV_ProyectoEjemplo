using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public string Name { get; set; } = null!;

    public string ProductNumber { get; set; } = null!;

    public string? Color { get; set; }

    public decimal StandardCost { get; set; }

    public decimal ListPrice { get; set; }

    public string? Size { get; set; }

    public decimal? Weight { get; set; }

    public int? ProductCategoryID { get; set; }

    public int? ProductModelID { get; set; }

    public DateTime SellStartDate { get; set; }

    public DateTime? SellEndDate { get; set; }

    public DateTime? DiscontinuedDate { get; set; }

    public byte[]? ThumbNailPhoto { get; set; }

    public string? ThumbnailPhotoFileName { get; set; }

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }

    public virtual ProductModel? ProductModel { get; set; }

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
}
