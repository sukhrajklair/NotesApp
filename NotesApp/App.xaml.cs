using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NotesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string userId;

        public static string UserId
        {
            get { return userId; }
            set { 
                userId = value;
                HasLoggedIn(null, new EventArgs());
            }
        }

        public static event EventHandler HasLoggedIn;



    }
}
