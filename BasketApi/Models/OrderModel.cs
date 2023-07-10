namespace BasketApi.Models
{
    public class OrderModel
    {
        /// <summary>
        /// The user email.
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// The total amount of products.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Collection of OrderLines.
        /// </summary>
        public List<OrderLineModel> OrderLines { get; set; } = new List<OrderLineModel>();
    }
}
