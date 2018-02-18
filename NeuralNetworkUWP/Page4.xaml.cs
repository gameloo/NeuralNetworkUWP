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
using System.Threading.Tasks;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NeuralNetworkUWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        MLP network;
        Task threadLearning;


        public Page4()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                network = (MLP)e.Parameter;
            threadLearning = new Task(() => network.learning());
            threadLearning.Start();
            Task.Run(() => LoopLog());

            Task.Run(async() =>
            {
                while (!threadLearning.IsCompleted)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        BtnNext.IsEnabled = true;
                    });
            });
        }

        private async void LoopLog()
        {
            string[] logStringArr;

            while (true)
            {
                logStringArr = network.GetLogInfo();
                if ((threadLearning.IsCompleted == true) && (logStringArr.Length == 0)) break;

                if (logStringArr.Length != 0)
                {
                    string logString = "";
                    for (int i = 0; i < logStringArr.Length; i++)
                    {
                        logString += logStringArr[i];
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                            () =>
                         {
                             BoxLog.Items.Add(logStringArr[i]);
                             if(ChBoxAutoScroll.IsChecked == true) BoxLog.ScrollIntoView(BoxLog.Items.Last());
                         });
                    }
                    Log.write(logString);
                }
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page5), network);
        }
    }
}
