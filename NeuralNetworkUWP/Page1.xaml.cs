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


namespace NeuralNetworkUWP
{
    public sealed partial class Page1 : Page
    {
        //MLP network;
        NeuralNetworkUWP.Beta_NeuralNetwork_v3.MLP network;

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
                     string serializedMLP = await FileIO.ReadTextAsync(file);
                     var stringReader = new StringReader(serializedMLP);
                     //network = (MLP)xmlSerializer.Deserialize(stringReader);
                     network = (NeuralNetworkUWP.Beta_NeuralNetwork_v3.MLP)xmlSerializer.Deserialize(stringReader);
                 });
                TbNumIN.Text = network.sizeIN.ToString();
                TbNumOUT.Text = network.sizeOUT.ToString();
                TbNumHidden.Text = network.GetHiddenLayersInfo();

                BtnNext.IsEnabled = true;
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page2));
        }
    }
}
