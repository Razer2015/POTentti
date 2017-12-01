using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class TilausOtsikkoProxy : TilausOtsikko
    {
        Asiakas _asiakas;
        List<TilausRivi> _tilausRivit;
        bool AsiakasHaettu = false;
        bool TilausRivitHaettu = false;

        public AsiakasRepository AsiakasRepository { get; set; }
        public TilausRiviRepository TilausRiviRepository { get; set; }

        public override Asiakas Asiakas
        {
            get
            {
                if (!AsiakasHaettu) {
                    _asiakas = AsiakasRepository.Hae(CustomerID);
                    AsiakasHaettu = true;
                }
                return (_asiakas);
            }
            set => base.Asiakas = value;
        }

        public override List<TilausRivi> TilausRivit
        {
            get
            {
                if (!TilausRivitHaettu) {
                    _tilausRivit = TilausRiviRepository.HaeTilauksenKaikki(Id);
                    TilausRivitHaettu = true;
                }
                return (_tilausRivit);
            }
            set => base.TilausRivit = value;
        }

        public TilausOtsikkoProxy(int id, string customerID) 
            : base(id, customerID) {

        }
    }
}
