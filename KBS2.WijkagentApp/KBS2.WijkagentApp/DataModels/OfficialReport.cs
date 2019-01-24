using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class OfficialReport
    {
        public Guid officialReportId { get; set; }
        public Guid reporterId { get; set; }
        public Guid? reportId { get; set; }
        public string observation { get; set; }
        public TimeSpan? time { get; set; }
        public string location { get; set; }

        public virtual Officer reporter { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual SoundRecord SoundRecord { get; set; }
    }
}