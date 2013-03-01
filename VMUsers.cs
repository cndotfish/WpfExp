using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WpfExp
{
    class VMUsers : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public List<User> Users
        {
            get
            {
                return new List<User>{                    
                    new User(){ Name="wxr", Age=22, Email1="54264531@g.cd", Email2="3243242@er.dd"},
                    new User(){ Name="wxr", Age=22, Email1="54264531@g.cd", Email2="3243242@er.dd"},
                    new User(){ Name="wxr", Age=22, Email1="54264531@g.cd", Email2="3243242@er.dd"},
                    new User(){ Name="wxr", Age=22, Email1="54264531@g.cd", Email2="3243242@er.dd"}
                };
            }
        }
    }

    class User : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string name;
        private int age;
        private string email1;
        private string email2;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                NotifyPropertyChanged("Age");
            }
        }

        public string Email1
        {
            get { return email1; }
            set
            {
                email1 = value;
                NotifyPropertyChanged("Email1");
            }
        }

        public string Email2
        {
            get { return email2; }
            set
            {
                email2 = value;
                NotifyPropertyChanged("Email2");
            }
        }
    }
}
