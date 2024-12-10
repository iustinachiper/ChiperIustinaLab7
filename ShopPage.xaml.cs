using ChiperIustinaLab7.Models;
using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
namespace ChiperIustinaLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions{ Name = "Magazinul meu preferat" };

        var shoplocation = locations?.FirstOrDefault();
        //var myLocation = await Geolocation.GetLocationAsync();
        var myLocation = new Location(46.7731796289, 23.6213886738);
       //pentru Windows Machine 

        var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);
        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }
        await Map.OpenAsync(shoplocation, options);
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shopList = (ShopList)BindingContext; // Asigură-te că BindingContext este ShopList

        if (shopList == null || shopList.ID == 0)
        {
            await DisplayAlert("Eroare", "Nu există niciun magazin de șters.", "OK");
            return;
        }

        bool answer = await DisplayAlert("Confirmare",
                                         "Ești sigur că vrei să ștergi acest magazin?",
                                         "Da", "Nu");

        if (answer)
        {
            // Apelează metoda existentă pentru ștergere
            await App.Database.DeleteShopListAsync(shopList);

            // Navighează înapoi la pagina anterioară
            await Navigation.PopAsync();
        }
    }

    //new commit
}