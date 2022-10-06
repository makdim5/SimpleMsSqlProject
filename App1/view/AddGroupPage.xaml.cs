using App1.util;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.view
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddGroupPage : Page
    {
        private const int EDIT_MODE = -10;
        private const int ADD_MODE = -20;
        private int mode;
        GoodGroups group;
        public AddGroupPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is GoodGroups)
            {

                mode = EDIT_MODE;
                group = e.Parameter as GoodGroups;

                this.nameField.Text = group.Name;

            }
            else
            {

                mode = ADD_MODE;
            }

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (nameField.Text != "")
            {


                if (mode == ADD_MODE)
                {
                    GoodGroupsManager.Add(nameField.Text);
                    Message.Show("Категория добавлена!", this.Frame.XamlRoot, "Поздравляем");

                }
                else if (mode == EDIT_MODE)
                {

                    GoodGroups newSalesman = new GoodGroups();
                    newSalesman.Name = nameField.Text;

                    GoodGroupsManager.Edit(group, newSalesman);
                    Message.Show("Категория обновлена!", this.Frame.XamlRoot, "Поздравляем");

                }

                this.Frame.Navigate(typeof(GroupsPage), null, new DrillInNavigationTransitionInfo());

            }
            else
            {
                Message.Show("Для добавления категории не введено название!", this.Frame.XamlRoot, "Предупреждение");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsPage), null, new DrillInNavigationTransitionInfo());
        }
    }
}
