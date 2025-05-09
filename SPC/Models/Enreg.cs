using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.Models
{
    public class Enreg
    {
        public int Id { get; set; }
        public string NoSerie { get; set; }
        public string NoMachine { get; set; }
        public string RessourceNo { get; set; }
        public string OperationNo { get; set; }
        public string Client { get; set; }
        private string _ref;
        public string Ref
        {
            get { return _ref; }
            set {
                if (_ref != value)
                {
                    _ref = value;
                    RefSizeTester?.Invoke();
                }
            }
        }
        public string NoEquipement { get; set; }
        public string UAP { get; set; }
        public string Section { get; set; }
        public string NoContact { get; set; }
        public string NoContact2 { get; set; }
        public string NoOutil { get; set; }
        public string NoOutil2 { get; set; }
        public decimal? AH { get; set; }
        public decimal? FH { get; set; }
        public decimal? Traction { get; set; }
        public decimal? LongueurD { get; set; }
        public decimal? LongueurD2 { get; set; }

        public decimal? Denudage { get; set; }
        public string Connexion { get; set; }

        public Action RefSizeTester { get; set; }
    }
}
