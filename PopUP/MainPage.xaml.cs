using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Конструктор класса MainPage
namespace PopUP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        List<ContentPage> pages = new List<ContentPage>() { new PopUp_Page() };
        List<string> tekstid = new List<string> { "Ava PopUP leht" };

        public MainPage()
        {
            // Создаем объект StackLayout и настраиваем его параметры
            StackLayout st = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Aqua
            };
            // Создаем кнопки для каждой страницы и добавляем их в StackLayout
            for (int i = 0; i < pages.Count; i++)
            {
                Button button = new Button
                {
                    Text = tekstid[i],
                    TabIndex = i,
                    BackgroundColor = Color.Fuchsia,
                    TextColor = Color.Black
                };
                st.Children.Add(button);
                // Привязываем метод Navig_funktsion к событию Clicked для каждой кнопки
                button.Clicked += Navig_funktsion;
            }
            // Устанавливаем содержимое страницы равным созданному StackLayout
            Content = st;
        }

        // Метод, который будет вызываться при нажатии на кнопку
        private async void Navig_funktsion(object sender, EventArgs e)
        {
            // Приводим объект-отправитель к типу Button и сохраняем его в переменную btn
            Button btn = sender as Button; //(Button)sender
           // Открываем страницу, соответствующую нажатой кнопке, в Popup-окне
            await Navigation.PushAsync(pages[btn.TabIndex]);
        }
    }
}