using System;

namespace KBS2.WijkagentApp.DataModels
{
    class Tweet
    {
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public string Id { get; set; }

        public Tweet(string userName, DateTime createdAt, string text, string source, string id)
        {
            UserName = userName;
            CreatedAt = createdAt;
            Text = text;
            Source = source;
            Id = id;
        }
    }
}
