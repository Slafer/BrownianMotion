using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animator_circles
{
    public class Circle
    {
        private Point _pos;
        private int _speedloose = 3;
        private int _radius;
        private int _diameter;
        private int _dx;
        private int _dy;
        public Size _containerSize;
        private Thread? t;
        public bool IsAlive = true;
        public Color Color { get; set; }
        public Circle(Size contSize, Point p)
        {
            Random r = new Random();
            _radius = r.Next(30,70);
            _pos=new Point(p.X-_radius,p.Y-_radius);
            _diameter = _radius * 2;
            Color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            _containerSize = contSize;
            _dx = r.Next(-15, 15);
            _dy = r.Next(-15,15);
        }
        public void Paint(Graphics g)
        {
            var b = new SolidBrush(Color);
            g.FillEllipse(b,_pos.X, _pos.Y, _diameter, _diameter);

        }

        /*public void Clear(Graphics g)
        {
            var b = new SolidBrush(Color.White);
            g.FillEllipse(b, _pos.X, _pos.Y, _diameter, _diameter);
        }*/
        private bool IsInX()
        {
            if((_pos.X<=_containerSize.Width-_diameter-Math.Abs(_dx))&&(_pos.X>=Math.Abs(_dx))) return true;
            return false;
        }
        private bool IsInY()
        {
            if ((_pos.Y >= Math.Abs(_dy)) && (_pos.Y <= _containerSize.Height - _diameter - Math.Abs(_dy))) return true;
            return false;
        }
        private void UnStuckX()
        {
          //  _dy = 0;
            while (_pos.X + _diameter >= _containerSize.Width) _pos.X -= 10;
            while (_pos.X + _diameter <= 0) _pos.X += 10;
        }
        private void UnStuckY()
        {
           // _dx = 0;
            while (_pos.Y + _diameter >= _containerSize.Height) _pos.Y -= 10;
            while (_pos.Y + _diameter <= 0) _pos.Y += 10;
        }
            private void ChangeXSpeed()
        {
            _dx = -_dx;
            if(_dx>0)
            {
                if(_dx>_speedloose)
                {
                    _dx -= _speedloose;
                }
                else
                {
                    if(_dx>1)
                    {
                        _dx -= 1;
                    }else
                    {
                        _dx = 1;
                    }
                }
            }else
            {
                if(_dx<0)
                {
                    if(-_dx>_speedloose)
                    {
                        _dx += _speedloose;
                    }
                    else
                    {
                        if(_dx<-1)
                        {
                            _dx += 1;
                        }
                        else
                        {
                            _dx = -1;
                        }
                    }
                }
            }
        }
        private void ChangeYSPeed()
        {
            _dy =-_dy;
            if (_dy > 0)
            {
                if (_dy > _speedloose)
                {
                    _dy -= _speedloose;
                }
                else
                {
                    if(_dy>1)
                    {
                        _dy -= 1;
                    }else
                    {
                        _dy = 1;
                    }
                }
            }
            if (_dy < 0)
            {
                if (-_dy > _speedloose)
                {
                    _dy += _speedloose;
                }
                else
                {
                    if(_dy<-1)
                    {
                        _dy += 1;
                    }
                    else
                    {
                        _dy = -1;
                    }
                }
            }
            
        }

        
        private bool IsCollide(Circle a)
        {
            if (this == a) return false;
            var p1 = new Point(this._pos.X, this._pos.Y);
            var p2 = new Point(a._pos.X, a._pos.Y);
            var distance = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            if (distance <= this._radius + a._radius) return true;
            return false;
        }
        private void Collision (Circle a)
        {
            Point _speedVect = new Point((this._pos.X - a._pos.X), (this._pos.Y - a._pos.Y));
            var mod = Math.Sqrt(_speedVect.X * _speedVect.X + _speedVect.Y * _speedVect.Y);
            var speed = Math.Sqrt(this._dx * this._dx + this._dy * this._dy);
            var new_speed = speed / mod;
            _speedVect = new Point(Convert.ToInt32((Math.Ceiling(_speedVect.X * new_speed))), Convert.ToInt32((Math.Ceiling(_speedVect.Y * new_speed))));
            this._dx = _speedVect.X;
            this._dy = _speedVect.Y;
            _pos.X += _dx;
            _pos.Y += _dy;
        }
        public void Move(List<Circle> circs)
        {
                if (IsInX())
                {
                    _pos.X += _dx;
                }
                else
                {
                    //               UnStuckX();
                    ChangeXSpeed();
                    //_dx=-_dx;
                    _pos.X += _dx;
                UnStuckX();
                }
                if (IsInY())
                {
                    _pos.Y += _dy;
                }
                else
                {
                    //               UnStuckY();
                    ChangeYSPeed();
                //_dy=-_dy;
                    _pos.Y += _dy;
                UnStuckY();

                }
        
            lock (circs)
            {
                foreach (var c in circs)
                {
                    if (this.IsCollide(c))
                        Collision(c);
                }
            }
            //if (IsStuckX())
            //{
            //    UnStuckX();
            //}
            //if (IsStuckY())
            //{
            //    UnStuckY();
            //}
            if ((Math.Abs(_dx)+Math.Abs(_dy))<=2) IsAlive = false;
        }

        public void Animate(List<Circle> circs)
        {
            if(t?.IsAlive ?? true)
            {
                t = new Thread(() =>
                  {
                      while (IsAlive)
                      {
                          Thread.Sleep(15);
                          Move(circs);
                      }
                  });
                t.IsBackground = true;
                t.Start();
            }
        }

        
    }
}
