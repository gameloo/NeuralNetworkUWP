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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NeuralNetworkUWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        public Page3()
        {
            this.InitializeComponent();
            BoxHidden.AddHandler(TappedEvent, new TappedEventHandler(BoxHidden_Tapped), true);
            BoxAlpha.AddHandler(TappedEvent, new TappedEventHandler(BoxAlpha_Tapped), true);
            BoxEps.AddHandler(TappedEvent, new TappedEventHandler(BoxEps_Tapped), true);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page4));
        }

        private void BoxAlpha_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dialogHelpAlpha.ShowAt((TextBox)sender);
        }

        private void BoxHidden_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dialogHelpHidden.ShowAt((TextBox)sender);
        }

        private void BoxEps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dialogHelpEps.ShowAt((TextBox)sender);
        }

        private void ButtonCloseDialog_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(BtnCloseHelpHidden)) dialogHelpHidden.Hide();
            if (sender.Equals(BtnCloseHelpAlpha)) dialogHelpAlpha.Hide();
            if (sender.Equals(BtnCloseHelpEps)) dialogHelpEps.Hide();

        }
    }
}
