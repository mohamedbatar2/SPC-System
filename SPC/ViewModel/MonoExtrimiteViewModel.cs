using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SPC.Models;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class MonoExtrimiteViewModel
    {
        public EnregComplet enregComplet { get; set; }
        public Enreg enreg { get; set; }
        public EnregDetail enregDetail { get; set; }
        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }

        public MonoExtrimiteViewModel(string NMachine, string NSerie, string NMatricule) 
        {
            Init();
            enreg = new Enreg()
            {
                NoSerie = NSerie,
                NoMachine = NMachine,
                OperationNo = NMatricule
            };

            ItemsSourceD = new List<string>() { "D", "D-F"};
        }

        public MonoExtrimiteViewModel(Enreg enreg) 
        {
            Init();
            this.enreg = enreg;

            ItemsSourceD = new List<string>() { "S", "F"};
        }

        private void Init()
        {
            SaveCommand = new RelayCommand(SaveSerie, parm => true);
            enregDetail = new EnregDetail()
            {
                DateCreation = DateTime.Now,
            };
        }

        private void SaveSerie(object obj)
        {
            if (checkFull())
            {

                enregDetail.Nature = enregDetail.Nature.Equals("D") ? "Debut"
                    : enregDetail.Nature.Equals("D-F") ? "Debut-Fin"
                    : enregDetail.Nature.Equals("F") ? "Fin"
                    : enregDetail.Nature.Equals("S") ? "Sourvillence"
                    : null;

                EnregManager.InsertNew(enreg);

                enregDetail.IdEnrg = EnregManager.GetId(enreg.NoSerie);

                EnregDetailManager.InsertNew(enregDetail);
                RequestClose?.Invoke();
            }
            else MessageBox.Show("remplire tout les cas svp");
        }

        private bool checkFull()
        {
            List<string> enregProps= new List<string>{"Client", "Ref", "Section", "Connexion"
                , "Denudage",};
            List<string> enregDetailProps= new List<string>{"Repere", "Nature", "Quantite"
                ,"AH1", "AH2", "AH3", "FH1", "FH2", "FH3"
                ,"Traction1" ,"Traction2","Traction3","NoOutil", "AspectCnx",
            };
            foreach(var prop in enregProps)
            {
                var propInfo = typeof(Enreg).GetProperty(prop);
                if(propInfo != null)
                {
                    var value = propInfo.GetValue(enreg);

                    if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        MessageBox.Show("prop");
                        return false;
                    }
                }
            }
            foreach(var prop in enregDetailProps)
            {
                var propInfo = typeof(EnregDetail).GetProperty(prop);
                if(propInfo != null)
                {
                    var value = propInfo.GetValue(enregDetail);

                    if(value == null || (value is string  && string.IsNullOrEmpty((string)value)))
                    {
                        MessageBox.Show(prop);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}



