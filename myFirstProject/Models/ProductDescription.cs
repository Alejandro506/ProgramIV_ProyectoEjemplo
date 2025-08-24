using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class ProductDescription
{
    public int ProductDescriptionID { get; set; }

    public string Description { get; set; } = null!;

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; } = new List<ProductModelProductDescription>();
}
