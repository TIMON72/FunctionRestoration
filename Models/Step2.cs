using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;

namespace FunctionRestoration.Models
{
    class Step2
    {
        
        public PlotModel Model { get; private set; } = new PlotModel();

        public static IList<DataPoint> Points { get; private set; } = new List<DataPoint>();

        public Step2()
        {
            Model = new PlotModel { Title = "Выборка с аддитивной помехой" };
            AddAdditiveNoise();
        }
        
        /// <summary>
        /// Погрешность отклонения
        /// </summary>
        /// <param name="deviation">Параметр отклонения от реального значения в %</param>
        /// <returns>Значение отклонения</returns>
        private double DeviationError(int deviation)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (DataPoint dp in Step1.Points)
            {
                if (dp.Y > max)
                    max = dp.Y;
                if (dp.Y < min)
                    min = dp.Y;
            }
            return Math.Abs(max - min) * deviation * 0.01;
        }
        /// <summary>
        /// Добавление помехи
        /// </summary>
        void AddAdditiveNoise()
        {
            ScatterSeries ss = new ScatterSeries
            {
                MarkerSize = 3,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(200, 200, 0)
            };
            double deviationError = DeviationError(5);
            Random random = new Random();
            foreach (DataPoint dp in Step1.Points)
            {
                double noiseY = random.Next(-5, 5) * deviationError;
                Points.Add(new DataPoint(dp.X, dp.Y + noiseY));
                ss.Points.Add(new ScatterPoint(dp.X, dp.Y + noiseY));
            }
            Model.Series.Add(ss);
        }
    }
}
