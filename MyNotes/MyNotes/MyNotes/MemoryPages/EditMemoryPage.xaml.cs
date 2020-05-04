using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyNotes.Models;

namespace MyNotes.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditMemoryPage : ContentPage
	{
        MemoriesDatabase database;
        Memory newMemory;

        public EditMemoryPage(ref Memory memory, MemoriesDatabase db)
        {
            InitializeComponent();
            memoryEditor.Text = memory.MemoryText;
            database = db;
            newMemory = memory;
        }

        /// <summary>
        /// Метод сохранения изменений записи
        /// </summary>
        /// <param name="sender">Отправитель Button</param>
        /// <param name="e">Событие отправителя</param>
        async void EditMemoryClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(memoryEditor.Text))
            {
                newMemory.MemoryText = memoryEditor.Text;
                // Сохранение измененной записи.
                await database.SaveMemoryAsync(newMemory);
                DependencyService.Get<IMessage>().ShortAlert("Запись успешно изменена");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Предупреждение!", "Нет записи!!!", "ОК");
            }
        }
    }
}