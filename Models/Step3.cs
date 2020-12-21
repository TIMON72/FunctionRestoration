using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;

namespace FunctionRestoration.Models
{
    class Step3 : ViewModel
    {
        public PlotModel Model { get; private set; } = new PlotModel();
        public static IList<DataPoint> Points { get; private set; } = new List<DataPoint>();
        public int degree = 1;
        public int Degree
        {
            get => degree;
            set
            {
                degree = value;
                OnPropertyChanged(nameof(Degree));
                Approximation();
            }
        }
        public double accuracy = 0;
        public double Accuracy
        {
            get => accuracy;
            set
            {
                accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }

        static int degree_cur = 0;
        static double[,] FI;
        static double[] A;
        double[] Y_model;

        public Step3()
        {
            Model = new PlotModel { Title = "Аппроксимация" };
            Approximation();
        }
        /// <summary>
        /// Вычисление полиномов Чебышева
        /// </summary>
        void CalculateChebyshevPolynom()
        {
            int n = degree_cur;
            for (int i = 0; i < Points.Count; i++)
                if (n == 0)
                    FI[0, i] = 1;
                else if (n == 1)
                    FI[1, i] = Points[i].X;
                else
                    FI[n, i] = 2 * Points[i].X * FI[n - 1, i] - FI[n - 2, i];
        }
        /// <summary>
        /// Вычисление коэффициентов
        /// </summary>
        void CalculateAlphas()
        {
            int n = degree_cur;
            double sum = 0;
            double sum2 = 0;
            //for (int i = 1; i <= Degree; i++)
            //    sum =+ Points[i].Y * FI[n, i];
            //A[n] = 2 * sum / Degree;
            for (int i = 0; i <= Degree; i++)
            {
                sum += Points[i].Y * FI[n, i];
                sum2 += Math.Pow(FI[n, i], 2);
            }
            A[n] = sum / sum2;
        }
        /// <summary>
        /// Изменение размера двумерного массива
        /// </summary>
        /// <param name="array">Изменяемый массив</param>
        void Resize(ref double[] array)
        {
            if (array is null)
            {
                array = new double[1];
                return;
            }
            int size_new = array.Length + 1;
            double[] new_array = new double[size_new];
            for (int i = 0; i < array.Length; i++)
                new_array[i] = array[i];
            array = new_array;
        }
        void Resize(ref double[,] array)
        {
            if (array is null)
            {
                array = new double[1, Points.Count];
                return;
            }
            int size_new = array.GetLength(0) + 1;
            double[,] new_array = new double[size_new, Points.Count];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    new_array[i, j] = array[i, j];
            array = new_array;
        }
        /// <summary>
        /// Вычисление оценочных точек
        /// </summary>
        void Calculate()
        {
            // Если ранг задан больше текущего, то вычислить параметры оценочной функции Y
            while (degree_cur < Degree)
            {
                Resize(ref FI);
                Resize(ref A);
                CalculateChebyshevPolynom();
                CalculateAlphas();
                degree_cur++;
            }
            // Вычисление оценочной Y
            Y_model = new double[Points.Count];
            for (int i = 0; i < Points.Count; i++)
            {
                double sum = 0;
                for (int n = 0; n < Degree; n++)
                    sum += A[n] * FI[n, i];
                Y_model[i] = sum - 1 / 2 * A[0];
            }
        }
        /// <summary>
        /// Вычисление точности модели
        /// </summary>
        void CalculateAccuracy()
        {
            double sum = 0;
            for (int i = 0; i < Points.Count; i++)
                sum += Math.Pow(Step1.Points[i].Y - Y_model[i], 2);
            Accuracy = Math.Sqrt(sum / (Points.Count));
        }
        /// <summary>
        /// Апроксимация
        /// </summary>
        public void Approximation()
        {
            Points.Clear();
            Model.Series.Clear();
            // Отображаем точки
            ScatterSeries ss = new ScatterSeries
            {
                MarkerSize = 3,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(200, 200, 0)
            };
            foreach (DataPoint dp in Step2.Points)
            {
                Points.Add(new DataPoint(dp.X, dp.Y));
                ss.Points.Add(new ScatterPoint(dp.X, dp.Y));
            }
            Model.Series.Add(ss);
            // Вычисление оценочных точек
            Calculate();
            // Вычисление точности модели
            CalculateAccuracy();
            // Записываем новые (смоделированные) точки в список
            for (int i = 0; i < Points.Count; i++)
                Points[i] = new DataPoint(Points[i].X, Y_model[i]);
            // Отображаем аппроксимирующую функцию
            FunctionSeries fs1 = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(200, 0, 0)
            };
            foreach (DataPoint dp in Points)
                fs1.Points.Add(new DataPoint(dp.X, dp.Y));
            Model.Series.Add(fs1);
            // Обновление графа
            Model.InvalidatePlot(true);
        }
    }
}
