using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class ReportDetails
    {
        public Guid reportId { get; set; }
        public Guid personId { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string statement { get; set; }
        public int? isHeard { get; set; }

        public virtual Person person { get; set; }
        public virtual Report report { get; set; }
    }
}