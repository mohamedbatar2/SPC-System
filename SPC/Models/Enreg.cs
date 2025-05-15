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
        private string client;

        public string Client
        {
            get { return client; }
            set {
                if (client != value)
                {
                    client = value;
                    CliAbsTester?.Invoke();
                }
            }
        }

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
        public decimal? HA { get; set; }
        public decimal? HI { get; set; }
        public decimal? Traction { get; set; }
        public decimal? LongueurD { get; set; }
        public decimal? LongueurD2 { get; set; }

        public decimal? Denudage { get; set; }
        public string Connexion { get; set; }

        public Action RefSizeTester { get; set; }
        public Action CliAbsTester { get; set; }

        public decimal? DenudageB { get; set; }
        public decimal? DenudageC { get; set; }
        public string ConnexionB { get; set; }
        public string ConnexionC { get; set; }
        public string NoOutilB { get; set; }
        public string NoOutilC { get; set; }

        public decimal? HA1C { get; set; }
        public decimal? HA2C { get; set; }
        public decimal? HA3C { get; set; }
        public decimal? HI1C { get; set; }
        public decimal? HI2C { get; set; }
        public decimal? HI3C { get; set; }
        public decimal? Traction1C { get; set; }
        public decimal? Traction2C { get; set; }
        public decimal? Traction3C { get; set; }
    }
}
