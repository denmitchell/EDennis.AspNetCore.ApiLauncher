using EDennis.Samples.SharedModel;
using System;

namespace EDennis.Samples.SharedModel {
    public class Name {
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public Time LastAccessed { get; set; }
    }
}
