using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NeuralNetworkUWP.NeuralNetwork;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NeuralNetworkUWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        MLP network;

        public Page1()
        {
            this.InitializeComponent();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page5), network);
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.CommitButtonText = "Открыть";
            openPicker.FileTypeFilter.Add(".mlp");
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                var xmlSerializer = new XmlSerializer(typeof(MLP));
                await Task.Run(async () =>
                 {
                     //var stringReader = new StringReader(System.IO.File.ReadAllText(file.Path));
                     string strinf = await FileIO.ReadTextAsync(file);
                     var stringReader = new StringReader(strinf);
                     //return (T)xmlSerializer.Deserialize(stringReader);
                     //var stringReader = await FileIO.ReadTextAsync(file);
                     network = (MLP)xmlSerializer.Deserialize(stringReader);
                 });
                BtnNext.IsEnabled = true;
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page2));
        }
    }
}
