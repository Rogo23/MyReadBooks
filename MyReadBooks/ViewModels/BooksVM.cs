using Prism.Navigation;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;

namespace MyReadBooks.ViewModels
{
    public class BooksVM
    {
        public ICommand NewBookCommand { get; set; }
        INavigationService _navigationService;
        public BooksVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NewBookCommand = new DelegateCommand(NewBookAction);
        }
        
        private async void NewBookAction()
        {
            await _navigationService.NavigateAsync("NewBookPage");
        }
    }
}
