using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/*
 * Based off-of this code written in C++ but written in C#
 * https://github.com/OneLoneCoder/videos/blob/master/OneLoneCoder_Snake.cpp
 *
 * Basically I wrote all of the C# code but in reality I didn't think of the
 * actual logic behind it (although I did make sure I understood it)
 */

namespace AsciiSnake
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
            Random RNG = new Random();
            
            int ScreenWidth = 120;
            int ScreenHeight = 40;
            
            char[] screen = new char[ScreenWidth * ScreenHeight];
            for(int ii = 0; ii < ScreenWidth * ScreenHeight; ii++)
                screen[ii] = ' ';
            //uint bytesWrittn = 0;

            while(true)
            {
                // I believe if this was a complete copy this should be a LinkedList but hey it's easier like this
                List<Vector2> snake = new List<Vector2>()
                {
                    new Vector2(60, 15),
                    new Vector2(61, 15),
                    new Vector2(62, 15),
                    new Vector2(63, 15),
                    new Vector2(64, 15),
                    new Vector2(65, 15),
                    new Vector2(66, 15),
                    new Vector2(67, 15),
                    new Vector2(68, 15),
                    new Vector2(69, 15)
                };

                Vector2 foodPos = new Vector2(30, 15);

                int score = 0;
                int snakeDir = 3;

                bool dead = false;

                bool keyLeft = false, keyRight = false, keyLeftOld = false, keyRightOld = false;
                
                while(!dead)
                {
                    #region Timing

                    //Thread.Sleep(20);

                    #endregion
                    
                    #region Input

                    string userInput = "";

                    switch(userInput)
                    {
                        case "A": // LEFT
                            keyLeft = true;
                            break;
                        case "D": // RIGHT
                            keyRight = true;
                            break;
                    }

                    if(keyLeft && !keyLeftOld)
                    {
                        snakeDir--;
                        if(snakeDir == -1) snakeDir = 3;
                    }
                    
                    if(keyRight && !keyRightOld)
                    {
                        snakeDir++;
                        if(snakeDir == 4) snakeDir = 0;
                    }

                    keyLeftOld = keyLeft;
                    keyRightOld = keyRight;
                    
                    #endregion
                    
                    #region Update
                    
                    // Snake Position
                    switch(snakeDir)
                    {
                        case 0: // UP
                            snake.Insert(0, new Vector2(snake.First().X, snake.First().Y - 1));
                            break;
                        case 1: // RIGHT
                            snake.Insert(0, new Vector2(snake.First().X + 1, snake.First().Y));
                            break;
                        case 2: // DOWN
                            snake.Insert(0, new Vector2(snake.First().X, snake.First().Y + 1));
                            break;
                        case 3: // LEFT
                            snake.Insert(0, new Vector2(snake.First().X - 1, snake.First().Y));
                            break;
                    }

                    // Collision Detection for Snake & Food
                    if(snake.First().X == foodPos.X && snake.First().Y == foodPos.Y)
                    {
                        score++;

                        while(screen[foodPos.Y * ScreenWidth + foodPos.X] != ' ')
                        {
                            foodPos.X = RNG.Next(ScreenWidth);
                            foodPos.Y = RNG.Next(ScreenHeight);
                        }
                        
                        for(int ii = 0; ii < 5; ii++)
                            snake.Add(snake.Last());
                    }
                    
                    // Collision Detection for Snake & World
                    if(snake.First().X < 0 || snake.First().X >= ScreenWidth)
                        dead = true;
                    if(snake.First().Y < 3 || snake.First().Y >= ScreenHeight)
                        dead = true;
                    
                    // Collision Detection for Snake & Snake
                    foreach(var segment in snake)
                    {
                        // TODO: this
                    }
                    
                    snake.RemoveAt(snake.Count - 1);

                    #endregion

                    #region Display

                    // Clear Screen
                    //for(int ii = 0; ii < ScreenWidth * ScreenHeight; ii++) screen[ii] = ' ';
                    Console.Clear();
                    
                    // Draw Stats & Border
                    for(int ii = 0; ii < ScreenWidth; ii++)
                    {
                        screen[ii] = '=';
                        screen[2 * ScreenWidth + ii] = '=';
                    }

                    #region Draw Snake
                    
                    // Draw Snake Body
                    foreach(var segment in snake)
                        screen[segment.Y * ScreenWidth + segment.X] = dead ? '+' : 'O';

                    // Draw Snake Head
                    screen[snake.First().Y * ScreenWidth + snake.First().X] = dead ? 'X' : '@';

                    #endregion
                    
                    // Draw Food
                    screen[foodPos.Y * ScreenWidth + foodPos.X] = '%';
                    
                    // If Dead...
                    //if(dead)
                       // Console.Write(;
                       
                    Console.Write(screen);
                    Console.ReadLine();
                    
                    #endregion
                }
            }
        }
    }
}