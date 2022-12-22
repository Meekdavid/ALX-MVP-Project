namespace catalogueService.requestETresponse
{
    public class productRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int categoryId { get; set; }
    }
}
