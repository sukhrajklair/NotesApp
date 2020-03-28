using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        private Visibility isEditing;
        public Visibility IsEditing 
        { get { return isEditing; } 
          set
            {
                isEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set 
            { 
                selectedNotebook = value;
                ReadNotes();
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand  NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public BeginEditCommand BeginEditCommand { get; set; }

        public HasEditedCommand HasEditedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public NotesVM()
        {
            isEditing = Visibility.Collapsed;

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            HasEditedCommand = new HasEditedCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            ReadNotebooks();
            ReadNotes();
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert<Note>(newNote);

            ReadNotes();
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New Notebook",
                UserId = int.Parse(App.UserId)
            };

            DatabaseHelper.Insert<Notebook>(newNotebook);

            ReadNotebooks();
        }

        public void ReadNotebooks()
        {
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                var notebooks = conn.Table<Notebook>().ToList();
                Notebooks.Clear();

                foreach(var notebook in notebooks)
                {
                    Notebooks.Add(notebook);
                }
            }
        }

        public void ReadNotes()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                if (SelectedNotebook != null)
                {
                    var notes = conn.Table<Note>().Where(note => note.NotebookId == SelectedNotebook.Id).ToList();
                    Notes.Clear();
                    foreach (var note in notes)
                    {
                        Notes.Add(note);
                    }
                }    
            }
        }

        public void StartEditing()
        {
            IsEditing = Visibility.Visible;
        }

        public void HasRenamed(Notebook notebook)
        {
            if(notebook != null)
            {
                DatabaseHelper.Update(notebook);
                IsEditing = Visibility.Collapsed;
                ReadNotebooks();
            }
            
        }


    }
}
