using Xamarin.Forms.Maps;

namespace Wijkagent_App.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public class Message
    {
        public string Label { get; set; }
        public string Content { get; set; }
        public Priority Priority { get; set; }
        public Pin Pin { get; set; }

        public Message(string label, string content, Priority priority, Position position)
        {
            Label = label;
            Content = content;
            Priority = priority;
            Pin = new Pin
            {
                Type = PinTypeChooser(priority),
                Position = position,
                Label = label,
                Address = content
            };
        }


        private PinType PinTypeChooser(Priority priority)
        {
            switch (priority)
            {
                case Priority.Low:
                    return PinType.Generic;
                case Priority.Medium:
                    return PinType.Place;
                case Priority.High:
                    return PinType.SavedPin;
                default:
                    return PinType.Generic;
            }
        }
    }
}