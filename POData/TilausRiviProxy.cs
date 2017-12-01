using POLuokat;

namespace POData
{
    public class TilausRiviProxy : TilausRivi
    {
        TilausOtsikko _tilaus;
        Tuote _tuote;
        bool TilausHaettu = false;
        bool TuoteHaettu = false;

        public TilausOtsikkoRepository TilausOtsikkoRepository { get; set; }
        public TuoteRepository TuoteRepository { get; set; }

        public override TilausOtsikko Tilaus
        {
            get
            {
                if (!TilausHaettu) {
                    _tilaus = TilausOtsikkoRepository.Hae(Id);
                    TilausHaettu = true;
                }
                return (_tilaus);
            }
            set => base.Tilaus = value;
        }

        public override Tuote Tuote
        {
            get
            {
                if (!TuoteHaettu) {
                    _tuote = TuoteRepository.Hae((int)ProductID);
                    TuoteHaettu = true;
                }
                return (_tuote);
            }
            set => base.Tuote = value;
        }

        public TilausRiviProxy(int id) 
            : base(id) {

        }
    }
}
