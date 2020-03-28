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

            //the following statements setup the speech recognizer
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

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double> { 8, 9, 10, 11, 12, 13, 14, 16 };
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        //add the recognized speech to text
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

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            //Since IsChecked is a nullable boolean, we need to interpret null value as false using '??' operator
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                //apply FontWeights.Bold value to the Inline.FontWeightProperty of Selection
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            //Since IsChecked is a nullable boolean, we need to interpret null value as false using '??' operator
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isButtonChecked)
            {
                //apply FontWeights.Bold value to the Inline.FontWeightProperty of Selection
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                //Since TextDecorations doesn't contain Normal property, we have to remove underline in a different way
                //We first try to get the TextDecorationsProperty and then try to remove "Underline" from it and assing the rest to textDecorations variable
                //then we use textDecorations variable to apply the text decorations which doesn't contain "Underline" anymore
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fontSizeComboBox.Text))
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
            }
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            //if the FontWeight property is set ( or not Unset) and the forntweight is set to Bold
            //then set the button to be checked
            boldButton.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(FontWeights.Bold));

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(FontStyles.Italic));

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = (contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if(string.IsNullOrEmpty(App.UserId))
            {
                Login login = new Login();
                login.ShowDialog();
            }
        }
    }
}
