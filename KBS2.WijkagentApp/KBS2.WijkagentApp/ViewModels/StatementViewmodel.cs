﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Android.App;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Application = Xamarin.Forms.Application;

namespace KBS2.WijkagentApp.ViewModels
{
    class StatementViewmodel : BaseViewModel
    {
        private string statement;
        private string tempStatement;
        private ReportDetails reportDetails;

        public Person Verbalisant { get; }
        public string Statement{ get { return statement;} set{ if (value != statement) { statement = value; NotifyPropertyChanged(); UpdateCommands(); } } }

        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => Delete(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));

        public StatementViewmodel(Person verbalisant)
        {
            Verbalisant = verbalisant;
            reportDetails = GetReportDetails(verbalisant);
            tempStatement = Statement = reportDetails.Statement ?? string.Empty;
        }

        private ReportDetails GetReportDetails(Person verbalisant)
        {
            return 
                (from  reportDetail in Constants.ReportDetails
                 where reportDetail.PersonId.Equals(verbalisant.PersonId)
                 select reportDetail).First();
        }


        private async void Cancel()
        {
            if (Statement != tempStatement)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result) SaveStatement();
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void SaveStatement()
        {
            reportDetails.Statement = tempStatement = Statement;
            UpdateCommands();
        }

        private bool DeleteStatement()
        {
            Statement = tempStatement = reportDetails.Statement = string.Empty;
            UpdateCommands();
            return true;
        }

        private async void Delete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Verklaring verwijderen?", "Ja", "Nee");
            if (result)
            {
                if (DeleteStatement())
                {
                    await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");
                }
            };
        }

        private void Save() => SaveStatement();

        private bool CanSave() => !Statement.Equals(tempStatement);

        private bool CanDelete() => !string.IsNullOrEmpty(reportDetails.Statement);

        private void UpdateCommands()
        {
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged();
        }
    }
}
