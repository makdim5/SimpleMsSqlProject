using App1.model;
using App1.util;
using App1.view;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SalesmenPage : Page
    {
        public List<Salesmen> salesmen;
        public Salesmen man;
        Grid myGrid;
        TextBlock t;
        public SalesmenPage()
        {
            this.InitializeComponent();
            SalesmenManager.UpdateDBCollection();
            salesmen = SalesmenManager.salesmen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSalesmanPage), null, new DrillInNavigationTransitionInfo());

        }

        private void View_ItemClick(object sender, ItemClickEventArgs e)
        {
            var manl = (Salesmen)e.ClickedItem;

            man = new Salesmen();
            man.Name = manl.Name;
            man.Id = manl.Id;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            if (man != null)
            {
                this.Frame.Navigate(typeof(AddSalesmanPage), man, new DrillInNavigationTransitionInfo());
            }
            else
            {
                
                Message.Show(" Выберите для изменения продавца!", this.Frame.XamlRoot, "Предупреждение!");
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (man != null)
            {
                ContentDialog deleteDialog = new ContentDialog()
                {
                    Title = "Подтверждение действия",
                    Content = "Вы действительно хотите удалить продавца?",
                    PrimaryButtonText = "ОК",
                    SecondaryButtonText = "Отмена",
                    XamlRoot = this.Frame.XamlRoot
                };

                ContentDialogResult result = await deleteDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    SalesmenManager.Remove(man.Id);
                    this.Frame.Navigate(typeof(SalesmenPage));
                    Message.Show("Запись удалена!!!", this.Frame.XamlRoot, "Сообщение");
                }
                else if (result == ContentDialogResult.Secondary)
                {

                }
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (man != null)
            {
                myGrid = new Grid();
                var printHelper = new Microsoft.Toolkit.Uwp.Helpers.PrintHelper(g);



                t = new TextBlock();
                t.Text = $"\t\t\t\tСправка\nПродавец {man.Name} работает в данном магазине!";
                t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                t.FontSize = 30;
                t.TextWrapping = TextWrapping.WrapWholeWords;
                myGrid.Children.Add(t);
                Grid.SetRow(t, 0);

                printBtn.IsEnabled = false;
                printHelper.AddFrameworkElementToPrint(myGrid);

                printHelper.OnPrintSucceeded += PrintHelperAfterActions;
                printHelper.OnPrintFailed += PrintHelperAfterActions;
                printHelper.OnPrintCanceled += PrintHelperAfterActions;



                await printHelper.ShowPrintUIAsync("Печать");
            }
        }

        private void PrintHelperAfterActions()
        {
            
            printBtn.IsEnabled = true;
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (man != null)
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
               
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                WTextRange textRange = paragraph.AppendText($"\t\t\t\tСправка\nПродавец {man.Name} работает в данном магазине!") as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";

                section.AddParagraph();


                MemoryStream stream = new MemoryStream();

                //Saves the Word document to MemoryStream
                await document.SaveAsync(stream, FormatType.Docx);

                //Saves the stream as Word document file in local machine
                Save(stream, $"{man.Name}.docx");
            }

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


    }
}
