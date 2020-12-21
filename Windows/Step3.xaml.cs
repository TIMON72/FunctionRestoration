using System.Windows.Controls;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Step3.xaml
    /// </summary>
    public partial class Step3 : UserControl
    {
        static Step3 instance;

        public static object Context  { private set; get; }
        public static Step3 Instance
        {
            get
            {
                if (instance is null)
                    instance = new Step3();
                return instance;
            }
        }

        public Step3()
        {
            InitializeComponent();
            Context = DataContext;
        }
    }
}
