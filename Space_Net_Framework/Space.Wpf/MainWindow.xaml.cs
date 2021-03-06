﻿using Space.Application;
using Space.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Space.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpaceProcessor _processor;
        private List<Globe> _globes;

        public MainWindow()
        {
            _processor = new SpaceProcessor(1);

            _globes = new List<Globe>();

            _globes.Add(new Globe
            {
                X = 400,
                Y = 300,
                Mass = 900,
                Name = "Sun",
                SpeedX = 0,
                SpeedY = 0,
                Color = Color.FromArgb(255, 255, 0, 0),
                Radius = 50,
                Id = Guid.NewGuid()
            });
            _globes.Add(new Globe
            {
                X = 1000,
                Y = 300,
                Mass = 42,
                Name = "Earth",
                SpeedX = 0,
                SpeedY = 1,
                Color = Color.FromArgb(255, 0, 255, 0),
                Radius = 10,
                Id = Guid.NewGuid()
            });
            _globes.Add(new Globe
            {
                X = 1040,
                Y = 300,
                Mass = 1,
                Name = "Moon",
                SpeedX = 0,
                SpeedY = 2,
                Color = Color.FromArgb(255, 0, 0, 255),
                Radius = 5,
                Id = Guid.NewGuid()
            });
            _globes.Add(new Globe
            {
                X = 1500,
                Y = 600,
                Mass = 1,
                Name = "Comet",
                SpeedX = -2,
                SpeedY = 0,
                Color = Color.FromArgb(255, 0, 0, 0),
                Radius = 5,
                Id = Guid.NewGuid()
            });

            InitializeComponent();

            DrawGlobes();
            DrawCenter(_processor.GenerateMassCenter(_globes));

            var spaceThread = new Thread(() => ProcessSpace());
            spaceThread.Start();
        }

        private void ProcessSpace()
        {
            while (true)
            {
                Thread.Sleep(20);

                ProcessGobes();

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    DrawGlobes();
                    DrawCenter(_processor.GenerateMassCenter(_globes, true));
                }));
            };
        }

        private void ProcessGobes(MassCenter center)
        {
            _globes.ForEach(globe =>
            {
                globe = _processor.CalculateSpeed(globe, center);
                globe = _processor.MoveGlobe(globe);
            });
        }

        private void ProcessGobes()
        {
            _globes.ForEach(globe =>
            {
                _globes
                .Where(influentingGlobe => influentingGlobe.Id != globe.Id)
                .ToList()
                .ForEach(influentingGlobe =>
                {
                    globe = _processor.CalculateSpeed(globe, influentingGlobe);
                });

                globe = _processor.MoveGlobe(globe);
            });
        }

        private void DrawGlobes()
        {
            canvas.Children.Clear();

            _globes.ForEach(globe =>
            {
                var el = new Ellipse();

                el.Width = globe.Radius * 2;
                el.Height = globe.Radius * 2;

                el.SetValue(Canvas.LeftProperty, globe.X - globe.Radius);
                el.SetValue(Canvas.TopProperty, globe.Y - globe.Radius);

                el.Fill = new SolidColorBrush {
                    Color = globe.Color
                };

                canvas.Children.Add(el);
            });
        }

        private void DrawCenter(MassCenter center)
        {
            var vLine = new Line();
            vLine.Stroke = Brushes.Black;
            vLine.X1 = center.X;
            vLine.X2 = center.X;
            vLine.Y1 = center.Y - 5;
            vLine.Y2 = center.Y + 5;
            vLine.StrokeThickness = 1;
            canvas.Children.Add(vLine);

            var hLine = new Line();
            hLine.Stroke = Brushes.Black;
            hLine.X1 = center.X - 5;
            hLine.X2 = center.X + 5;
            hLine.Y1 = center.Y;
            hLine.Y2 = center.Y;
            hLine.StrokeThickness = 1;
            canvas.Children.Add(hLine);
        }
    }
}
