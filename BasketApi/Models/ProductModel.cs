namespace BasketApi.Models
{
    /// <summary>
    /// Product entity DTO to map retrieved Products.
    /// Follows IMPACT's API Productschema.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Product's instance ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product's instance Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Product's instance Price.
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// Product's instance Size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Product's instance review Stars (I GUESS).
        /// </summary>
        public int Stars { get; set; } 
    }
}
