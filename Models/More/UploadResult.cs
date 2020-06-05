using System.Collections.Generic;

namespace Models.More
{
    public class UploadResult
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public List<string> Images { get; set; }
    }
}
