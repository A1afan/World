using System;
using System.Collections.Generic;
using System.Drawing;

namespace World
{
    public class World_
    {
        public static Graphics g;
        public static Color bkcolor;
        private static int delta = 20;
        private static int maxY;
        private static int maxX;
        static public Image[] imageWorld = new Image[3];

        //embedded classes       
        class Anima : IDisposable
        {
            public Point p;
            public int type;
            public Color color;
            public int reproduction;
            public byte lifeage;

            public Anima() { }
            public void Dispose() { }
            virtual public void Draw(Color c) { }

            public void move(int dir)
            {
                Draw(bkcolor);
                switch (dir)
                {
                    case 0://up
                        if (p.Y - delta > 0)
                            p.Y -= delta;
                        else
                            p.Y = maxY;
                        break;
                    case 1://right
                        if (p.X + delta < maxX)
                            p.X += delta;
                        else
                            p.X = 0;
                        break;
                    case 2://down
                        if (p.Y + delta < maxY)
                            p.Y += delta;
                        else
                            p.Y = 0;
                        break;
                    case 3://left
                        if (p.X - delta > 0)
                            p.X -= delta;
                        else
                            p.X = maxX;
                        break;
                }
                Draw(color);
            }
        }

        class Grass : Anima
        {
            override public void Draw(Color c)
            {
                if (c != bkcolor)
                {
                    g.DrawImage(imageWorld[0], p.X, p.Y, 20, 20);
                }
                else
                {
                    SolidBrush b = new SolidBrush(c);
                    g.FillRectangle(b, p.X, p.Y, delta, delta);
                }
            }
            public Grass()
            {
                color = Color.Green;
                type = 1;
                reproduction = 40;
                lifeage = 45;
            }
        }

        class Grass_eating : Anima
        {
            override public void Draw(Color c)
            {

                if (c != bkcolor)
                {
                    g.DrawImage(imageWorld[1], p.X, p.Y, 20, 20);
                }
                else
                {
                    SolidBrush b = new SolidBrush(c);
                    g.FillRectangle(b, p.X, p.Y, delta, delta);
                }
            }
            public Grass_eating()
            {
                color = Color.Yellow;
                type = 2;
                reproduction = 0;
                lifeage = 130;
            }
        }

        class Anima_eating : Anima
        {
            override public void Draw(Color c)
            {
                if (c != bkcolor)
                {
                    g.DrawImage(imageWorld[2], p.X, p.Y, 20, 20);
                }
                else
                {
                    SolidBrush b = new SolidBrush(c);
                    g.FillRectangle(b, p.X, p.Y, delta, delta);
                }
            }
            public Anima_eating()
            {
                color = Color.Red;
                type = 0;
                reproduction = 0;
                lifeage = 100;
            }
        }

        List<Anima> animas = new List<Anima>();
        Random r = new Random();
        public World_(int width, int heigh)
        {
            maxX = width;
            maxY = heigh;
            createWorld();
        }

        Anima createAnima(int type)
        {
            Anima a = null;
            switch (type)
            {
                case 1: a = new Grass(); break;
                case 0: a = new Anima_eating(); break;
                case 2: a = new Grass_eating(); break;
            }
            a.p.X = r.Next(maxX / delta) * delta;
            a.p.Y = r.Next(maxY / delta) * delta;
            return a;
        }

        private void createWorld()
        {
            for (int i = 0; i < 20; i++)
                animas.Add(createAnima(r.Next(50) % 3));
        }

        private int intersection(Anima a)
        {
            for (int i = 0; i < animas.Count; i++)
            {
                if (a != animas[i] && a.p == animas[i].p)
                    return i;
            }
            return -1;
        }

        private void checkIntersection()
        {
            for (int j = 0; j < animas.Count; j++)
            {
                int i = intersection(animas[j]);
                if (i >= 0)
                {
                    if (animas[j].type == animas[i].type && animas[j].reproduction == 1 && animas[i].reproduction == 1 && animas[j].type != 1)
                    {
                        animas.Add(createAnima(animas[i].type));
                        animas[j].reproduction = 0;
                        animas[i].reproduction = 0;
                    }
                    if (animas[j].type == 1 && animas[i].type == 2)
                    {
                        Anima a = animas[j];
                        a.Draw(bkcolor);
                        animas.Remove(a);
                        a.Dispose();
                    }
                    else
                    if (animas[j].type == 0 && animas[i].type == 2)
                    {
                        Anima a = animas[i];
                        a.Draw(bkcolor);
                        animas.Remove(a);
                        a.Dispose();
                    }
                    else
                    if (animas[j].type == 1 && animas[i].type == 0)
                    {
                        Anima a = animas[j];
                        a.Draw(bkcolor);
                        animas.Remove(a);
                        a.Dispose();
                    }
                }
            }
        }

        public void Live()
        {
            for (int i = 0; i < animas.Count; i++)
            {
                if (animas[i].type != 1) animas[i].move(r.Next(30) % 4);
                else
                    animas[i].Draw(animas[i].color);
                if (animas[i].reproduction == 0) animas[i].reproduction = 1;
                if (animas[i].reproduction > 10)
                {
                    animas[i].reproduction--;
                }
                else if (animas[i].reproduction == 10)
                {
                    animas.Add(createAnima(animas[i].type));
                    animas[i].reproduction = 15;
                }

                if (animas[i].lifeage > 0)
                {
                    animas[i].lifeage--;
                }
                else
                {
                    Anima a = animas[i];
                    a.Draw(bkcolor);
                    animas.Remove(a);
                    a.Dispose();
                }
            }
            checkIntersection();
        }
    }
}
