using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for Notes.xaml
    /// </summary>
    public partial class Notes : Window
    {
        SpeechRecognitionEngine recognizer;

        public Notes()
        {
            InitializeComponent();

            //get the current culture from windows
            var currentCulture = (from r in SpeechRecognitionEngine.InstalledRecognizers()
                                 where r.Culture.Equals(Thread.CurrentThread.CurrentCulture)
                                 select r).FirstOrDefault();
            recognizer = new SpeechRecognitionEngine(currentCulture);
            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendDictation();
            Grammar grammar = new Grammar(builder);
            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += Recognizer_SpeecRecognized;
        }

        private void Recognizer_SpeecRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;

            contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(recognizedText)));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfCharacters = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Length;
            statusTextBlock.Text = $"Document length: {amountOfCharacters} characters";

        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            //var textToBold = new TextRange(contentRichTextBox.Selection.Start, contentRichTextBox.Selection.End);

            //Since IsChecked is a nullable boolean, we need to interpret null value as false using '??' operator
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                //apply FontWeights.Bold value to the Inline.FontWeightProperty of Selection
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else 
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void speechButton_Click(object sender, RoutedEventArgs e)
        {
            //Since IsChecked is a nullable boolean, we need to interpret null value as false using '??' operator
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                recognizer.RecognizeAsyncStop();
            }
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            //if the FontWeight property is set ( or not Unset) and the forntweight is set to Bold
            //the set the button to be checked
            boldButton.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(FontWeights.Bold));
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
