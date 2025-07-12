using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class ProductModelProductDescription
{
    public int ProductModelID { get; set; }

    public int ProductDescriptionID { get; set; }

    public string Culture { get; set; } = null!;

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ProductDescription ProductDescription { get; set; } = null!;

    public virtual ProductModel ProductModel { get; set; } = null!;
}
