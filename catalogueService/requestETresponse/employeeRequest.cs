using System;

namespace catalogueService.requestETresponse
{
    public class employeeRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime hiredDate { get; set; }
        public int locationId { get; set; }
        public int jobId { get; set; }
    }
}
