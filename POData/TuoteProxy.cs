using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class TuoteProxy : Tuote
    {
        List<TilausRivi> _tilausRivit;
        bool TilausRivitHaettu = false;

        public TilausRiviRepository TilausRiviRepository { get; set; }

        public override List<TilausRivi> TilausRivit
        {
            get
            {
                if (!TilausRivitHaettu) {
                    _tilausRivit = TilausRiviRepository.HaeTuotteenKaikki(Id);
                    TilausRivitHaettu = true;
                }
                return (_tilausRivit);
            }
            set => base.TilausRivit = value;
        }

        public TuoteProxy(int id, string nimi) 
            : base(id, nimi) {

        }
    }
}
