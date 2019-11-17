using EDennis.Samples.SharedModel;
using System;

namespace EDennis.Samples.SharedModel {
    public class Location {
        public int Id { get; set; }

        public string City  { get; set; }

        public string State;

        public Time LastAccessed { get; set; }
    }
}
