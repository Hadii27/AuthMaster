namespace AuthMaster.Model
{
    public class BlockList
    {
        public int id { get; set; }
        public string userId { get; set; }

        public string Blocked { get; set; }

        public DateTime dateTime { get; set; }

        public string Reason { get; set; }
    }
}
