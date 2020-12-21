using System.Windows.Controls;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Step2.xaml
    /// </summary>
    public partial class Step2 : UserControl
    {
        static Step2 instance;
        public static Step2 Instance
        {
            get
            {
                if (instance is null)
                    instance = new Step2();
                return instance;
            }
        }

        public Step2()
        {
            InitializeComponent();
        }
    }
}
