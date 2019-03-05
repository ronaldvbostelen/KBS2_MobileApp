namespace KBS2.WijkagentApp.DataModels.EventArgs
{
    class ReportDetailsEventArgs
    {
        public ReportDetails ReportDetails { get; set; }

        public ReportDetailsEventArgs(ReportDetails reportDetails)
        {
            ReportDetails = reportDetails;
        }
    }
}
