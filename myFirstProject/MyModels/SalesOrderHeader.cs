using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace myFirstProject.Models;

public partial class SalesOrderHeader
{
    [NotMapped]
    public string FreightPct
    {
        get
        {
            if (SubTotal == 0)
            {
                return "0.00%"; // Avoid division by zero 
            }
            decimal ratio = TaxAmt / SubTotal;
            decimal percentage = Math.Round(ratio * 100, 2); // convert to percentage and round to 2 decimals 
            return $"{percentage:0.00}%"; // format with 2 decimals and add %         
        }
    }

}
