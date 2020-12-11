using FunctionRestoration.Windows;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;

namespace FunctionRestoration.Models
{
    class Step1
    {
        public PlotModel Model { get; private set; } = new PlotModel();

        public static IList<DataPoint> Points { get; private set; } = new List<DataPoint>();

        public Step1()
        {
            Model = new PlotModel { Title = "Сгенерированная выборка" };
            GenerateSamaple();
        }
        /// <summary>
        /// Генерация выборки
        /// </summary>
        void GenerateSamaple()
        {
            ScatterSeries ss = new ScatterSeries
            {
                MarkerSize = 3,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0, 200, 0)
            };
            for (double i = 0.1; i < 10;  i += 0.1)
            {
                DataPoint dp = new DataPoint(i, Main.Func(i));
                Points.Add(dp);
                ss.Points.Add(new ScatterPoint(dp.X, dp.Y));
            }
            Model.Series.Add(ss);
        }
    }
}
