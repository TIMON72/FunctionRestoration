using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public static double Func(double x) => 8 * x - 3;

        int stepNum = 0;
        readonly List<dynamic> StepsList = new List<dynamic>
        {
            new Step0(),
            new Step1(),
            new Step2(),
            new Step3()
        };

        public Main()
        {
            InitializeComponent();
            Step.Content = StepsList[0];
            B_PreviousStep.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Событие нажатия кнопки "Далее"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_NextStep_Click(object sender, RoutedEventArgs e)
        {
            stepNum++;
            Step.Content = StepsList[stepNum];
            B_PreviousStep.Visibility = Visibility.Visible;
            if (stepNum == StepsList.Count - 1)
                B_NextStep.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Событие нажатия кнопки "Назад"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_PreviousStep_Click(object sender, RoutedEventArgs e)
        {
            stepNum--;
            Step.Content = StepsList[stepNum];
            B_NextStep.Visibility = Visibility.Visible;
            if (stepNum == 0)
                B_PreviousStep.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Событие изменения значения поля ввода "Ранг полинома"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TB_Degree_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TB_Degree.Text;
            if (input == "")
                return;
            int inputNum;
            try
            {
                inputNum = int.Parse(input);
            } 
            catch (Exception)
            {
                MessageBox.Show("Введите число", "Ошибка ввода");
                return;
            }
            if (inputNum < 1 || inputNum > 9)
            {
                MessageBox.Show("Введите число в диапазоне [1; 9]", "Ошибка ввода");
                return;
            }
            Models.Step3.Degree = inputNum;
            StepsList[3] = new Step3();
            if (stepNum == 3)
                Step.Content = StepsList[3];
        }
    }
}
