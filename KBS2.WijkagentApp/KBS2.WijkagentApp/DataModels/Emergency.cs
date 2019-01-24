using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Emergency
    {
        public Guid emergencyId { get; set; }
        public Guid officerId { get; set; }
        public string location { get; set; }
        public TimeSpan? time { get; set; }
        public string description { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public string status { get; set; }

        public virtual Officer officer { get; set; }
    }
}