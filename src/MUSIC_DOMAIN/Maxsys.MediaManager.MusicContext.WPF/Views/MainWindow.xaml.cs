using System.Windows;

namespace Maxsys.MediaManager.MusicContext.WPF
{
    //* Using custom IMainContentOwner/IAppCloser implementation classes approach
    // just need to add:
    //      services.AddSingleton<IMainContentCloser, MainWindowContentCloser>();
    public partial class MainWindow : Window
    {
        //private readonly MainWindowViewModel _viewModel;

        public MainWindow(/*ILogger<MainWindow> logger, ILogger<MainWindowViewModel> viewModelLogger, 
            NavigationStore navigationStore, IServiceProvider serviceProvider*/)
        {
            InitializeComponent();

            //_viewModel = new MainWindowViewModel(viewModelLogger, navigationStore, new MainWindowContentOwner(this), new MainWindowAppCloser(this), serviceProvider);

            //DataContext = _viewModel; //= new(logger, host, new MainWindowContentOwner(this), new MainWindowAppCloser(this));

            //Loaded += async (o, s) => { await _viewModel.LoadedCatalogsAsync(); };
        }
    }

    //*/

    /* Using self IMainContentOwner/IAppCloser interfaces implementation approach
    // Dependency injection -> services.AddSingleton<IMainContentCloser>(sp => sp.GetRequiredService<MainWindow>());
    public partial class MainWindow : Window, IMainContentOwner, IAppCloser
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(ILogger<MainWindow> logger, IHost host)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, host, this, this);

            Loaded += async (o, s) => { await _viewModel.LoadCatalogsAsync(); };
        }

        internal void UpdateToolBarText(string text)
        {
            ToolBarText.Text = text;
        }

        public void SetMainContent(object content)
        {
            if (content is IView view)
                this.ContentContainer.Content = view;
        }

        public void CloseMainContent()
        {
            if (this.ContentContainer.Content is not null)
            {
                var currentView = this.ContentContainer.Content;
                this.ContentContainer.Content = null;
                GC.SuppressFinalize(currentView);
            }
        }

        public void CloseApp() => this.Close();
    }

    //*/

    /* ORIGINAL
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHost _host;

        public ICommand OpenViewCommand { get; set; }

        //public MainWindow(IServiceProvider serviceProvider, ILogger<MainWindow> logger)
        public MainWindow(IHost host, ILogger<MainWindow> logger)
        {
            InitializeComponent();

            _host = host;
            _serviceProvider = _host.Services.CreateScope().ServiceProvider;
            _logger = logger;

            OpenViewCommand = new WPFOpenViewCommand(OpenViewAction);

            DataContext = this;

            Loaded += (o, s) => { _logger.LogInformation("MainWindow Loaded"); };
        }

        private void OpenViewAction(Type viewType)
        {
            //var ctx = _serviceProvider.GetRequiredService<MusicAppContext>();
            //_logger.LogWarning($"MainWindow MusicAppContext-{ctx.ContextId}");

            _logger.LogDebug("OpenViewAction called.");

            try
            {
                CloseMainContent();

                IView view;
                view = (IView)_serviceProvider.GetRequiredService(viewType);
                //view = (IView)_serviceProvider.GetRequiredService<CreateMusicCatalogView>();

                //Console.WriteLine("Using Scope()");
                //using (var scope = _host.Services.CreateScope())
                //{
                //    view = (IView)scope.ServiceProvider.GetRequiredService(viewType);
                //    SetMainContent(view);
                //    Console.WriteLine("Disposing Scope()...");
                //}
                //Console.WriteLine("Scope() disposed.");

                SetMainContent(view);

                _logger.LogDebug($"{viewType.CatalogName} in the ContentContainer");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening view.");
            }
        }

        internal void CloseMainContent()
        {
            if (ContentContainer.Content is not null)
            {
                if (ContentContainer.Content is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                else
                {
                    var currentView = ContentContainer.Content;

                    GC.SuppressFinalize(currentView);
                    GC.Collect();
                }
                ContentContainer.Content = null;
            }
        }
        internal void SetMainContent(IView view)
        {
            ContentContainer.Content = view;
        }
        internal void UpdateToolBarText(string text)
        {
            ToolBarText.Text = text;
        }

        private void Menu_File_Exit_Click(object sender, EventArgs e) => this.Close();
    }
    //*/
}