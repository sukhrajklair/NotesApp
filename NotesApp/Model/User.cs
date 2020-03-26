using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class User : INotifyPropertyChanged
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

		private string firstName;
		[MaxLength(50)]
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;
		[MaxLength(50)]
		public string LastName
		{
			get { return lastName; }
			set 
			{ 
				lastName = value;
				OnPropertyChanged("LastName");
			}
		}

		private string username;
		public string Username
		{
			get { return username; }
			set 
			{ 
				username = value;
				OnPropertyChanged("Username");
			}
		}

		private string email;
		public string Email
		{
			get { return email; }
			set 
			{ 
				email = value;
				OnPropertyChanged("Email");
			}
		}

		private string password;
		public string Password
		{
			get { return password; }
			set
			{ 
				password = value;
				OnPropertyChanged("Password");
			}

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	}
}
