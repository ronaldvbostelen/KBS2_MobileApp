using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Report
    {
        public Guid reportId { get; set; }
        public Guid reporterId { get; set; }
        public Guid? processedBy { get; set; }
        public string type { get; set; }
        public TimeSpan? time { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public int? priority { get; set; }
        public string comment { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }

        public virtual Person reporter { get; set; }
        public virtual ReportDetails ReportDetails { get; set; }
    }
}