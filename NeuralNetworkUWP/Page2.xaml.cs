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
using System.Threading.Tasks;

namespace NeuralNetworkUWP
{
    public sealed partial class Page2 : Page
    {
        DataToTrain dataToTrain;

        public Page2()
        {
            this.InitializeComponent();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page3), dataToTrain);
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                openPicker.CommitButtonText = "Открыть";
                openPicker.FileTypeFilter.Add(".csv");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    await Task.Run(() => { dataToTrain = new DataToTrain(file); });
                    TbNumIN.Text = dataToTrain.SizeIn.ToString();
                    TbNumOUT.Text = dataToTrain.SizeOut.ToString();
                    BtnNext.IsEnabled = true;
                }
                TbError.Text = "Нет";
            }
            catch
            {
                TbError.Text = "Ошибка при загрузке файла";
                BtnNext.IsEnabled = false;
                return;
            }
        }
    }
}
