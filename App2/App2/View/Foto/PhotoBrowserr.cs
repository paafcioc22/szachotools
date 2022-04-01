using Stormlion.PhotoBrowser;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace App2.View.Foto
{
    public class PhotoBrowserr: ContentPage
    {        

        public PhotoBrowserr(List<Photo> photos)
        {
            new PhotoBrowser
            {
                Photos = photos,
                ActionButtonPressed = (index) =>
                {
                    Debug.WriteLine($"Clicked {index}");
                    PhotoBrowser.Close();
                },
                EnableGrid = true,                
             
            }.Show();
            
        }
    }
}
