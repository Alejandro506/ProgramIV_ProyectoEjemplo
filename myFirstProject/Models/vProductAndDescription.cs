using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class vProductAndDescription
{
    public int ProductID { get; set; }

    public string Name { get; set; } = null!;

    public string ProductModel { get; set; } = null!;

    public string Culture { get; set; } = null!;

    public string Description { get; set; } = null!;
}
