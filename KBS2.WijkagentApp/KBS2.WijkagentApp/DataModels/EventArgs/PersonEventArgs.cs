namespace KBS2.WijkagentApp.DataModels.EventArgs
{
    public class PersonEventArgs
    {
        public Person Person { get; set; }

        public PersonEventArgs(Person person)
        {
            Person = person;
        }
    }
}
