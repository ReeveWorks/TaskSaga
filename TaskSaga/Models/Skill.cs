using System;

namespace TaskSaga.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Level { get; set; }
        public int Exp { get; set; }
        public int ExpCap { get; set; }

    }
}
