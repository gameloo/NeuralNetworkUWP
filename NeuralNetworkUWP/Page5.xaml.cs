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
using Windows.Storage.Pickers;
using System.Xml.Serialization;
using Windows.Storage;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace NeuralNetworkUWP
{
    public sealed partial class Page5 : Page
    {
        MLP network;

        public Page5()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                network = (MLP)e.Parameter;
                TbNumIN.Text = network.sizeIN.ToString();
                TbNumOUT.Text = network.sizeOUT.ToString();
            }
        }



        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            BoxIn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            try
            {
                string[] tempString = BoxIn.Text.Split(';');
                double[] question = new double[tempString.Length];
                if (question.Length != network.sizeIN) throw new Exception();
                for (int i = 0; i < tempString.Length; i++)
                    question[i] = Convert.ToDouble(tempString[i]);

                double[] answer = network.calculate(question);
                string answerString = "";

                for (int i = 0; i < answer.Length; i++)
                {
                    if (ChBoxBool.IsChecked == false)
                        answerString += answer[i].ToString();
                    else
                    {
                        if (answer[i] > 0.5) answerString += "1";
                        else answerString += "0";
                    }
                    if (i != (answer.Length - 1)) answerString += ";";
                }

                BoxOut.Text = answerString;
        }
            catch
            {
                BoxIn.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                return;
            }
}

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add("Plain Text", new List<string> { ".mlp" });
            fileSavePicker.SuggestedFileName = "NewNeuralNetwork";
            fileSavePicker.CommitButtonText = "Сохранить";

            var newFile = await fileSavePicker.PickSaveFileAsync();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MLP));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, network);

            if (newFile != null)
            {
                await FileIO.WriteTextAsync(newFile, stringWriter.ToString());
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
