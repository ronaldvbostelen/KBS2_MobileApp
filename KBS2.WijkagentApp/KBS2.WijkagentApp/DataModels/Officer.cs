using System;
using System.Collections.Generic;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Officer
    {
        public Officer()
        {
            Emergency = new HashSet<Emergency>();
            OfficialReport = new HashSet<OfficialReport>();
            PushMessage = new HashSet<PushMessage>();
        }

        public Guid officerId { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }

        public virtual ICollection<Emergency> Emergency { get; set; }
        public virtual ICollection<OfficialReport> OfficialReport { get; set; }
        public virtual ICollection<PushMessage> PushMessage { get; set; }
    }
}