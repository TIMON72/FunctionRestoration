using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FunctionRestoration.Windows
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public static double Func(double x) => Math.Sin(x * 5) * 0.5;
        public static double DeltaX { get; set; } = 0.01;

        int stepNum = 0;
        readonly List<dynamic> StepsList = new List<dynamic>
        {
            Step0.Instance,
            Step1.Instance,
            Step2.Instance,
            Step3.Instance
        };
        /// <summary>
        /// Конструктор
        /// </summary>
        public Main()
        {
            InitializeComponent();
            DataContext = Step3.Context;
            Step.Content = StepsList[0];
            B_PreviousStep.Visibility = Visibility.Hidden;
            L_Degree.Visibility = Visibility.Hidden;
            TB_Degree.Visibility = Visibility.Hidden;
            L_Accuracy.Visibility = Visibility.Hidden;
            TB_Accuracy.Visibility = Visibility.Hidden;
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
            {
                B_NextStep.Visibility = Visibility.Hidden;
                L_Degree.Visibility = Visibility.Visible;
                TB_Degree.Visibility = Visibility.Visible;
                L_Accuracy.Visibility = Visibility.Visible;
                TB_Accuracy.Visibility = Visibility.Visible;
            }           
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
            B_PreviousStep.Visibility = Visibility.Hidden;
            L_Degree.Visibility = Visibility.Hidden;
            TB_Degree.Visibility = Visibility.Hidden;
            L_Accuracy.Visibility = Visibility.Hidden;
            TB_Accuracy.Visibility = Visibility.Hidden;
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
            // Проверка на пустоту
            if (input == "")
                return;
            // Проверка на число
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
            // Проверка на диапазон
            if (inputNum < 1 || inputNum > 10)
            {
                MessageBox.Show("Введите число в диапазоне [1; 9]", "Ошибка ввода");
                return;
            }
            BindingExpression be = TB_Degree.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }
    }
}
