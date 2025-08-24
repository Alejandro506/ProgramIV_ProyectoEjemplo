using System; 

using System.ComponentModel.DataAnnotations; 

 

namespace myFirstProject.MyControllerModels 

{ 

    public partial class SalesOrderDetail 

    { 

        [Required] 

        public int SalesOrderID { get; set; } 

 

        [Required] 

        public int SalesOrderDetailID { get; set; } 

 

        [Required] 

        [Range(0, 500, ErrorMessage = "Order quantity cannot be negative")] 

        public short OrderQty { get; set; } 

 

        [Required] 

        public int ProductID { get; set; } 

 

        [Required] 

        [Range(0, 1000000, ErrorMessage = "Unit Price cannot be negative")] 

        public decimal UnitPrice { get; set; } 

 

        [Required] 

        [Range(0, 1000000, ErrorMessage = "Unit Price Discount cannot be negative")] 

        public decimal UnitPriceDiscount { get; set; } 

         [Range(0, double.MaxValue, ErrorMessage = "Line Total cannot be negative")] 

        public decimal LineTotal { get; set; } 


    } 

} 