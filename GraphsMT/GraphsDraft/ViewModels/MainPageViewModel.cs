using CommunityToolkit.Mvvm.ComponentModel;

namespace GraphsDraft.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    
    private HashSet<string> VisitedVertexes = new();
    
    [ObservableProperty] 
    private string entryText;

    public async Task ShowGraph()
    {
        
    }
    
    int[,] adjacencyMatrix7 = {
        {0, 3, 1, 2, 2},
        {3, 0, 8, 3, 1},
        {1, 8, 0, 1, 4},
        {1, 1, 1, 0, 6},
        {2, 1, 4, 6, 0}
    };
    
    
}