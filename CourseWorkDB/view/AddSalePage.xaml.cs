using CourseWorkDB.model;
using CourseWorkDB.util;
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

namespace CourseWorkDB.view
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddSalePage : Page
    {
        List<SalesContentViewRow> newSalesContents;
        bool goodsmanFlag = false;
        public AddSalePage()
        {
            this.InitializeComponent();
            SalesmenManager.UpdateDBCollection();
            foreach (var item in SalesmenManager.GetListOfNames())
            {
                menComboBox.Items.Add(item);
            }

            GoodManager.UpdateDBCollection();

            foreach (var item in GoodManager.GetListOfNames())
            {
                goodComboBox.Items.Add(item);
            }

            newSalesContents = new List<SalesContentViewRow>();
            CalculateIncome();
        }

        public void UpdateCompts()
        {
            int menCBIndex = menComboBox.SelectedIndex;
            int goodCBIndex = goodComboBox.SelectedIndex;
            menComboBox.Items.Clear();
            goodComboBox.Items.Clear();
            SalesmenManager.UpdateDBCollection();
            foreach (var item in SalesmenManager.GetListOfNames())
            {
                menComboBox.Items.Add(item);
            }

            GoodManager.UpdateDBCollection();


            foreach (var item in GoodManager.GetListOfNames())
            {
                goodComboBox.Items.Add(item);
            }

            if (goodsmanFlag)
            {
                newSalesContents = new List<SalesContentViewRow>();
                listView.Items.Clear();
                goodsmanFlag = false;
            }

            if (menCBIndex != -1 && menCBIndex < menComboBox.Items.Count)
                menComboBox.SelectedIndex = menCBIndex;

            if (goodCBIndex != -1 && goodCBIndex < goodComboBox.Items.Count)
                goodComboBox.SelectedIndex = goodCBIndex;
            CalculateIncome();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(SalesPage), null, new DrillInNavigationTransitionInfo());
           
        }

        private void menComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void View_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (listView.SelectedIndex != -1)
            {
                var selectedItem = newSalesContents[listView.SelectedIndex];
                goodComboBox.SelectedValue = selectedItem.name;
                priceNumBox.Value = (double)selectedItem.price;
                amountNumBox.Value = selectedItem.amount;
            }
        }

        private void addGoodBtn_Click(object sender, RoutedEventArgs e)
        {
            if (goodComboBox.SelectedValue != null && priceNumBox.Value > 0 && amountNumBox.Value > 0)
            {
                SalesContentViewRow item = new SalesContentViewRow((string)goodComboBox.SelectedValue,
                    (decimal)priceNumBox.Value,
                    (byte)amountNumBox.Value);

                int searchIndex = -1;

                for (int i = 0; i < newSalesContents.Count; i++)
                {

                    if (newSalesContents[i].name == item.name) {
                        searchIndex = i;
                        break;
                    }
                }

                if (searchIndex != -1)
                {
                    newSalesContents[searchIndex].amount = item.amount;
                    newSalesContents[searchIndex].price = item.price;

                    listView.Items[searchIndex] = ($"{newSalesContents[searchIndex].name}" +
                    $", цена: {newSalesContents[searchIndex].price} ," +
                    $" количество: {newSalesContents[searchIndex].amount}");
                } else
                {
                    newSalesContents.Add(item);
                    listView.Items.Add($"{item.name} , цена: {item.price} , количество: {item.amount}");
                }

            }
            CalculateIncome();
        }

        private void salemenBtn_Click(object sender, RoutedEventArgs e)
        {


            WindowShower.ShowSaleP<SalesmenPage>(this);
            
        }

        private async void goodBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteDialog = new ContentDialog()
            {
                Title = "Подтверждение действия",
                Content = "При обновлении списка товаров текущая продажа будет очищена! Продолжить?",
                PrimaryButtonText = "ОК",
                SecondaryButtonText = "Отмена"
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                WindowShower.ShowSaleP<GoodsPage>(this);
                goodsmanFlag = true;
            }
            else if (result == ContentDialogResult.Secondary)
            {

            }
           
        }

        private void delGoodBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex != -1)
            {
                var selectedItem = newSalesContents[listView.SelectedIndex];
                newSalesContents.Remove(selectedItem);
                goodComboBox.SelectedValue = "";
                priceNumBox.Value = 0;
                amountNumBox.Value = 1;

                listView.Items.Remove(listView.Items[listView.SelectedIndex]);

                CalculateIncome();
            }
            
        }

        private void changeGoodBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex != -1)
            {
                var selectedItem = newSalesContents[listView.SelectedIndex];
               

                selectedItem.name = goodComboBox.SelectedValue.ToString();
                selectedItem.price = (decimal)priceNumBox.Value;
                selectedItem.amount = (byte)amountNumBox.Value;



                listView.Items[listView.SelectedIndex] = ($"{selectedItem.name}" +
                    $", цена: {selectedItem.price} , количество: {selectedItem.amount}");

                CalculateIncome();
            }
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (menComboBox.SelectedValue != null && newSalesContents.Count > 0)
            {
                SalesManager.UpdateDBCollection();
                SalesManager.Add(SalesmenManager.GetSalesmenId(menComboBox.SelectedValue.ToString()));

                SalesContentManager.UpdateDBCollection();

                foreach (var item in newSalesContents)
                {
                    var stuff = new SalesContent();
                    stuff.price = item.price;
                    stuff.amount = item.amount;
                    stuff.goodId = GoodManager.GetGoodId(item.name);
                    stuff.saleId = SalesManager.lastAddedSale.Id;
                    SalesContentManager.Add(stuff);
                }

                Message.Show("Продажа успешно добавлена!", this.Frame.XamlRoot, "Поздравление!");

                this.Frame.Navigate(typeof(SalesPage), null, new DrillInNavigationTransitionInfo());
            } else
            {
                Message.Show("Вы не выбрали продавца и/или товар для продажи!", this.Frame.XamlRoot, "Предупреждение!");
            }
        }

        public void CalculateIncome() 
        {
            decimal income = 0;

            foreach (var item in newSalesContents)
            {
                income += item.price* item.amount;
            }

            incomeTextBlock.Text = $"Итого: {income}";
        }
    }
}
