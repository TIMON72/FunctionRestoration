using System.Windows.Controls;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Step0.xaml
    /// </summary>
    public partial class Step0 : UserControl
    {
        static Step0 instance;
        public static Step0 Instance 
        {
            get
            {
                if (instance is null)
                    instance = new Step0();
                return instance;
            }
        }

        public Step0()
        {
            InitializeComponent();
        }
    }
}