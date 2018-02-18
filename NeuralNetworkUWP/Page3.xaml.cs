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
using NeuralNetworkUWP.NeuralNetwork;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NeuralNetworkUWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        DataToTrain dataToTrain;
        MLP network;

        public Page3()
        {
            this.InitializeComponent();
            BoxHidden.AddHandler(TappedEvent, new TappedEventHandler(AnyBox_Tapped), true);
            BoxAlpha.AddHandler(TappedEvent, new TappedEventHandler(AnyBox_Tapped), true);
            BoxEps.AddHandler(TappedEvent, new TappedEventHandler(AnyBox_Tapped), true);
            BoxError.AddHandler(TappedEvent, new TappedEventHandler(AnyBox_Tapped), true);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                dataToTrain = (DataToTrain)e.Parameter;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            BoxHidden.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            BoxAlpha.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            BoxEps.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            BoxError.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            try
            {
                network = new MLP(dataToTrain, TextToIntArray(BoxHidden.Text));
            }
            catch
            {
                BoxHidden.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }
            try
            {
                network.Alpha = Convert.ToDouble(BoxAlpha.Text);
            }
            catch
            {
                BoxAlpha.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }
            try
            {
                network.Epsilon = Convert.ToDouble(BoxEps.Text);
            }
            catch
            {
                BoxEps.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }
            try
            {
                network.Error = Convert.ToDouble(BoxError.Text);
            }
            catch
            {
                BoxError.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }
            Frame.Navigate(typeof(Page4), network);
        }

        private int[] TextToIntArray(string text)
        {
            string[] tempString = text.Split(';');
            int[] returnArray = new int[tempString.Length];

            for (int i = 0; i < returnArray.Length; i++)
                returnArray[i] = Convert.ToInt32(tempString[i]);
            return returnArray;
        }


        private void ButtonCloseDialog_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(BtnCloseHelpHidden)) dialogHelpHidden.Hide();
            if (sender.Equals(BtnCloseHelpAlpha)) dialogHelpAlpha.Hide();
            if (sender.Equals(BtnCloseHelpEps)) dialogHelpEps.Hide();
            if (sender.Equals(BtnCloseHelpError)) dialogHelpError.Hide();

        }

        private void AnyBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender.Equals(BoxHidden)) dialogHelpHidden.ShowAt((TextBox)sender);
            if (sender.Equals(BoxAlpha)) dialogHelpAlpha.ShowAt((TextBox)sender);
            if (sender.Equals(BoxEps)) dialogHelpEps.ShowAt((TextBox)sender);
            if (sender.Equals(BoxError)) dialogHelpError.ShowAt((TextBox)sender);
        }
    }
}

