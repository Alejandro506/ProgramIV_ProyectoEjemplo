using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class CustomerAddress
{
    public int CustomerID { get; set; }

    public int AddressID { get; set; }

    public string AddressType { get; set; } = null!;

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
