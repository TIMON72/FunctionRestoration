using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionRestoration.Models
{
    class Step3
    {

        public PlotModel Model { get; private set; } = new PlotModel();

        public static IList<DataPoint> Points { get; private set; } = new List<DataPoint>();

        public static double Func(double a, double x, double b) => a * x + b;

        public static int Degree { get; set; } = 1;

        static double[] H = new double[10];
        //static double[] P = new double[10];
        //static double[] FI = new double[10];
        //static double[] A = new double[10];

        double x_delta = 0.1;
        int degree_cur = 0;

        double[,] P;
        double[,] FI;
        double[,] A;
        double[] a;
        double[,] Y_model;

        public Step3()
        {
            Model = new PlotModel { Title = "Аппроксимация" };
            //Test1();
            Test2();
        }

        void Test1()
        {
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
            // Ищем коэффициенты аппроксимирующей функции
            int n = Step2.Points.Count;
            double sum_x = 0, sum_y = 0, sum_x2 = 0, sum_xy = 0;
            for (int i = 0; i < n; i++)
            {
                sum_x += Step2.Points[i].X;
                sum_y += Step2.Points[i].Y;
                sum_x2 += Math.Pow(Step2.Points[i].X, 2);
                sum_xy += Step2.Points[i].X * Step2.Points[i].Y;
            }
            double a = (n * sum_xy - (sum_x * sum_y)) / (n * sum_x2 - sum_x * sum_x);
            double b = (sum_y - a * sum_x) / n;
            // Отображаем аппроксимирующую функцию
            FunctionSeries fs1 = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(200, 0, 0)
            };
            foreach (DataPoint dp in Points)
                fs1.Points.Add(new DataPoint(dp.X, Func(a, dp.X, b)));
            Model.Series.Add(fs1);
            // Отображаем исходную функцию
            FunctionSeries fs2 = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(0, 200, 0)
            };
            foreach (DataPoint dp in Step1.Points)
                fs1.Points.Add(new DataPoint(dp.X, dp.Y));
            Model.Series.Add(fs2);
        }

        void CalculateLegendreOrthogonalPolynom(double x, int i)
        {
            int n = degree_cur;
            if (n == 0)
                P[0, i] = 1;
            else if (n == 1)
                P[1, i] = x;
            else
                P[n, i] = ((2 * n - 1) * x * P[n - 1, i] - (n - 1) * P[n - 2, i]) / n;
        }

        void CalculateLegendreOrthonormalPolynom(int i)
        {
            int n = degree_cur;
            FI[n, i] = Math.Sqrt((double)(2 * n + 1) / 2) * P[n, i];
        }

        void CalculateAlphas(double x, int i)
        {
            int n = degree_cur;

            double sum = 0;
            for (int ii = 1; ii < x / x_delta; ii++)
                sum += Points[ii].Y * FI[n, ii] * x_delta;
            A[n, i] = sum;

            //a[n] = (double)2 / (2 * n + 1);
        }

        void CalculateAlphas2()
        {
            int n = degree_cur;

            double sum = 0;
            for (int i = 1; i < Points.Count && i < Points[i].X / x_delta; i++)
                sum += Points[i].Y * FI[n, i] * x_delta;
            a[n] = sum;

            //a[n] = (double)2 / (2 * n + 1);
        }

        void CalculateAlphas3()
        {
            int n = degree_cur;
            double sum = 0;

            for (int i = 0; i < Points.Count; i++)
                sum = Points[i].Y * FI[n, i];
            a[0] = sum / n;

            for (int i = 1; i < Points.Count; i++)
                sum += Points[i].Y * FI[n, i] * x_delta;
            a[n] = sum;
        }

        void CalculateY()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                CalculateLegendreOrthogonalPolynom(Points[i].X, i);
                CalculateLegendreOrthonormalPolynom(i);
                //CalculateAlphas(Points[i].X, i);
                //CalculateAlphas2();
            }




            for (int i = 0; i < Points.Count; i++)
            {
                double polynom_cur = FI[degree_cur, i];//A[degree_cur, i] * FI[degree_cur, i];
                if (degree_cur < 1)
                    Y_model[degree_cur, i] = polynom_cur;
                else
                    Y_model[degree_cur, i] = FI[degree_cur - 1, i] + polynom_cur;//A[degree_cur + 1, i] * FI[degree_cur + 1, i];

                //double polynom_cur = a[degree_cur] * FI[degree_cur, i];
                //if (degree_cur < 1)
                //    Y_model[degree_cur, i] = polynom_cur;
                //else
                //    Y_model[degree_cur, i] = a[degree_cur - 1] * FI[degree_cur - 1, i] + polynom_cur;

                //double polynom_cur = A[degree_cur, i] * FI[degree_cur, i];
                //if (degree_cur < 1)
                //    Y_model[degree_cur, i] = polynom_cur;
                //else
                //    Y_model[degree_cur, i] = A[degree_cur - 1, i] * FI[degree_cur - 1, i] + polynom_cur;
            }

        }

        void Resize(ref double[,] array)
        {
            int size_new = array.GetLength(0) + 1;
            double[,] new_array = new double[size_new, Points.Count];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    new_array[i, j] = array[i, j];
            array = new_array;
        }

        void Test2()
        {
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

            P = new double[1, Points.Count];
            //for (int i = 0; i < Points.Count; i++)
            //    P[0, i] = 1;
            FI = new double[1, Points.Count];
            A = new double[1, Points.Count];
            a = new double[Degree];
            Y_model = new double[1, Points.Count];
            while (degree_cur < Degree)
            {
                CalculateY();
                if (degree_cur < Degree - 1)
                {
                    Resize(ref Y_model);
                    Resize(ref P);
                    Resize(ref FI);
                    Resize(ref A);
                }
                degree_cur++;
            }
            degree_cur = Degree - 1;

            for (int i = 0; i < Points.Count; i++)
                Points[i] = new DataPoint(Points[i].X, Y_model[degree_cur, i]);

            //for (int n = degree_cur; n < degree; n++)
            //    for (int i = 0; i < Points.Count; i++)
            //    {
            //        CalculateLegendreOrthogonalPolynom(Points[i].X, n);
            //    }    

            //Y_model[n, i] = GetModelY(Points[i].X, n);

            // Отображаем аппроксимирующую функцию
            FunctionSeries fs1 = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(200, 0, 0)
            };
            foreach (DataPoint dp in Points)
                fs1.Points.Add(new DataPoint(dp.X, dp.Y));
            Model.Series.Add(fs1);
        }
    }
}
