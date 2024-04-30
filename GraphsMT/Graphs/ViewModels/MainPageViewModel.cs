using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;
using MauiContract;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    // текущий путь до сборки с реализацией
    // D:\maui_last_stable\El_Prim\GraphsMT\MauiContractImplementation\bin\Debug\net8.0
    public static Assembly MainAssembly = null;
    public static IGraphSolver SolverPrl = null;

    public const string PathToContract = "D:\\maui_last_stable\\El_Prim\\GraphsMT\\MauiContract\\bin\\Debug\\net8.0\\MauiContract.dll";
    // сюда из изначальной матрицы будут записываться пути в формате "13": путь из 1 в 3 и наоборот
    // чтоб при отрисовке пути понимать, был ли он уже отрисован(содержится в нашем сете) или нет
    private HashSet<string> _existedGraphPaths = new();

    // Позволяет избежать следующего случая:
    // приложение запустилось, открыл контексное меню проводника
    // закрыл, ничего не выбрав, и все окрасилось в красный
    private bool InitialLaunch = true;

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
    


    // Этот же метод впоследствии запустит асинхронно метод решения задачи на алгоритм Прима
    [RelayCommand]
    public async Task RemoveEntryAsync()
    {
        // initialXBorderEntryPos = EntryTranslationY;  коммент птмчт всегда 0 по сути
        
        string inputMatrix = EnteredBorderEntryText;

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

        //SolverPrl.SolvePrl(new int[3, 4]);
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
        try
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
                        BackgroundColorAssemblyButton = Color.FromArgb("#1fa136");
                        PlaceholderText = "Enter integers for your matrix";
                        PlaceHolderTextColor = Color.FromArgb("#b8b8b8");
                        MainAssembly = assembly;

                        // Сюда запихну экземпляр класса IGraphSolver
                        object graphSolver = null;
                        //MethodInfo methodToInvoke = null;

                        foreach (var type in assembly.GetTypes())
                        {
                            if (type != null && typeof(IGraphSolver).IsAssignableFrom(type))
                            {
                                graphSolver = Activator.CreateInstance(type);
                                // вот тут непонятно, надо ли метод доставать, учитывая, что он зависит от содержимого класса IGraphSolver
                                //methodToInvoke = type.GetMethod("SolvePrl");
                                break;
                            }
                        }
                        
                        SolverPrl = graphSolver as IGraphSolver;
                        InitialLaunch = false;
                        
                        return true;
                    }
                    else
                    {
                        BackgroundColorAssemblyButton = Color.FromArgb("#ff1717");
                        // На время сохраняем текст

                        PlaceholderText = "Wrong assembly was chosen!";
                        PlaceHolderTextColor = Color.FromArgb("#ff1717");
                        MainAssembly = null;

                        return false;
                    }
                }
                else
                {
                    // DLL не загружена из-за ошибки
                    Debug.WriteLine("Ошибка загрузки DLL.");
                    BackgroundColorAssemblyButton = Color.FromArgb("#ff1717");
                    PlaceholderText = "Dll loading error!";
                    PlaceHolderTextColor = Color.FromArgb("#ff1717");
                    MainAssembly = null;

                    return false;
                }
            }
            else
            {
                // ----
                if (MainAssembly != null || InitialLaunch)
                {
                    return true;
                }
                
                Debug.WriteLine("Dll wasn't found or some error occured while reading");
                BackgroundColorAssemblyButton = Color.FromArgb("#ff1717");
                PlaceholderText = "Error occured while reading dll!";
                PlaceHolderTextColor = Color.FromArgb("#ff1717");
                InitialLaunch = false;
                MainAssembly = null;

                return false;
            }
        }
        catch (Exception e)
        {
            if (MainAssembly != null)
            {
                return true;
            }
            
            Debug.WriteLine(e);
            BackgroundColorAssemblyButton = Color.FromArgb("#ff1717");
            PlaceholderText = "Wrong assembly was chosen!";
            PlaceHolderTextColor = Color.FromArgb("#ff1717");
            InitialLaunch = false;
            MainAssembly = null;

            return false;
        }
        
    }


    private async Task<bool> IsValidDll(Assembly assembly)
    {
        var types = assembly.GetTypes();
        
        // по сути это правильно, ведь dll будет в папке bin вместе с exe приложения
        // но папки винды там нет
        // походу в папку C:\WINDOWS\system32\MauiContract.dll
        Assembly contractAssembly = Assembly.LoadFrom(PathToContract);

        var contractTypes = contractAssembly.GetTypes();

        // чет где-то налажал, херово работает
        // а нет, все норм
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