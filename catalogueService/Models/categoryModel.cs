using catalogueService.Database;
using catalogueService.Database.DBsets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace catalogueService.Models
{
    public class categoryModel
    {
        public int categoryId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
