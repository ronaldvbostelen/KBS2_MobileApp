using System;
using System.Collections.ObjectModel;
using System.Linq;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models;
using KBS2.WijkagentApp.Models.DataProviders;


namespace KBS2.WijkagentApp.ViewModels
{
    public class StarViewModel : BaseViewModel
    {
        private ObservableCollection<Person> persons;
        public ObservableCollection<Person> Persons { get { return persons;} set { if (value != persons) { persons = value; NotifyPropertyChanged(); } } }

        public StarViewModel()
        {
            //insert into...
            App.WijkagentDb.Person.Add(new Person {PersonId = Guid.NewGuid(), FirstName = "jannes", LastName = "bakkersloot" });
            App.WijkagentDb.Officer.Add(new Officer {officerId = Guid.NewGuid(), passWord = "1234", userName = "jannes"});

            //update
            var persG = App.WijkagentDb.Person.Where(x => string.IsNullOrEmpty(x.Gender)).ToList();
            persG.ForEach(x => x.Gender = "A");
            
            //select.. from .... where...
            var person = App.WijkagentDb.Person.Where(x => x.FirstName.Equals("jannes")).ToList();

            //save all changes
            App.WijkagentDb.SaveChanges();

            Persons = new ObservableCollection<Person>(person);
            
        }
        
    }
}