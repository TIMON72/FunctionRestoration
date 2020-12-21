using System.Windows.Controls;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Step1.xaml
    /// </summary>
    public partial class Step1 : UserControl
    {
        static Step1 instance;
        public static Step1 Instance
        {
            get
            {
                if (instance is null)
                    instance = new Step1();
                return instance;
            }
        }

        public Step1()
        {
            InitializeComponent();
        }
    }
}
