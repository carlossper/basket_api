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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product's instance Price.
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// Product's instance Size.
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Product's instance review Stars.
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// String representation of ProductModel.
        /// </summary>
        public override string ToString()
        {
            return $"Product: Id={Id}, Name={Name}, Price={Price}, Size={Size}, Stars={Stars}";
        }
    }
}
