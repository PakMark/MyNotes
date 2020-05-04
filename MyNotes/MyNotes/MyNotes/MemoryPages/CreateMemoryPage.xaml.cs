using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateMemoryPage : ContentPage
	{
		public CreateMemoryPage()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Обработчик кнопки "Создать запись"
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие</param>
        async void CreateMemoryClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(memoryEditor.Text))
            {
                // Сохранение записи в базе данных.
                await MemoriesPage.Database.SaveMemoryAsync(new Memory
                {
                    MemoryText = memoryEditor.Text
                });
                memoryEditor.Text = string.Empty;
                DependencyService.Get<IMessage>().ShortAlert("Запись успешно сохранена");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет записи!!!", "ОК");
            }
        }
    }
}