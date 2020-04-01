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
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set 
            { 
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                ReadNotes();
            }
        }

        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                SelectedNoteChanged(this, new EventArgs());
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand  NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public BeginEditCommand BeginEditCommand { get; set; }

        public HasEditedCommand HasEditedCommand { get; set; }

        public DeleteCommand DeleteCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            HasEditedCommand = new HasEditedCommand(this);
            DeleteCommand = new DeleteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            //ReadNotebooks();
            //ReadNotes();
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
                UserId = int.Parse(App.UserId),
                IsEditing = true
            };

            DatabaseHelper.Insert<Notebook>(newNotebook);

            ReadNotebooks();
        }

        public void ReadNotebooks()
        {
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                int userId = int.Parse(App.UserId);
                var notebooks = conn.Table<Notebook>().Where(nb => nb.UserId == userId).ToList();
                Notebooks.Clear();

                foreach(var notebook in notebooks)
                {
                    //notebook.IsEditing = false;
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

        public void StartEditing(Notebook notebook)
        {
            notebook.IsEditing = true;
        }

        public void HasRenamed(Notebook notebook)
        {
            if(notebook != null)
            {
                DatabaseHelper.Update(notebook);
                notebook.IsEditing = false;
                ReadNotebooks();
            }
            
        }

        public void UpdateSelectedNote()
        {
            DatabaseHelper.Update(SelectedNote);
        }

        public void DeleteSelectedNotebook(Notebook notebook)
        {
            if (notebook != null)
            {
                DatabaseHelper.Delete(notebook);
                ReadNotebooks();
            }
        }
    }
}
