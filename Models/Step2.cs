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
            Random random = new Random();
            foreach (DataPoint dp in Step1.Points)
            {
                double noiseY = random.Next(-5, 5) * 0.05;
                Points.Add(new DataPoint(dp.X, dp.Y + noiseY));
                ss.Points.Add(new ScatterPoint(dp.X, dp.Y + noiseY));
            }
            Model.Series.Add(ss);
        }
    }
}
