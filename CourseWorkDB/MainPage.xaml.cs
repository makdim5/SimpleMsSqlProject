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
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace CourseWorkDB
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            myFrame.Navigate(typeof(SalesPage));
            pageName.Text = "Продажи";
        }

        private void gamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void myListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageGoods.IsSelected)
            {
                myFrame.Navigate(typeof(GoodsPage), null, new DrillInNavigationTransitionInfo());
                pageName.Text = "Товары";
            }
            else if (pageGroups.IsSelected)
            {
                // Play the drill in animation
                myFrame.Navigate(typeof(GroupsPage), null, new DrillInNavigationTransitionInfo());
                pageName.Text = "Категории товаров";
            }
            else if (pageSalesmen.IsSelected)
            {
                // Play the drill in animation
                myFrame.Navigate(typeof(SalesmenPage), null, new DrillInNavigationTransitionInfo());
                pageName.Text = "Продавцы";
            }
            else if (pageSales.IsSelected)
            {
                // Play the drill in animation
                myFrame.Navigate(typeof(SalesPage), null, new DrillInNavigationTransitionInfo());
                pageName.Text = "Продажи";
            }
            
        }
    }
}
