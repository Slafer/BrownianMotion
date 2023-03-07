using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animator_circles
{
    public class Animator
    {
        private Size cSize;
        private BufferedGraphics bg;
        private Graphics _g;
        private List<Circle> circs=new List<Circle>();
        private Thread? t;
        private bool alive;
        private Graphics g
        {
            get => _g;
            set
            {
                _g = value;
                lock (_g)
                {
                    bg = BufferedGraphicsManager.Current.Allocate(g, Rectangle.Ceiling(_g.VisibleClipBounds));
                }
            }
        }

        public Animator(Size containerSize, Graphics g)
        {
            this.alive = true;
            cSize = containerSize;
            this.g = g;
        }
        public void AddCircle(Point location)
        {
            Circle c=new Circle(cSize, location);
            c.Animate(circs);
            lock (circs)
            {
                circs.Add(c);
            }
        }
        public void Start()
        {
            if(t == null || !t.IsAlive)
            {
                t = new Thread(() =>
                {
                    Graphics tg;
                    lock (bg)
                    {


                        do
                        {
                            lock (circs)
                            {
                                tg = bg.Graphics;
                                tg.Clear(Color.White);
                                circs.RemoveAll(it => !it.IsAlive);
                                for (int i = 0; i < circs.Count; i++)
                                {
                                    circs[i].Paint(tg);
                                }
                                lock (g)
                                {
                                    bg?.Render(g);

                                }
                            }
                            Thread.Sleep(15);

                        } while (this.alive);


                    }
                });
                t.IsBackground = true;
                t.Start();
            }
        }
        public void Resize(Size new_size, Graphics g)
        {
                cSize = new Size(new_size.Width, new_size.Height);
                lock (circs)
                {
                    foreach (var c in circs)
                    {
                        c._containerSize = cSize;
                    }
                }
            lock (g)
            {
               lock (this.g)
               {
                  this.g = g;
               }
            }
        }
        public void Close()
        {
            this.alive = false;

//            t.Abort();
        }
    }
}
