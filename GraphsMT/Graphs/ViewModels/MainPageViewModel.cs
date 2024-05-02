using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Forms;
using Graphs.Drawables;
using MauiContract;

namespace Graphs.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    // текущий путь до сборки с реализацией
    // D:\maui_last_stable\El_Prim\GraphsMT\MauiContractImplementation\bin\Debug\net8.0
    // текущая сборка, что активна
    public static Assembly MainAssembly = null;
    // Решатель, полученный из сборки
    public static IGraphSolver SolverPrl = null;
    // Решенная матрица смежности, по которой и будет ориентироваться рисовальщик
    public static int[,] SolvedMatrix = null;

    // Путь к контракту
    public const string PathToContract = "D:\\maui_last_stable\\El_Prim\\GraphsMT\\MauiContract\\bin\\Debug\\net8.0\\MauiContract.dll";

    // Позволяет избежать следующего случая:
    // приложение запустилось, открыл контексное меню проводника
    // закрыл, ничего не выбрав, и все окрасилось в красный
    // поэтому нужно понять, пыталились ли до этого сборку подгрузить
    private bool InitialLaunch = true;

    public MainPageViewModel()
    {
        // Задаем текст для Entry и цвет для placeholder
        PlaceholderText = "Enter integers for your matrix";
        PlaceHolderTextColor = Color.FromArgb("#b8b8b8");
    }
    
    
    // Это свойство по итогу так не пригодилось
    // изначально думал, что сделаю колесо загрузки
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    // В данном случае binding тайтла происходит через MainPage, поэтому это свойство тоже не пригодилось
    [ObservableProperty]
    string title;

    // Это тоже не пригодилось
    public bool IsNotBusy => !IsBusy;

    
    // BorderButton
    // Конпка для возврата entry для введения матрицы смежности
    [ObservableProperty]
    bool isVisibleBorderButton;
    [ObservableProperty]
    bool isEnabledBorderButton;
    
    
    // BorderEntry
    // Сам entry для введения матрицы
    // Смещение по Y для entry и кнопки указания сборки
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(AbsLayoutAssebmlyButton))]
    int entryTranslationY;
    [ObservableProperty]
    bool isEnabledEntry;
    // Введеный в entry текст
    [ObservableProperty]
    string enteredBorderEntryText;
    [ObservableProperty] 
    string placeholderText;
    [ObservableProperty] 
    Color placeHolderTextColor;

    
    // AssemblyButton
    [Obsolete]
    [ObservableProperty]
    bool dllLoadedSuccesfully; // от этого будет зависеть можно ли будет нажать enter в entry
    // чет так и не пригодилось
    // Красный или зеленый цвет кнопки указания сборки в зависимости от успеха указания
    [ObservableProperty]
    Color backgroundColorAssemblyButton;
    // Абсолютные кординаты для кнопки сборки под entry
    public Rect AbsLayoutAssebmlyButton => new Rect(20, EntryTranslationY + 65, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
    


    // Этот же метод впоследствии запустит асинхронно метод решения задачи на алгоритм Прима
    [RelayCommand]
    public async Task RemoveEntryAsync() // aka EnterCommand
    {
        if (SolverPrl == null)
        {
            Debug.WriteLine("Класс SolverPrl равен null");
            EnteredBorderEntryText = "";
            PlaceholderText = "Dll was not chosen!";
            PlaceHolderTextColor = Color.FromArgb("#ff1717");
            
            return;
        }
        
        PlaceholderText = "Enter integers for your matrix";
        PlaceHolderTextColor = Color.FromArgb("#b8b8b8");
        
        
        var matrixToInput = await Task.Run(() => ConvertInputToMatrix(EnteredBorderEntryText));
        bool isValidInput = SolverPrl.ValidateMatrix(matrixToInput);

        if (!isValidInput)
        {
            Debug.WriteLine("Не валидная матрица на вход");
            EnteredBorderEntryText = "";
            PlaceholderText = "Invalid matrix was given!";
            PlaceHolderTextColor = Color.FromArgb("#ff1717");
            
            return;
        }
        
        // Насколько должен съехать entry с assemblyButton, чтоб их не было видно
        // Не очень надежный способ, но чет я поленился городить еще несколько статических переменных,
        // которые будут узнавать итоговую высоту этих двух штук
        // anyway, должно хватить
        var target =  Screen.PrimaryScreen.Bounds.Height/4;
        
        IsEnabledEntry = false;
        IsVisibleBorderButton = true;
        
        while (EntryTranslationY > -target)
        {
            await Task.Delay(10);
            EntryTranslationY -= 5;
        }

        // Матрица уже получена, можем очистить entry
        EnteredBorderEntryText = "";
        
        IsEnabledBorderButton = true;

        // Вызываем метод отрисовки графа на странице MainPage
        // Делаем метод решения асинхронным, чтоб UI оставался отзывчивым
        // пока решение будет готовиться, UI продолжит работу
        await SolveAndDrawMatrix(matrixToInput);
        GraphicsDrawable.CoordsToDraw = await Task.Run((() => GenerateCoordinates(matrixToInput.GetLength(0))));
        
        // тут, после того, как мы решили матрицу и сгенерировали кординаты, нужно обновить граф
        // с помощью validate()
        // ...
        // вот эта строка обязательна
        // кстати, вот из-за строки ниже, скорее всего и происходит зависание, тк она синхронная, и пока там 
        // метод все рисует, UI поток блочится
        // тк он void
        MainPage.ReDrawGraph();
    }
    
    
    public Dictionary<int, ValueTuple<int, int>> GenerateCoordinates(int numberOfVertexes)
    {
        // пускай радиус вершины на данном этапе будет равен 75
        var result = new Dictionary<int, ValueTuple<int, int>>();

        for (int i = 0; i < numberOfVertexes; i++)
        {
            GenerateNewCoords:
            int x = new Random().Next(100, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width-100);
            int y = new Random().Next(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-200);

            foreach (var vertex in result)
            {
                if (Math.Pow(Math.Pow(x - vertex.Value.Item1, 2) + Math.Pow(y - vertex.Value.Item2, 2) , 0.5) < 150)
                    goto GenerateNewCoords;
            }
            
            result[i] = (x, y);
        }

        return result;
    }


    async Task SolveAndDrawMatrix(int[,] matrix)
    {
        var result = await Task.Run(() => SolverPrl.SolvePrl(matrix));
        
        SolvedMatrix = result;
    }


    async Task<int[,]> ConvertInputToMatrix(string input)
    {
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int[] numbers = Array.ConvertAll(parts, int.Parse);

        if (Math.Abs(Math.Pow(numbers.Length, 0.5) - (int)Math.Pow(numbers.Length, 0.5)) > double.Epsilon || numbers.Length == 0)
        {
            return new int[0, 1]; // возвращаю заведомо неправильную матрицу
        }

        int square = (int)Math.Pow(numbers.Length, 0.5);
        int[,] result = new int[square, square];
        int currentindex = 0;
        for (int i = 0; i < square; i++)
        {
            for (int j = 0; j < square; j++)
            {
                result[i, j] = numbers[currentindex];
                ++currentindex;
            }
        }

        return result;
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