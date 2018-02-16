using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memento
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemoDetailsPage : ContentPage
	{
	    public event EventHandler<Memo> MemoAdded;
	    public event EventHandler<Memo> MemoUpdated;

        public MemoDetailsPage (Memo memo)
		{
		    if (memo == null)
		        throw new ArgumentNullException(nameof(memo));

            InitializeComponent ();

		    BindingContext = new Memo()
		    {
		        Id = memo.Id,
		        Title = memo.Title,
		        Content = memo.Content,
		        Date = memo.Date
		    };
        }

	    private async void OnSave(object sender, EventArgs e)
	    {
	        var memo = BindingContext as Memo;

	        if (String.IsNullOrWhiteSpace(memo.Title))
	        {
	            await DisplayAlert("Error", "Please enter the title.", "OK");
                return;
	        }

	        if (memo.Id == 0)
	        {
	            MemoAdded?.Invoke(this, memo);
	        }
	        else
	        {
	            MemoUpdated?.Invoke(this, memo);
	        }

	        await Navigation.PopAsync();
	    }

	}
}