using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Sqlite_Practice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Sqlite_Practice
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SQLiteConnection conn;
        private List<Person> persons;
        int personId;
        string function = "insert";

        public MainPage()
        {
            this.InitializeComponent();
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            conn.CreateTable<Person>();
            getPersons();
            ListPerson.SelectionChanged += ListPerson_SelectionChanged;
            btn_Delete.Click += Btn_Delete_Click;

        }
 private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            delete();
        }
        private void ListPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selected = (Person)ListPerson.SelectedItem;
            if (selected != null)
            {
                personId = selected.Id;
                tb_Age.Text = Convert.ToString(selected.Age);
                tb_Name.Text = selected.Name;
                function = "update";

               
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            addPerson();
          
        }

        private void addPerson()
        {
            String age, name;
            age = tb_Age.Text;
            name = tb_Name.Text;
            if (name == "")
            {
                tb_Name.Focus(FocusState.Programmatic);
            }
            else
            {
                if (age == "")
                {
                    tb_Age.Focus(FocusState.Programmatic);
                }
                else
                {
                    if (function == "insert")
                    {
                     conn.Insert(new Person() {
                      Name = name,
                      Age = Convert.ToInt16(age) 
                    });
                    }
                    else
                    {
                        conn.Update(new Person()
                        {
                            Id = personId,
                            Name = name,
                            Age = Convert.ToInt16(age)
                        });
                    }

                    function = "insert";

                    tb_Age.Text = "";
                    tb_Name.Text = "";
                    getPersons();
                }
            }
        }

        private void getPersons()
        {
            persons = new List<Person>();
            var query = conn.Table<Person>();
            foreach (var data in query)
            {
                persons.Add(new Person { Name = data.Name, Age = data.Age, Id = data.Id });
            }
            ListPerson.ItemsSource = null;
            ListPerson.ItemsSource = persons;
        }

        private void delete()
        {
            conn.Delete(new Person()
            {
                Id = personId
        });

            function = "insert";
            tb_Age.Text = "";
            tb_Name.Text = "";
            //conn.RunInTransaction(() =>
            //{
            //    var p = conn.Execute("DELETE FROM Person");
            //    //! var p = conn.Execute("DELETE FROM Person WHERE Id =?", personId);
            //});


            getPersons();
        }

       
    }
}
