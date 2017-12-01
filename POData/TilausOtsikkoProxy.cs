using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class TilausOtsikkoProxy : TilausOtsikko
    {
        List<TilausRivi> _tilausRivit;
        bool TilausRivitHaettu = false;

        public TilausRiviRepository TilausRiviRepository { get; set; }

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
