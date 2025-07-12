using System;
using System.Collections.Generic;

namespace myFirstProject.Models;

public partial class SalesOrderHeader
{
    public int SalesOrderID { get; set; }

    public byte RevisionNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public byte Status { get; set; }

    public bool OnlineOrderFlag { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public string? PurchaseOrderNumber { get; set; }

    public string? AccountNumber { get; set; }

    public int CustomerID { get; set; }

    public int? ShipToAddressID { get; set; }

    public int? BillToAddressID { get; set; }

    public string ShipMethod { get; set; } = null!;

    public string? CreditCardApprovalCode { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }

    public string? Comment { get; set; }

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual Address? BillToAddress { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();

    public virtual Address? ShipToAddress { get; set; }
}
