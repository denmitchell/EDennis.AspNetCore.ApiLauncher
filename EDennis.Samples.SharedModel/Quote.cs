using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.Samples.SharedModel {
    public class Quote {
        public int Id { get; set; }
        public string Text { get; set; }
        public Time LastAccessed { get; set; }
    }
}
