namespace BasketApi.Models
{
    /// <summary>
    /// Order's DTO. 
    /// TODO:: Refactor to use Product DTO if it makes sense.
    /// </summary>
    public class OrderLineModel
    {
        /// <summary>
        /// The Product's identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The Product's Name.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// The Product's Price per unit.
        /// </summary>
        public double? ProductUnitPrice { get; set; }

        /// <summary>
        /// The Product's size.
        /// </summary>
        public string ProductSize { get; set; } = string.Empty;

        /// <summary>
        /// The Product's amount.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The Order's total price.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
