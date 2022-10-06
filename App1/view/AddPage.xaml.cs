using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using App1.util;
using Windows.UI.Xaml.Input;

namespace App1
{

    public sealed partial class AddPage : Page
    {
        private const int EDIT_MODE = -10;
        private const int ADD_MODE = -20;
        private int mode;
        Good good;
        public AddPage()
        {
         
            this.InitializeComponent();

            GoodGroupsManager.UpdateDBCollection();
            foreach (var item in GoodGroupsManager.GetListOfNames())
            {
                comboBox.Items.Add(item);
            }


        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Good)
            {

                mode = EDIT_MODE;
                good = e.Parameter as Good;

                this.goodNameField.Text = good.Name;
                if (good.Image != "")
                {
                    urlField.Text = good.Image;
                    im.Source = WindowShower.SetImage(good.Image);
                }
                this.discribtionField.Text = good.Discribtion;
                
                comboBox.SelectedValue = GoodGroupsManager.GetGroupName(good.GroupId);

            }
            else
            {

                mode = ADD_MODE;
            }

        }


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mode == EDIT_MODE) {
                this.Frame.Navigate(typeof(BtnPage), good, new DrillInNavigationTransitionInfo());
            } else if (mode == ADD_MODE)
            {
                this.Frame.Navigate(typeof(GoodsPage), null, new DrillInNavigationTransitionInfo());
            }

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (goodNameField.Text != "" && comboBox.SelectedItem != null) {

                try
                {
                    im.Source = WindowShower.SetImage(urlField.Text);

                    if (mode == ADD_MODE)
                    {
                        GoodManager.Add(GoodGroupsManager.GetGroupId(comboBox.SelectedItem.ToString()),
                            goodNameField.Text, discribtionField.Text, urlField.Text);
                        Message.Show("Товар добавлен!", this.Frame.XamlRoot, "Поздравляем");
                        this.Frame.Navigate(typeof(GoodsPage), null, new DrillInNavigationTransitionInfo());
                    }
                    else if (mode == EDIT_MODE)
                    {

                        Good newGoodVersion = new Good();
                        newGoodVersion.Name = goodNameField.Text;
                        newGoodVersion.Discribtion = discribtionField.Text;
                        newGoodVersion.Image = urlField.Text;
                        newGoodVersion.GroupId = GoodGroupsManager.GetGroupId(comboBox.SelectedItem.ToString());

                        GoodManager.Edit(good, newGoodVersion);
                        Message.Show("Товар обновлен!", this.Frame.XamlRoot, "Поздравляем");
                        this.Frame.Navigate(typeof(BtnPage), newGoodVersion, new DrillInNavigationTransitionInfo());

                        
                    }
                }
                catch (System.Exception )
                {
                    
                    Message.Show("Неверный формат URl!", this.Frame.XamlRoot, "Предупреждение");

                }
            }
            else
            {
                Message.Show("Для добавления товара не введены обязательные данные (название товара, группа)", this.Frame.XamlRoot, "Предупреждение");

            }
        }

        private void urlField_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void urlField_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                try
                {
                    if (urlField.Text.Contains("data:image") || urlField.Text.Contains("https://"))
                    {
                        im.Source = WindowShower.SetImage(urlField.Text);
                    }
                    else if (urlField.Text == "")
                    {
                        im.Source = WindowShower.SetImage(urlField.Text);
                    }
                    else 
                    {
                        urlField.Text = "";
                        Message.Show("Неверный формат URl!", this.Frame.XamlRoot, "Предупреждение");
                    }

                }
                catch (System.Exception ex)
                {

                    Message.Show("Неверный формат URl!", this.Frame.XamlRoot, "Предупреждение");

                }
            }
               
        }
    }
}
