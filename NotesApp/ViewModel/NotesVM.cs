using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    class NotesVM
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set 
            { 
                selectedNotebook = value; 
                //TODO: get the notes
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand  NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            ReadNotebooks();
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
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New Notebook"
            };

            DatabaseHelper.Insert<Notebook>(newNotebook);
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
    }
}
