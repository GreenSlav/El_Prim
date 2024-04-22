using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty] 
    int entryTranslationY;
    


    [RelayCommand]
    public async Task RemoveEntryAsync()
    {
        EntryTranslationY = -1000;
    }
}