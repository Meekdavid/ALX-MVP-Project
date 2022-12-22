using System.ComponentModel.DataAnnotations;

namespace catalogueService.requestETresponse
{
    public class locationRequest
    {
        public string province { get; set; }
        public string city { get; set; }
        public string street { get; set; }
    }
}
