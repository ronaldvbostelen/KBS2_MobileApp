﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    public class ReportDetails : BaseDataModel
    {
        private string reportId;
        private string officialReportId;
        private string personId;
        private char type;
        private string description;
        private string statement;
        private bool isHeard;

        public string ReportId
        {
            get { return reportId; }
            set
            {
                if (value != reportId)
                {
                    reportId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string OfficialReportId
        {
            get { return officialReportId; }
            set
            {
                if (value != officialReportId)
                {
                    officialReportId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PersonId
        {
            get { return personId; }
            set
            {
                if (value != personId)
                {
                    personId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public char Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Statement
        {
            get { return statement; }
            set
            {
                if (value != statement)
                {
                    statement = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsHeard
        {
            get { return isHeard;}
            set
            {
                if (value != isHeard)
                {
                    isHeard = value;
                    NotifyPropertyChanged();
                }
            } }
    }
}