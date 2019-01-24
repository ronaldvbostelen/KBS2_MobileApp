using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class SoundRecord
    {
        public Guid officialReportId { get; set; }
        public string URL { get; set; }

        public virtual OfficialReport officialReport { get; set; }
    }
}