namespace BasketApi.Models
{
    public class BasketModel
    {
        /// <summary>
        /// Basket GUID identificator
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Basket's order's collection.
        /// </summary>
        public List<OrderLineModel> OrderLines { get; set; } = new List<OrderLineModel>();
    }
}
