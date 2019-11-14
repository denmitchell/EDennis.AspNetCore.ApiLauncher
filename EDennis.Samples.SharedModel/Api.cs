using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.Samples.SharedModel {
    public class Api {
        public string ProjectName { get; set; }
        public string Host { get; set; }
        public string Scheme { get; set; }
        public int? HttpsPort { get; set; }
        public int? HttpPort { get; set; }
        public decimal Version { get; set; }

        public int? MainPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpsPort;
                else
                    return HttpPort;
            }
        }

        public int? AltPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpPort;
                else
                    return HttpsPort;
            }
        }

        public string MainAddress { 
            get {
                if (Scheme == null)
                    return null;
                return $"{Scheme}://{Host}:{MainPort}";
            } 
        }

        public string AltAddress {
            get {
                if (Scheme == null)
                    return null;
                return $"{(Scheme.Equals("https",StringComparison.OrdinalIgnoreCase) ? "http": "https")}://{Host}:{AltPort}";
            }
        }


        public string[] Urls {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
                    return new string[] { MainAddress };
                else
                    return new string[] { MainAddress, AltAddress};
            }
        }
    }
}
