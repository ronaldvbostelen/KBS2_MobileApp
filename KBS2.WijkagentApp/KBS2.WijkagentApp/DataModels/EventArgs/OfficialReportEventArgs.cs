namespace KBS2.WijkagentApp.DataModels.EventArgs
{
    class OfficialReportEventArgs
    {
        public OfficialReport OfficialReport { get; set; }

        public OfficialReportEventArgs(OfficialReport officialReport) { OfficialReport = officialReport; }
    }
}
