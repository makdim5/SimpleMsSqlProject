using CourseWorkDB.model;
using CourseWorkDB.util;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace CourseWorkDB.view
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddSalesmanPage : Page
    {
        private const int EDIT_MODE = -10;
        private const int ADD_MODE = -20;
        private int mode;
        Salesmen salesmen;
        public AddSalesmanPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Salesmen)
            {

                mode = EDIT_MODE;
                salesmen = e.Parameter as Salesmen;

                this.nameField.Text = salesmen.Name;

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
                    SalesmenManager.Add(nameField.Text);
                    Message.Show("Продавец добавлен!", this.Frame.XamlRoot, "Поздравляем");
                   
                }
                else if (mode == EDIT_MODE)
                {

                    Salesmen newSalesman = new Salesmen();
                    newSalesman.Name = nameField.Text;
                    
                    SalesmenManager.Edit(salesmen, newSalesman);
                    Message.Show("Продавец обновлен!", this.Frame.XamlRoot, "Поздравляем");
                    
                }

                this.Frame.Navigate(typeof(SalesmenPage), null, new DrillInNavigationTransitionInfo());

            }
            else
            {
                Message.Show("Для добавления продавца не введено имя!", this.Frame.XamlRoot, "Предупреждение");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SalesmenPage), null, new DrillInNavigationTransitionInfo());
        }
    }
}
