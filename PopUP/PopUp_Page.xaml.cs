using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

// конструктор класса
namespace PopUP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp_Page : ContentPage
    {
        private int questionIndex;
        private int score;
        private List<int> numbers;
        private Random random;

        public PopUp_Page()
        {
            InitializeComponent();
            // создание нового экземпляра класса Random
            random = new Random();
        }

        // обработчик события нажатия на кнопку "Начать тест"
        private async void OnStartTestClicked(object sender, EventArgs e)
        {
           
            // получение имени пользователя из поля ввода
            var name = NameEntry.Text;

            // проверка наличия имени пользователя
            if (string.IsNullOrWhiteSpace(name))
            {
                // вывод сообщения об ошибке
                await DisplayAlert("Viga", "Palun sisesta oma nimi.", "OK");
                return;
            }

            // сброс переменных
            questionIndex = 0;
            score = 0;
            numbers = new List<int>();

            // генерация списка случайных чисел
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(random.Next(1, 11));
            }

            // вызов метода отображения вопроса
            await DisplayQuestion();
        }

        // Метод, отображающий текущий вопрос и запрашивающий ответ пользователя
        private async System.Threading.Tasks.Task DisplayQuestion()
        {
            // Получаем первый множитель для текущего вопроса из списка
            var number1 = numbers[questionIndex];

            // Генерируем случайный второй множитель от 1 до 10
            var number2 = random.Next(1, 11);

            // Вычисляем правильный ответ
            var answer = number1 * number2;

            // Запрашиваем ответ пользователя с помощью метода DisplayPromptAsync
            var response = await DisplayPromptAsync($"Küsimus {questionIndex + 1}", $"Kui palju on {number1} x {number2}?", placeholder: "Vastus");

            // Записываем вопрос и правильный ответ в файл
            await SaveDataToFileAsync("test.txt", $"{number1} x {number2} = {answer}\n");

            // Проверяем, является ли введенный пользователем ответ числом и сравниваем с правильным ответом
            if (int.TryParse(response, out int userAnswer))
            {
                if (userAnswer == answer)
                {
                    // Если ответ верный, увеличиваем счетчик правильных ответов
                    score++;
                }
            }

            // Проверяем, не закончился ли тест
            if (questionIndex < 9)
            {
                // Если вопросов меньше 10, переходим к следующему вопросу
                questionIndex++;
                await DisplayQuestion();
            }
            else
            {
                // Иначе вычисляем процент правильных ответов и буквенную оценку
                var percentageScore = (double)score /10 * 100;

                var letterGrade = GetLetterGrade(percentageScore);
                // Отображаем сообщение с результатами теста
                await DisplayAlert("Test lõpetatud", $"{NameEntry.Text}, sinu punktid {score} / 10. ({percentageScore}%)\nHinne: {letterGrade}", "OK");
            }
        }

        // Определение буквенной оценки на основе процентного результата
        private string GetLetterGrade(double percentageScore)
        {
            if (percentageScore >= 90)
            {
                return "5";
            }
            else if(percentageScore >= 75)
            {
                return "4";
            }
            else if (percentageScore >= 50)
            {
                return "3";
            }
            else
            {
                return "2";
            }
        }

        // Обработчик клика на кнопку, возвращающийся на предыдущую страницу
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Асинхронное сохранение данных в файл
        async Task SaveDataToFileAsync(string fileName, string data)
        {
            // Получаем путь к локальной папке приложения
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, fileName);
            
            // Записываем данные в файл
            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                await sw.WriteAsync(data).ConfigureAwait(false);
            }
        }

        // Обработчик нажатия на кнопку "Удалить"
        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            // Получаем путь к рабочему столу и полный путь к файлу
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullPath = Path.Combine(path, "test.txt");

            
            // Если файл существует, то удаляем его
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Korras", $"Faili kustutamine õnnestus", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Viga", $"Viga faili kustutamisel: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Viga", "Faili ei leitud", "OK");
            }
        }
    }
}