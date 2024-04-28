using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    // сюда из изначальной матрицы будут записываться пути в формате "13": путь из 1 в 3 и наоборот
    // чтоб при отрисовке пути понимать, был ли он уже отрисован(содержится в нашем сете) или нет
    private HashSet<string> _existedGraphPaths = new();

    // сет использованных для графов цветов
    public HashSet<string> UsedColorsForGraph = new();


    private int[,] matrixForExample = {
        {0, 3, 1, 2, 2},
        {3, 0, 8, 3, 1},
        {1, 8, 0, 1, 4},
        {1, 1, 1, 0, 6},
        {2, 1, 4, 6, 0}
    };

    async Task DrawMatrixGraph()
    {
        
    }
    
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
    [NotifyPropertyChangedFor(nameof(AbsLayoutAssebmlyButton))]
    int entryTranslationY;
    [ObservableProperty]
    bool isEnabledEntry;
    [ObservableProperty]
    string enteredBorderEntryText;
    
    // AssemblyButton
    [ObservableProperty] 
    Color backgroundColorAssemblyButton;
    public Rect AbsLayoutAssebmlyButton => new Rect(20, EntryTranslationY + 65, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
    


    [RelayCommand]
    public async Task RemoveEntryAsync()
    {
        // initialXBorderEntryPos = EntryTranslationY;  коммент птмчт всегда 0 по сути

        var target =  Screen.PrimaryScreen.Bounds.Height/4;
        
        IsEnabledEntry = false;
        IsVisibleBorderButton = true;
        
        while (EntryTranslationY > -target)
        {
            await Task.Delay(10);
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
            await Task.Delay(10);
            ++EntryTranslationY;
        }
        
        IsEnabledEntry = true;
        IsVisibleBorderButton = false;
    }


    private bool LoadDll(string pathToDll)
    {
        return true;
    }
}