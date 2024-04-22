using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

    // BorderButton
    [ObservableProperty]
    bool isVisibleBorderButton;
    [ObservableProperty]
    bool isEnabledBorderButton;
    
    // BorderEntry
    [ObservableProperty] 
    int entryTranslationY;
    [ObservableProperty]
    bool isEnabledEntry;
    [ObservableProperty]
    string enteredBorderEntryText;


    [RelayCommand]
    public async Task RemoveEntryAsync()
    {
        // initialXBorderEntryPos = EntryTranslationY;  коммент птмчт всегда 0 по сути

        var target =  Screen.PrimaryScreen.Bounds.Height/4;
        
        IsEnabledEntry = false;
        IsVisibleBorderButton = true;
        
        while (EntryTranslationY > -target)
        {
            await Task.Delay(4);
            --EntryTranslationY;
        }

        EnteredBorderEntryText = "";
        
        
        IsEnabledBorderButton = true;
    }
    
    
    
    [RelayCommand]
    async Task GetBackEntryAsync()
    {
        IsEnabledBorderButton = false;
        
        var target =  0;
        
        while (EntryTranslationY != target)
        {
            await Task.Delay(4);
            ++EntryTranslationY;
        }
        
        IsEnabledEntry = true;
        IsVisibleBorderButton = false;
    }
}