using System;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Socials
    {
        public Guid socialsId { get; set; }
        public Guid personId { get; set; }
        public string profile { get; set; }

        public virtual Person person { get; set; }
    }
}