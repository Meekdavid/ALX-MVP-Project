using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class locationModel
    {
        public int locationId { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string street { get; set; }
    }
}
