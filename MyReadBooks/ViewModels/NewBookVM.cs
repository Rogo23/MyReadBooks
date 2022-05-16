using MyReadBooks.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyReadBooks.Models;
using System.Windows.Input;
using Prism.Commands;
using SQLite;
using System.Collections.ObjectModel;

namespace MyReadBooks.ViewModels
{
    public class NewBookVM
    {
        public ObservableCollection<Item> SearchResults { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public NewBookVM()
        {
            SearchResults = new ObservableCollection<Item>();
            SearchCommand = new DelegateCommand<string>(GetSearchResults);
            SaveCommand = new DelegateCommand<Item>(SaveBook, CanSaveBook);
        }
        private async void GetSearchResults(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync($"https://www.googleapis.com/books/v1/volumes?q={query}&key{Constants.GOOGLE_BOOKS_API_KEY}");

                var data = JsonConvert.DeserializeObject<BooksAPI>(result);

                SearchResults.Clear();
                foreach (var book in data.items)
                {
                    SearchResults.Add(book);
                }
            }
        }

        private void SaveBook(Item book)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Item>();
                int booksInserted = conn.Insert(book);
                if (booksInserted >= 1)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Book Saved", "Ok");
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "An error occurred while saving the book", "Ok");
                }
            }

        }

        private bool CanSaveBook(Item book)
        {
            return book != null;
        }
    }
}
