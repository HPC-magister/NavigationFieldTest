using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NavigField
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch arrow;
        SpriteFont SpriteFont1;
        Texture2D arrowTexture;
        Rectangle arrowDestination;
        Rectangle arrowSource;
        Color bckgrColor;
                
        FieldSpace GuidanseFieldSpace1;
        NavigationFieldSpace NavigationFieldSpace1;

        public delegate int Del(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated, int countOfSubIterations);

        public int costMinFinder(int xIsCalculated, int yIsCalculated, int param3, int param4, int countOfSubIterations)
        {
            Del costCreatorDel = costCreator;


            Indexer(costCreatorDel, xIsCalculated, yIsCalculated, countOfSubIterations, 0);

            return 0;
        }

        public int costCreator(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated, int param5)
        {
            if (NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed > 0
                && NavigationFieldSpace1.FieldArray[xCurrent, yCurrent].iterationsPassed > 0)
            {
                if (NavigationFieldSpace1.FieldArray[xCurrent, yCurrent].pathCost
                < NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].pathCost)
                {
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].pathCost
                        = NavigationFieldSpace1.FieldArray[xCurrent, yCurrent].pathCost + 1;

                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed++;
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;
                }
            }
            else
                if (NavigationFieldSpace1.FieldArray[xCurrent, yCurrent].iterationsPassed > 0)
                {
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].pathCost
                    = NavigationFieldSpace1.FieldArray[xCurrent, yCurrent].pathCost + 1;

                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed++;
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                    NavigationFieldSpace1.FieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;
                }

                
            


            return 0;
        }

        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

  
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = "Navigation Field";
            this.IsMouseVisible = true;

            arrowSource = new Rectangle(0, 0, 265, 27);
            arrowDestination = new Rectangle(200, 200, 100, 10);

            bckgrColor = Color.Gray;
            double PI = Math.PI;
                        
            GuidanseFieldSpace1 = new FieldSpace(3, 3);

            Random random = new Random(); 

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    GuidanseFieldSpace1.UpdateCell(false, i, j, random.Next(), 5);
            
            NavigationFieldSpace1 = new NavigationFieldSpace(10, 9);/*
            NavigationFieldSpace1.FieldArray[0, 0].pathCost = Double.PositiveInfinity;
            NavigationFieldSpace1.FieldArray[1, 1].pathCost = Double.PositiveInfinity;
            NavigationFieldSpace1.FieldArray[1, 2].pathCost = Double.PositiveInfinity;
            NavigationFieldSpace1.FieldArray[0, 1].pathCost = Double.PositiveInfinity;
            NavigationFieldSpace1.FieldArray[0, 1].pathCost = Double.PositiveInfinity;
            */
            /*
            GuidanseFieldSpace1.UpdateCell(true, 0, 1, PI * 3 / 4, 5);
            GuidanseFieldSpace1.UpdateCell(true, 1, 2, PI * 3 / 4, 5);
            GuidanseFieldSpace1.UpdateCell(true, 2, 1, PI * 3 / 4, 5);
            */


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            arrow = new SpriteBatch(GraphicsDevice);            
            arrowTexture = Content.Load<Texture2D>("arrow");
            SpriteFont1 = Content.Load<SpriteFont>("SpriteFont1");
            

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {           
            GraphicsDevice.Clear(bckgrColor);

            arrow.Begin();                       

                arrow.DrawString(SpriteFont1, 
                "Count of fields:   " + GuidanseFieldSpace1.GetCountOfFields().ToString() + 
                "\nCount of active fields:   " + GuidanseFieldSpace1.GetCountOfActiveCells(), new Vector2(0,0), Color.Black);

                Del handler = costMinFinder;
                NavigationFieldSpace1.FieldArray[5, 5].pathCost = 5;

                int xAimIndex = 5, yAimIndex = 5;
                NavigationFieldSpace1.FieldArray[xAimIndex, yAimIndex].iterationsPassed = 1;

                int maxDistToBorder = Math.Max(Math.Max((NavigationFieldSpace1.xSize - 1) - xAimIndex, xAimIndex),
                                          Math.Max((NavigationFieldSpace1.ySize - 1) - yAimIndex, yAimIndex));

                Indexer(handler, xAimIndex, yAimIndex, maxDistToBorder, 1);

            arrow.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);                     
            
        }

        public int Indexer(Del handler, int xAimIndex, int yAimIndex, int countOfIterations, int countOfSubIterations)
        {
            
            for (int i = 1; i <= countOfIterations; i++)
            {
                int onSideCellsToDraw = 2 * i;

                int xBeginIndex = xAimIndex + i;
                int yBeginIndex = yAimIndex + i;

                int x = xBeginIndex, y = yBeginIndex;

                int[] kxky = new int[] { 0, -1, 0, 1, 0 };


                for (int rectSide = 0; rectSide < 4; rectSide++)
                {
                    int kx = kxky[rectSide + 1], ky = kxky[rectSide];

                    for (int dl = 0; dl < onSideCellsToDraw; dl++)
                    {
                        x += kx; y += ky;

                        if ((0 <= x) && (x < NavigationFieldSpace1.FieldArray.GetLength(0)) &&
                            (0 <= y) && (y < NavigationFieldSpace1.FieldArray.GetLength(1)))
                        {
                            
                            if (!NavigationFieldSpace1.FieldArray[x, y].isObstacle)
                            {                                
                                handler(x, y, xAimIndex, yAimIndex, countOfSubIterations);
                            }
                        }
                    }
                }
            }


            int xDrawCoord = 50;
            int yDrawCoord = 50;

            int dist = 50;

            for (int x = 0; x < NavigationFieldSpace1.xSize; x++)
                for (int y = 0; y < NavigationFieldSpace1.ySize; y++)
                {
                    arrowDestination = new Rectangle(xDrawCoord + x * dist, yDrawCoord + y * dist, 
                        (int)NavigationFieldSpace1.FieldArray[x, y].amplitude * 10,
                        (int)NavigationFieldSpace1.FieldArray[x, y].amplitude);
                    arrow.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor, 
                        (float)NavigationFieldSpace1.FieldArray[x, y].angle, 
                        new Vector2(132, 13), SpriteEffects.None, 0);

                    if (!NavigationFieldSpace1.FieldArray[x, y].isObstacle)
                        arrow.DrawString(SpriteFont1,
                        NavigationFieldSpace1.FieldArray[x, y].pathCost.ToString(),
                        new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);
                    else
                        arrow.DrawString(SpriteFont1, "inf",
                        new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);                    
                }

            return 0;
        }
    }
}
