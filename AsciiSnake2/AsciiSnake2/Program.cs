namespace AsciiSnake2
{
    internal class Program
    {
        struct Vector2
        {
            public Vector2(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X;
            public int Y;
        }
        
        public static void Main(string[] args)
        {
            int ScreenHeight = 40;
            int ScreenWidth = 120;
            
            char[] screen = new char[ScreenWidth * ScreenHeight];
            for(int ii = 0; ii < ScreenWidth * ScreenHeight; ii++)
                screen[ii] = ' ';
            
            while(true)
            {
                int score = 0;
                
                Vector2 foodPos = new Vector2(0, 0);
            }
        }
    }
}