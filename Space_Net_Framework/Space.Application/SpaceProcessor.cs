using Space.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application
{
    public class SpaceProcessor
    {
        public double G
        {
            get;
            private set;
        }

        public SpaceProcessor(int g = 1)
        {
            G = g;
        }

        public MassCenter GenerateMassCenter(List<Globe> globes, bool chained)
        {
            var mc = new MassCenter();

            globes.OrderByDescending(g => g.Mass).ToList().ForEach(globe =>
            {
                //Первый объект?
                if (mc.Mass == 0)
                {
                    mc.X = globe.X;
                    mc.Y = globe.Y;
                    mc.Mass = globe.Mass;
                }
                else
                {
                    mc.X += (globe.X - mc.X) * globe.Mass / (mc.Mass + globe.Mass);
                    mc.Y += (globe.Y - mc.Y) * globe.Mass / (mc.Mass + globe.Mass);
                    mc.Mass += globe.Mass;
                }
            });

            return mc;
        }

        public MassCenter GenerateMassCenter(List<Globe> globes)
        {
            var mc = new MassCenter();

            mc.Mass = globes.Sum(g => g.Mass);

            mc.X = globes.Sum(g => g.X * g.Mass) / mc.Mass;
            mc.Y = globes.Sum(g => g.Y * g.Mass) / mc.Mass;

            return mc;
        }

        public Globe MoveGlobe(Globe globe)
        {

            globe.X += globe.SpeedX;
            globe.Y += globe.SpeedY;

            return globe;
        }

        public Globe CalculateSpeed(Globe globe, MassCenter center)
        {

            var distanceSquare = DistanceSquare(center.X, center.Y, globe.X, globe.Y);

            var vectorSpeed = G * center.Mass / distanceSquare;

            var distance = Math.Sqrt(distanceSquare);

            globe.SpeedX += (center.X - globe.X) * vectorSpeed / distance;
            globe.SpeedY += (center.Y - globe.Y) * vectorSpeed / distance;

            return globe;
        }

        public double Distance(double x0, double y0, double x1, double y1)
        {
            return Math.Sqrt(DistanceSquare(x0, y0, x1, y1));
        }

        public double DistanceSquare(double x0, double y0, double x1, double y1)
        {
            return Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2);
        }
    }
}
