namespace Hotel_Rooms.Models
{
    public class Rooms
    {
        public int id { get; set; }
        public string type { get; set; }
        public int roomTemp { get; set; }
        public int price { get; set; }
        public string available { get; set; }
        public DateOnly dateStart { get; set; }
        public DateOnly dateEnd { get; set; }
        public int roomNumber { get; set; }
    }
}
