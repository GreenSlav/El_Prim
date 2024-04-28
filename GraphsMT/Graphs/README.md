### Dependencies и все такое:
- Указать reference проекта с приложением к System.Windows.Forms.dll 
- Скачать nugets к проекту: Microsoft.Win32.SystemEvents
- При рисовании графов нужно брать во внимание: разрешение экрана и кол-во вершин
- При отдалении Entry подлагивает, нврн из-за того, что там постоянно во ViewModel создается класс Rect,
который относится к к кнопке ChooseAssembly