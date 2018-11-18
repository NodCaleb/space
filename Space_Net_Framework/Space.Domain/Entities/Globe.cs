using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Space.Domain.Entities
{
    public class Globe : MassCenter
    {
        public int Radius
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public double SpeedX
        {
            get;
            set;
        }
        public double SpeedY
        {
            get;
            set;
        }
        public Color Color { get; set; }
        public Guid Id { get; set; }
    }
}
