using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class AsiakasProxy : Asiakas
    {
        List<TilausOtsikko> _tilaukset;
        bool TilauksetHaettu = false;

        public TilausOtsikkoRepository TilausOtsikkoRepository { get; set; }

        public override List<TilausOtsikko> Tilaukset
        {
            get
            {
                if (!TilauksetHaettu) {
                    _tilaukset = TilausOtsikkoRepository.HaeAsiakkaanKaikki(Id);
                    TilauksetHaettu = true;
                }
                return (_tilaukset);
            }
            set => base.Tilaukset = value;
        }

        public AsiakasProxy(string id, string nimi) 
            : base(id, nimi) {

        }
    }
}
