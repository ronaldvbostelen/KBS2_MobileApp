using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Extensions;
using KBS2.WijkagentApp.Managers;
using KBS2.WijkagentApp.Models.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Models.DataControllers
{
    public class DataController
    {
        private readonly string apiUrl = "https://wijkagent.azurewebsites.net/api";

        private MobileServiceClient Client;

        public DataController()
        {
            Client = new MobileServiceClient(apiUrl);
        }

        public IMobileServiceTable<Address> AddressTable => Client.GetTable<Address>();
        public IMobileServiceTable<Antecedent> AntecedentTable => Client.GetTable<Antecedent>();
        public IMobileServiceTable<Emergency> EmergencyTable => Client.GetTable<Emergency>();
        public IMobileServiceTable<Officer> OfficerTable => Client.GetTable<Officer>();
        public IMobileServiceTable<OfficialReport> OfficialeReportTable => Client.GetTable<OfficialReport>();
        public IMobileServiceTable<Person> PersonTable => Client.GetTable<Person>();
        public IMobileServiceTable<Picture> PictureTable => Client.GetTable<Picture>();
        public IMobileServiceTable<PushMessage> PushMessageTable => Client.GetTable<PushMessage>();
        public IMobileServiceTable<Report> ReportTable => Client.GetTable<Report>();
        public IMobileServiceTable<ReportDetails> ReportDetailsTable => Client.GetTable<ReportDetails>();
        public IMobileServiceTable<SocialMessage> SocialMessageTable => Client.GetTable<SocialMessage>();
        public IMobileServiceTable<Socials> SocialsTable => Client.GetTable<Socials>();
        public IMobileServiceTable<SoundRecord> SoundRecordTable => Client.GetTable<SoundRecord>();

        public async Task<HttpResponseMessage> CheckOfficerCredentialsAsync(Officer officer)
        {
            //first hash before we send it over the ether
            officer.Password = new PasswordManager().GenerateHash(officer.Password);

            var httpContent = new StringContent(JsonConvert.SerializeObject(officer), Encoding.UTF8, "application/json");

            //try catch because notfound (404) will throw exception
            try
            {
                return await Client.InvokeApiAsync("/login/", httpContent, HttpMethod.Post, new Dictionary<string, string>(), new Dictionary<string, string>());
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);
                return e.Response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ObservableCollection<Report>> FetchReportsAsync()
        {
            try
            {
                var reportList = await App.DataController.ReportTable.ToListAsync();

                // setting ID (!important!) & adding to ReportsCollection
                reportList.ForEach(x => x.Id = x.ReportId);

                return reportList.ListToObservableCollection();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets fout tijdens het ophalen van de reportgegevens. Probeer opnieuw.", "OK");
                return new ObservableCollection<Report>();
            }
        }

        public async Task<ObservableCollection<ReportDetails>> FetchReportDetailsAsync(Guid reportId)
        {
            string lookupUri = $"/api/tables/ReportDetails/{reportId}";

            try
            {
                return await Client.InvokeApiAsync<ObservableCollection<ReportDetails>>(lookupUri, HttpMethod.Get, new Dictionary<string, string>(), CancellationToken.None);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);
                return new ObservableCollection<ReportDetails>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new ObservableCollection<ReportDetails>();
            }
        }

            /* this shitty code is needed because async processing screws up this framework (if we dont initialise a new instance
             * we get some illogical exception.
             */
        public async Task<T> InsertIntoAsync<T>(T entryObject) where T : IDatabaseObject
        {
            Client = new MobileServiceClient(apiUrl);

            try
            {
                await Client.GetTable<T>().InsertAsync(entryObject);
                return entryObject;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public async Task<T> UpdateSetAsync<T>(T entryObject) where T : IDatabaseObject
        {
            Client = new MobileServiceClient(apiUrl);

            try
            {
                await Client.GetTable<T>().UpdateAsync(entryObject);
                return entryObject;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}
