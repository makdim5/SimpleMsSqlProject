using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CourseWorkDB.util
{
    public class Message
    {
        public static async void Show(string msg, XamlRoot xamlRoot, string title = "Message")
        {

            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = msg,
                PrimaryButtonText = "ОК",
                XamlRoot = xamlRoot,
            };

            await dialog.ShowAsync();
        }
    }
}
