using System;

namespace TaskSaga.Models
{
    public class Quest
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DayLimit { get; set; }
        public string TimeLimit { get; set; }
    }
}
