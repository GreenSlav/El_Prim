using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    // текущий путь до сборки с реализацией
    public Assembly MainAssembly;

    public const string PathToContract = "D:\\maui_last_stable\\El_Prim\\GraphsMT\\MauiContract\\bin\\Debug\\net8.0\\MauiContract.dll";
    // сюда из изначальной матрицы будут записываться пути в формате "13": путь из 1 в 3 и наоборот
    // чтоб при отрисовке пути понимать, был ли он уже отрисован(содержится в нашем сете) или нет
    private HashSet<string> _existedGraphPaths = new();

    public MainPageViewModel()
    {
        PlaceholderText = "Enter integers for your matrix";
        PlaceHolderTextColor = Color.FromArgb("#b8b8b8");
    }

    // сет использованных для графов цветов
    public HashSet<string> UsedColorsForGraph = new();

    private static string PathToDll = "";


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
    [ObservableProperty] 
    string placeholderText;
    [ObservableProperty] 
    Color placeHolderTextColor;

    
    // AssemblyButton
    [ObservableProperty]
    bool dllLoadedSuccesfully; // от этого будет зависеть можно ли будет нажать enter в entry
    
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
            EntryTranslationY -= 5;
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
            EntryTranslationY += 5;
        }
        
        IsEnabledEntry = true;
        IsVisibleBorderButton = false;
    }


    [RelayCommand]
    private async Task<bool> LoadDll(string pathToDll)
    {
        var pickedFile = await PickFile();

        
        if (pickedFile != null)
        {
            Assembly assembly = null;
            // запасной вариант
            // using (var stream = await pickedFile.OpenReadAsync())
            // {
            //     var assemblyData = new byte[stream.Length];
            //     await stream.ReadAsync(assemblyData, 0, assemblyData.Length);
            //     assembly = Assembly.Load(assemblyData);
            // }
            assembly = Assembly.LoadFrom(pickedFile.FullPath);
            
            if (assembly != null)
            {
                // DLL успешно загружена, можно выполнять дальнейшие действия
                Debug.WriteLine("DLL успешно загружена.");
                
                // тут нужно проверить корректность сборки
                bool loadedDllIsValid = await IsValidDll(assembly);

                if (loadedDllIsValid)
                {
                    BackgroundColorAssemblyButton = Color.FromArgb("#2ef24f");
                    PlaceholderText = "Enter integers for your matrix";
                    PlaceHolderTextColor = Color.FromArgb("#b8b8b8");
                    MainAssembly = assembly;
                }
                else
                {
                    BackgroundColorAssemblyButton = Color.FromArgb("#ff1717");
                    // На время сохраняем текст
                    string backupText = PlaceholderText;
                    Color backupColor = PlaceHolderTextColor;

                    PlaceholderText = "Wrong assembly was chosen!";
                    PlaceHolderTextColor = Color.FromArgb("#ff1717");
                    MainAssembly = null;
                }
            }
            else
            {
                // DLL не загружена из-за ошибки
                Debug.WriteLine("Ошибка загрузки DLL.");
            }
        }
        else
        {
            Debug.WriteLine("Dll wasn't found or some error occured while reading");
        }

        return false;
    }


    private async Task<bool> IsValidDll(Assembly assembly)
    {
        var types = assembly.GetTypes();
        
        // по сути это правильно, ведь dll будет в папке bin вместе с exe приложения
        // но папки винды там нет
        // походу в папку C:\WINDOWS\system32\MauiContract.dll
        Assembly contractAssembly = Assembly.LoadFrom(PathToContract);

        var contractTypes = contractAssembly.GetTypes();

        bool contractIsImplemented = contractTypes.All((p) =>
        {
            return types.Any( (s) =>
            {
                if (p.IsGenericType && s.IsGenericType)
                {
                    var boundedP = p.MakeGenericType(typeof(int));
                    var boundedS = s.MakeGenericType(typeof(int));

                    return boundedP.IsAssignableFrom(boundedS);
                }

                return p.IsAssignableFrom(s);
            });
        });

        return contractIsImplemented;
    }
    
    
    private async void Button_Clicked()
    {
        // Вызываем метод для выбора файла DLL
        var pickedFile = await PickFile();

        if (pickedFile != null)
        {
            try
            {
                // Загружаем DLL-файл в память через поток
                Assembly assembly = null;
                using (var stream = await pickedFile.OpenReadAsync())
                {
                    var assemblyData = new byte[stream.Length];
                    await stream.ReadAsync(assemblyData, 0, assemblyData.Length);
                    assembly = Assembly.Load(assemblyData);
                }

                if (assembly != null)
                {
                    // DLL успешно загружена, можно выполнять дальнейшие действия
                    Debug.WriteLine("DLL успешно загружена.");
                }
                else
                {
                    // DLL не загружена из-за ошибки
                    Debug.WriteLine("Ошибка загрузки DLL.");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при загрузке DLL
                Debug.WriteLine($"Ошибка при загрузке DLL: {ex.Message}");
            }
        }
    }

        // Метод для выбора DLL-файла
    private async Task<FileResult?> PickFile()
    {
        var dllFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".dll" } },
            });
        
        try
        {
            // Вызываем метод для открытия проводника и выбора DLL-файла
            var pickedFile = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = dllFileType, // Указываем тип файла DLL
                PickerTitle = "Выберите DLL-файл" // Заголовок окна выбора файла
            });

            PathToDll = pickedFile.FullPath;
            
            return pickedFile;
        }
        catch (Exception ex)
        {
            // Обработка исключений при выборе файла
            Debug.WriteLine($"Ошибка при выборе файла: {ex.Message}");
            return null;
        }
    }
}