using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using CourseWorkDB.util;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace CourseWorkDB
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BtnPage : Page
    {
        Good good;
        public BtnPage()
        {
            this.InitializeComponent();
        }

        

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GoodsPage), null, new DrillInNavigationTransitionInfo());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            good= e.Parameter as Good;

            this.nameBlock.Text = good.Name;
            im.Source = WindowShower.SetImage(good.Image);
            this.textBox.Text =good.Discribtion;
           
        }

        private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteDialog = new ContentDialog()
            {
                Title = "Подтверждение действия",
                Content = "Вы действительно хотите удалить запись товара?",
                PrimaryButtonText = "ОК",
                SecondaryButtonText = "Отмена",
                XamlRoot = this.Frame.XamlRoot
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                GoodManager.Remove(good.Id);
                this.Frame.Navigate(typeof(GoodsPage), null, new DrillInNavigationTransitionInfo());
                Message.Show("Запись удалена!!!", this.Frame.XamlRoot, "Сообщение");
            }
            else if (result == ContentDialogResult.Secondary)
            {
                
            }
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage), good, new DrillInNavigationTransitionInfo());
        }
    }
}
