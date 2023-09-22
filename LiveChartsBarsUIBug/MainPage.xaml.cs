namespace LiveChartsBarsUIBug
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
            cbVisibility.IsChecked = true;
            btnUpdate.Focus();
        }
    }
}