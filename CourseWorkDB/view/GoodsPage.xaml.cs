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


namespace CourseWorkDB
{
    public sealed partial class GoodsPage : Page
    {

        private List<Good> goods;
        public GoodsPage()
        {
            this.InitializeComponent();
            GoodManager.UpdateDBCollection();
            goods = GoodManager.goods;

            
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var good = (Good)e.ClickedItem;
            this.Frame.Navigate(typeof(BtnPage) ,good, new DrillInNavigationTransitionInfo());
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddPage), new DrillInNavigationTransitionInfo());
        }
    }
}
