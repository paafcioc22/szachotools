using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace App2.Model
{
    public class AkcjeGrupy<K,T> : ObservableCollection<AkcjeNagElem>
    {
        public string Grupa { get; set; }
        public Color kolor { get; set; }
        public K Key { get; private set; }
        private int SumaStan { get; set; }
        private int SumaSkan { get; set; }

        private string _skan;
        public string SumaStanVsSKan
        {
            get
            {
                _skan = String.Format("{0}/{1}", SumaSkan, SumaStan);
                return _skan;
            }
            set { _skan = value; }

        }
        public AkcjeGrupy(K key, IEnumerable<AkcjeNagElem> items)
        {
            Key = key;
            foreach (var item in items)
            {
                this.Items.Add(item);
                SumaStan += item.TwrStan;
                SumaSkan += item.TwrSkan;
            }
        }

        public AkcjeGrupy(string tytul, Color color)
        {
            Grupa = tytul;
            kolor = color;
        }
    }
}
