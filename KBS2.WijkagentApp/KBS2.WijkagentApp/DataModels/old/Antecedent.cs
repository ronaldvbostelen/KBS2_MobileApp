namespace KBS2.WijkagentApp.DataModels.old
{
    class Antecedent : BaseDataModel
    {
        private string antecedentId;
        private string personId;
        private string type;
        private string verdict;
        private string crime;
        private string description;


        public string AntecedentId
        {
            get { return antecedentId; }
            set
            {
                if (value != antecedentId)
                {
                    antecedentId = value;
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


        public string Type
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


        public string Verdict
        {
            get { return verdict; }
            set
            {
                if (value != verdict)
                {
                    verdict = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Crime
        {
            get { return crime; }
            set
            {
                if (value != crime)
                {
                    crime = value;
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
    }
}