using System.Collections.Generic;

namespace POLuokat
{
    public class Asiakas
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public virtual List<TilausOtsikko> Tilaukset { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Asiakas() {
            Tilaukset = new List<TilausOtsikko>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyName"></param>
        public Asiakas(string id, string companyName) : this() {
            this.Id = id;
            this.CompanyName = companyName;
        }

        public override string ToString() {
            return ($"{Id} {CompanyName} ({Tilaukset.Count})");
        }
    }
}
