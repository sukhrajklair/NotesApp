﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class Note: INotifyPropertyChanged
    {
		private int id;
		[PrimaryKey, AutoIncrement]
		public int Id
		{
			get { return id; }
			set 
			{ 
				id = value;
				OnPropertyChanged("Id");
			}
		}

		private int notebookId;
		[Indexed]
		public int NotebookId
		{
			get { return notebookId; }
			set 
			{ 
				notebookId = value;
				OnPropertyChanged("NotebookId");
			}
		}

		private DateTime createdTime;
		public DateTime CreatedTime
		{
			get { return createdTime; }
			set 
			{ 
				createdTime = value;
				OnPropertyChanged("CreatedTime");
			}
		}

		private DateTime updatedTime;
		public DateTime UpdatedTime
		{
			get { return updatedTime; }
			set 
			{ 
				updatedTime = value;
				OnPropertyChanged("UpdatedTime");
			}
		}

		private string title;
		public string Title
		{
			get { return title; }
			set
			{
				title = value;
				OnPropertyChanged("Title");
			}
		}

		private string fileLocation;
		public string FileLocation
		{
			get { return fileLocation; }
			set 
			{ 
				fileLocation = value;
				OnPropertyChanged("FileLocation");
			}
		}

		private bool isEditing;
		[Ignore]
		public bool IsEditing
		{
			get { return isEditing; }
			set
			{
				isEditing = value;
				OnPropertyChanged("IsEditing");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
