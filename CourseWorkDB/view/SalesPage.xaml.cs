using CourseWorkDB.model;
using CourseWorkDB.util;
using CourseWorkDB.view;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace CourseWorkDB
{
    public sealed partial class SalesPage : Page
    {
        Grid myGrid;
        TextBlock t;
        public List<SalesView> sview;
        public SalesView selectedSview;

        public List<SalesContentViewRow> rows;

        public SalesPage()
        {
            this.InitializeComponent();
            //SalesViewManager.UpdateDBCollection();
            //sview = SalesViewManager.sales;

            //SalesContentViewRowManager.UpdateDBCollection(10);
            //rows = SalesContentViewRowManager.context;

            //generalIncomeTextBlock.Text = $"Итого заработано: {SalesViewManager.GetGeneralIncome(sview)}";
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (dataGrid.SelectedIndex > -1) ? dataGrid.SelectedIndex : 0;
            SalesContentViewRowManager.UpdateDBCollection(sview[index].Id);
            rows = SalesContentViewRowManager.context;
            dataGridContent.ItemsSource = rows;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSalePage), null, new DrillInNavigationTransitionInfo());
            sview = SalesViewManager.sales;
            dataGrid.ItemsSource = sview;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var printHelper = new Microsoft.Toolkit.Uwp.Helpers.PrintHelper(tableGrid);

            dataGrid.SelectedIndex = -1;

            tableGrid.Children.Remove(dataGrid);

            //printHelper.AddFrameworkElementToPrint(dataGrid);

            printBtn.IsEnabled = false;

            grid.Children.Remove(generalIncomeTextBlock);


            myGrid = new Grid();

            for (int i = 0; i < 3; i++)
            {

                RowDefinition row = new RowDefinition();

                myGrid.RowDefinitions.Add(row);
            }
            t = new TextBlock();
            t.Text = "Отчет по продажам";
            t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            t.FontSize = 30;
            myGrid.Children.Add(t);
            Grid.SetRow(t, 0);
            myGrid.Children.Add(dataGrid);
            Grid.SetRow(dataGrid, 1);
            myGrid.Children.Add(generalIncomeTextBlock);
            Grid.SetRow(generalIncomeTextBlock, 2);

            printHelper.AddFrameworkElementToPrint(myGrid);

            printHelper.OnPrintSucceeded += PrintHelperAfterActions;
            printHelper.OnPrintFailed += PrintHelperAfterActions;
            printHelper.OnPrintCanceled += PrintHelperAfterActions;


            await printHelper.ShowPrintUIAsync("Печать");

        }

        private void PrintHelperAfterActions()
        {
            myGrid.Children.Remove(generalIncomeTextBlock);
            myGrid.Children.Remove(dataGrid);
            myGrid.Children.Remove(t);
            tableGrid.Children.Add(dataGrid);
            grid.Children.Add(generalIncomeTextBlock);
            Grid.SetRow(generalIncomeTextBlock, 1);
            printBtn.IsEnabled = true;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Creating a new document
            WordDocument document = new WordDocument();
            //Adding a new section to the document
            WSection section = document.AddSection() as WSection;
            //Set Margin of the section
            section.PageSetup.Margins.All = 72;
            //Set page size of the section
            section.PageSetup.PageSize = new SizeF(612, 792);

            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Calibri";
            style.CharacterFormat.FontSize = 11f;
            style.ParagraphFormat.BeforeSpacing = 0;
            style.ParagraphFormat.AfterSpacing = 8;
            style.ParagraphFormat.LineSpacing = 13.8f;

            style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
            style.ApplyBaseStyle("Normal");
            style.CharacterFormat.FontName = "Calibri Light";
            style.CharacterFormat.FontSize = 16f;
            style.CharacterFormat.TextColor = Color.FromArgb(46, 116, 181);
            style.ParagraphFormat.BeforeSpacing = 12;
            style.ParagraphFormat.AfterSpacing = 0;
            style.ParagraphFormat.Keep = true;
            style.ParagraphFormat.KeepFollow = true;
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;

            IWParagraph paragraph = section.AddParagraph();
            paragraph.ApplyStyle("Heading 1");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
            WTextRange textRange = paragraph.AppendText("Отчет по продажам") as WTextRange;
            textRange.CharacterFormat.FontSize = 18f;
            textRange.CharacterFormat.FontName = "Calibri";

            //Appends table
            IWTable table = section.AddTable();
            table.ResetCells(sview.Count + 1, 3);
            table.TableFormat.Borders.BorderType = BorderStyle.Wave;
            table.TableFormat.IsAutoResized = true;

            //Appends table headers
            paragraph = table[0, 0].AddParagraph();
            paragraph.ParagraphFormat.AfterSpacing = 0;
            paragraph.BreakCharacterFormat.FontSize = 12f;
            textRange = paragraph.AppendText("Дата продажи") as WTextRange;
            textRange.CharacterFormat.FontSize = 18f;
            textRange.CharacterFormat.FontName = "Calibri";

            paragraph = table[0, 1].AddParagraph();
            paragraph.ParagraphFormat.AfterSpacing = 0;
            paragraph.BreakCharacterFormat.FontSize = 12f;
            textRange = paragraph.AppendText("Продавец") as WTextRange;
            textRange.CharacterFormat.FontSize = 18f;
            textRange.CharacterFormat.FontName = "Calibri";

            paragraph = table[0, 2].AddParagraph();
            paragraph.ParagraphFormat.AfterSpacing = 0;
            paragraph.BreakCharacterFormat.FontSize = 12f;
            textRange = paragraph.AppendText("Общая сумма продажи") as WTextRange;
            textRange.CharacterFormat.FontSize = 18f;
            textRange.CharacterFormat.FontName = "Calibri";

            for (int tableCounter = 1, sviewCounter = 0; sviewCounter < sview.Count;
                tableCounter++, sviewCounter++)
            {
                //Appends table headers
                paragraph = table[tableCounter, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(sview[sviewCounter].datetime + "") as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";

                paragraph = table[tableCounter, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(sview[sviewCounter].manName) as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";

                paragraph = table[tableCounter, 2].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(sview[sviewCounter].summa + "") as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";
            }

            paragraph = section.AddParagraph();
            paragraph.ApplyStyle("Normal");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
            textRange = paragraph.AppendText(generalIncomeTextBlock.Text) as WTextRange;
            textRange.CharacterFormat.FontSize = 20f;
            textRange.CharacterFormat.FontName = "Calibri";

            section.AddParagraph();


            MemoryStream stream = new MemoryStream();

            //Saves the Word document to MemoryStream
            await document.SaveAsync(stream, FormatType.Docx);

            //Saves the stream as Word document file in local machine
            Save(stream, "Sample.docx");

        }

        //Saves the Word document
        async void Save(MemoryStream streams, string filename)
        {
            streams.Position = 0;
            StorageFile stFile;
            if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.DefaultFileExtension = ".docx";
                savePicker.SuggestedFileName = filename;
                savePicker.FileTypeChoices.Add("Word Documents", new List<string>() { ".docx" });
                stFile = await savePicker.PickSaveFileAsync();
            }
            else
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            if (stFile != null)
            {
                using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    //Write compressed data from memory to file
                    using (Stream outstream = zipStream.AsStreamForWrite())
                    {
                        byte[] buffer = streams.ToArray();
                        outstream.Write(buffer, 0, buffer.Length);
                        outstream.Flush();
                    }
                }
            }
            //Launch the saved Word file
            if (stFile != null)
                await Windows.System.Launcher.LaunchFileAsync(stFile);
        }

        private void dateFiltBtn_Click(object sender, RoutedEventArgs e)
        {
            if (datePic1.Date != null && datePic2.Date != null)
            {
                if (datePic1.Date.Value.DateTime < datePic2.Date.Value.DateTime)
                {

                    var temp = SalesViewManager.GetSalesPerDate(datePic1.Date.Value.DateTime.Date, datePic2.Date.Value.DateTime.Date);
                    if (temp.Count != 0)
                    {
                        sview = temp;
                        dataGrid.ItemsSource = sview;
                        generalIncomeTextBlock.Text = $"Итого заработано в период " +
                            $"{datePic1.Date.Value.DateTime.Date} - " +
                            $"{datePic2.Date.Value.DateTime.Date}: {SalesViewManager.GetGeneralIncome(sview)}";
                    }
                    else
                    {
                        Message.Show("Продаж в данный период нет!", this.Frame.XamlRoot ,"Сообщение");
                        sview = SalesViewManager.sales;
                        dataGrid.ItemsSource = sview;
                        generalIncomeTextBlock.Text = $"Итого заработано : {SalesViewManager.GetGeneralIncome(sview)}";
                    }
                }
                else
                {
                    Message.Show("Дата начала периода должна быть раньше даты окончания!", this.Frame.XamlRoot, "Предупреждение");

                }
            }
            else
            {
                Message.Show("Для фильтрации необходимо указать рамки периода!", this.Frame.XamlRoot, "Предупреждение");
            }
        }

        private void clearFiltBtn_Click(object sender, RoutedEventArgs e)
        {
            datePic1.Date = null;
            datePic2.Date = null;
            sview = SalesViewManager.sales;
            dataGrid.ItemsSource = sview;
            generalIncomeTextBlock.Text = $"Итого заработано : {SalesViewManager.GetGeneralIncome(sview)}";
        }
    }
}
