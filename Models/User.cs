namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }


        public virtual ICollection<Court> Courts { get; set; }
    }
}
