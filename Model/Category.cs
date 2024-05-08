namespace HSport.App.Model
#nullable disable
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List <Product> Products { get; set; }
    }
}
