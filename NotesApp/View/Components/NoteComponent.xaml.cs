using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotesApp.View.Components
{
    /// <summary>
    /// Interaction logic for NoteComponent.xaml
    /// </summary>
    public partial class NoteComponent : UserControl
    {


        public Note Note
        {
            get { return (Note)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Note.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(NoteComponent), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NoteComponent note = d as NoteComponent;
            if (note != null)
            {
                note.titleTextBlock.Text = (e.NewValue as Note).Title;
                note.editedTextBlock.Text = (e.NewValue as Note).UpdatedTime.ToShortDateString();
                note.contentTextBlock.Text = (e.NewValue as Note).Title;
            }

        }

        public NoteComponent()
        {
            InitializeComponent();
        }
    }
}
