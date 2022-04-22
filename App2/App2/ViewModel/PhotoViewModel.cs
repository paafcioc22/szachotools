using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModel
{
    public class PhotoViewModel : INotifyPropertyChanged
    {

        readonly IList<Photo> source;
        public ObservableCollection<Photo> Photos { get; private set; }
        public ICommand DeleteCommand => new Command<Photo>(RemovePhoto);
        public ICommand FavoriteCommand => new Command<Photo>(FavoriteMonkey);


        public PhotoViewModel()
        {
            source = new List<Photo>();

            Photos = new ObservableCollection<Photo>(source);
 
        }


        void FavoriteMonkey(Photo photo)
        {
             
        }

        void RemovePhoto(Photo photo)
        {
            if (Photos.Contains(photo))
            {
                Photos.Remove(photo);
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
