using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Persistence;
using SQLite;
using Xamarin.Forms;

namespace Memento
{
	public partial class MainPage : ContentPage
	{
	    private ObservableCollection<Memo> _memos;
	    private SQLiteAsyncConnection _connection;

        public MainPage()
		{
			InitializeComponent();
		    _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        }

	    protected override async void OnAppearing()
	    {
	        await _connection.CreateTableAsync<Memo>();

	        var memos = await _connection.Table<Memo>().ToListAsync();
	        _memos = new ObservableCollection<Memo>(memos);

	        MemoList.ItemsSource = GetMemo();


	        base.OnAppearing();
	    }

	    ObservableCollection<Memo> GetMemo(string searchText = null)
	    {
	        if (string.IsNullOrWhiteSpace(searchText))
	            return _memos;

	        return new ObservableCollection<Memo>(_memos.Where(c => c.Title.StartsWith(searchText)));

        }

        private async void OnDelete(object sender, EventArgs e)
	    {
	        var memo = (sender as MenuItem).CommandParameter as Memo;

	        if (await DisplayAlert("Warning", $"Are you sure you want to delete {memo.Title}?", "Yes", "No"))
	        {
	            await _connection.DeleteAsync(memo);
                _memos.Remove(memo);
                
	        }

	    }

	    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
	        if (MemoList.SelectedItem == null)
	            return;

	        var selectedMemo = e.SelectedItem as Memo;

	        var page = new MemoDetailsPage(selectedMemo);

	        page.MemoUpdated += (source, memo) =>
	        {

                selectedMemo.Id = memo.Id;
	            selectedMemo.Title = memo.Title;
	            selectedMemo.Content = memo.Content;
	            selectedMemo.Date = memo.Date;

	            _connection.UpdateAsync(selectedMemo);
            };

	        MemoList.SelectedItem = null;

	        await Navigation.PushAsync(page);
	    }

	    private async void OnAddMemo(object sender, EventArgs e)
	    {
	        var page = new MemoDetailsPage(new Memo());

	        page.MemoAdded += async (source, memo) =>
	        {
	            await _connection.InsertAsync(memo);
	            _memos.Add(memo);
	        };

	        await Navigation.PushAsync(page);
	    }

	    private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	        MemoList.ItemsSource = GetMemo(e.NewTextValue);
	    }
	}
}
