using System.Collections.Generic;

namespace eTickets.Models
{
    public class UserEmailOptions
    {
        public List<string>  ToEmails { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public List<KeyValuePair<string,string>> placeholders { get; set; }
    }
}
