using Wijkagent_App.DataModels.Enums;
using Xamarin.Forms.Maps;

namespace Wijkagent_App.Models
{
    public class Message
    {
        public Priority Priority { get; set; }
        public Pin Pin { get; set; }

        public Message(string label, string content, Priority priority, Position position)
        {
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