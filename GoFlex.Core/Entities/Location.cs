namespace GoFlex.Core.Entities
{
    public class Location : Entity<int>
    {
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
