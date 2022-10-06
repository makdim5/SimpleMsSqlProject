using App1.util;
using App1.view;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class GroupsPage : Page
    {
        public List<GoodGroups> groups;
        public GoodGroups man;
        public GroupsPage()
        {
            this.InitializeComponent();

            GoodGroupsManager.UpdateDBCollection();
            groups = GoodGroupsManager.goodsGroups;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddGroupPage), null, new DrillInNavigationTransitionInfo());

        }

        private void View_ItemClick(object sender, ItemClickEventArgs e)
        {
            var manl = (GoodGroups)e.ClickedItem;

            man = new GoodGroups();
            man.Name = manl.Name;
            man.Id = manl.Id;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (man != null)
            {
                this.Frame.Navigate(typeof(AddGroupPage), man, new DrillInNavigationTransitionInfo());
            }
            else
            {

                Message.Show(" Выберите для изменения категории!", this.Frame.XamlRoot, "Предупреждение!");
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (man != null)
            {
                ContentDialog deleteDialog = new ContentDialog()
                {
                    Title = "Подтверждение действия",
                    Content = "Вы действительно хотите удалить категорию?",
                    PrimaryButtonText = "ОК",
                    SecondaryButtonText = "Отмена"
                };

                ContentDialogResult result = await deleteDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    GoodGroupsManager.Remove(man.Id);
                    this.Frame.Navigate(typeof(SalesmenPage));
                    Message.Show("Запись удалена!!!", this.Frame.XamlRoot, "Сообщение");
                }
                else if (result == ContentDialogResult.Secondary)
                {

                }
            }
        }
    }
}
