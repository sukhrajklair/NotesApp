using NotesApp.Model;
using NotesApp.ViewModel.Commands;
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
    /// Interaction logic for NotebookComponent.xaml
    /// </summary>
    public partial class NotebookComponent : UserControl
    {

        public Notebook Notebook
        {
            get { return (Notebook)GetValue(NotebookProperty); }
            set { SetValue(NotebookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Notebook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookProperty =
            DependencyProperty.Register("Notebook", typeof(Notebook), typeof(NotebookComponent), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotebookComponent notebookComponent = d as NotebookComponent;

            if (notebookComponent != null)
            {
                notebookComponent.NotebookComponentTextBlock.Text = (e.NewValue as Notebook).Name;
            }
        }

        public NotebookComponent()
        {
            InitializeComponent();
        }
    }
}
