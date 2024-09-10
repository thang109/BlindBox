namespace BlindBoxWebsite.ViewModels
{
    public class VnPayRequestModel
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public int TicketID { get; set; }
    }
}
