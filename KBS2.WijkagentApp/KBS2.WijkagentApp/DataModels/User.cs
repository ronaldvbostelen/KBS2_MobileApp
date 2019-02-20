using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.DataModels
{
    public static class User
    {
        public static Officer Base { get; set; }
        public static Person Person { get; set; }

        public static Guid Id => Base.OfficerId;
        public static Guid? PersonId => Base.PersonId;
        public static string Name => Base.UserName;

        
        public static async Task<Person> FetchUserPersonRecord()
        {
            try
            {
                return await App.DataController.PersonTable.LookupAsync(PersonId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Ophalen gebruiker mislukt", "Probeer later opnieuw", "Ok");
                return new Person{FirstName = string.Empty, LastName = string.Empty};
            }
        }
    }
}