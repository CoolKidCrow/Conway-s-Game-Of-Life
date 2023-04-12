using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Conway_s_Game_Of_Life.Model
{
    public class ButtonModel : INotifyPropertyChanged
    {
        private int _value;
        private int _index;

        public int Value
        {
            get => _value;
            set 
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public int Index
        {
            get => _index;
            set 
            {
                _index = value;
                OnPropertyChanged();
            }
        }



        public ButtonModel(int value, int index)
        {
            Value = value;
            Index = index;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
