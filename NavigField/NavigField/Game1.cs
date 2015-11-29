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
        SpriteBatch aim;
        SpriteBatch obstacle;
        SpriteFont SpriteFont1;
        Texture2D arrowTexture;
        Rectangle arrowSource;
        Rectangle arrowDestination;

        Rectangle aimSource;
        Texture2D aimTexture;
        Rectangle aimDestination;

        Rectangle obstacleSource;
        Texture2D obstacleTexture;
        Rectangle obstacleDestination;
        
        Color bckgrColor;
                
        FieldSpace GuidanseFieldSpace1;
        NavigationFieldSpace NavigationFieldSpace1;
     

        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 700;
        }

  
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = "Navigation Field";
            this.IsMouseVisible = true;

            arrowSource = new Rectangle(0, 0, 71, 27);
            aimSource = new Rectangle(0, 0, 100, 100);
            obstacleSource = new Rectangle(0, 0, 100, 100);

            bckgrColor = Color.Gray;
                        
            GuidanseFieldSpace1 = new FieldSpace();

            Random random = new Random(); 

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    GuidanseFieldSpace1.UpdateCell(false, i, j, 0 * Math.PI/4, 0);
                        
            NavigationFieldSpace1 = new NavigationFieldSpace(50, 30);
            NavigationFieldSpace1.sleepTime = 0;

            System.Threading.Thread t = new System.Threading.Thread(thr);
            
            t.Start();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            arrow = new SpriteBatch(GraphicsDevice);
            aim = new SpriteBatch(GraphicsDevice);
            obstacle = new SpriteBatch(GraphicsDevice);

            arrowTexture = Content.Load<Texture2D>("Arrow");
            aimTexture = Content.Load<Texture2D>("Aim");
            obstacleTexture = Content.Load<Texture2D>("Obstacle");
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

        public void thr()
        {
            int bx = 25;
            int by = 11;

            NavigationFieldSpace1.FieldArray[7, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[8, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[6, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[9, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[10, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[11, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[12, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[13, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[14, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[15, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[16, 3].amplitude = 0.99;
            NavigationFieldSpace1.FieldArray[7, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[8, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[6, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[9, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[10, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[11, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[12, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[13, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[14, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[15, 3].angle = -Math.PI / 2;
            NavigationFieldSpace1.FieldArray[16, 3].angle = -Math.PI / 2;
            
            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 11].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 8, by + 11].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 9, by + 11].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 10, by + 11].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 11].isObstacle = true;

            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 12].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 13].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 14].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 15].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 7, by + 16].isObstacle = true;

            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 12].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 13].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 14].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 15].isObstacle = true;
            NavigationFieldSpace1.NavigFieldArray[bx + 11, by + 16].isObstacle = true;
            
            
            

            double sin = 0.5;
            double a0 = Math.PI / 2;
            /*
            for (int i = 0; i < 15; i += 3)
            {


                


                if (i != 9)
                    a0 -= Math.PI / 4;
                else
                {
                    a0 = 0;

                }
                if (i == 12)
                    a0 = Math.PI / 4;

                sin = Math.Sin(a0);
                a0 = Math.Asin(sin);

                */
                double[] a = { Math.PI / 4, Math.PI / 4, Math.PI / 4, Math.PI / 4, Math.PI / 4, Math.PI / 4,
                                0, 0, 0, 0, 0 -Math.PI / 4, -Math.PI / 4, -Math.PI / 4, -Math.PI / 4, 0, 0, 0};
                int[] dy = { 1, 1, 2, 2, 3, 3, 4, 4, 4, 4, 4, 3, 3, 2, 2, 1, 1, 1 };

            for (int x = 0; x < 17; x++)
                for (int y = 0 + dy[x]; y < 11 + dy[x]; y++)
                {
                    NavigationFieldSpace1.FieldArray[x + 3, y + 7].angle = a[x];
                    NavigationFieldSpace1.FieldArray[x + 3, y + 7].amplitude 
                        = 0.99 * Math.Abs(Math.Abs(y - dy[x] - 5) - 6) / 6;
                }
            
            for (int x = 17; x < 22; x++)
                for (int y = 0 + dy[x - 17]; y < 11 + dy[x - 17]; y++)
                {
                    NavigationFieldSpace1.FieldArray[x + 3, y + 7].angle = a[x - 17];
                    NavigationFieldSpace1.FieldArray[x + 3, y + 7].amplitude 
                        = 0.99 * Math.Abs(Math.Abs(y - dy[x - 17] - 5) - 6) / 6;
                }
            //  }
            /*

            Creating complex-aim cost propagating algoritm

            int xAimIndex = 1;
            int yAimIndex = 10;

            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].isAim = true;
            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].pathCost = 0;
            
            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].calcIterationsPassed = 1;
            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].wasCalculated = true;

            xAimIndex = 1;
            yAimIndex = 11;

            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].isAim = true;
            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].pathCost = 0;

            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].calcIterationsPassed = 1;
            NavigationFieldSpace1.NavigFieldArray[xAimIndex, yAimIndex].wasCalculated = true;
            */
            NavigationFieldSpace1.CalculateFieldForAim(1, 14, 1, 50);

            
        }

        protected override void Draw(GameTime gameTime)
        {           
            GraphicsDevice.Clear(bckgrColor);

            arrow.Begin();
            aim.Begin();
            
            obstacle.Begin();            

                arrow.DrawString(SpriteFont1, 
                "Calc iterations passed:   " + NavigationFieldSpace1.calcIterationsPassed.ToString(), new Vector2(0,0), Color.Black);

                

                int xDrawCoord = 25;
                int yDrawCoord = 25;
                
                int dist = 20;

            
                      
            
                for (int i = 0; i < NavigationFieldSpace1.xSize; i++)
                    for (int j = 0; j < NavigationFieldSpace1.ySize; j++)
                    {
                        GridCell GridCell1 = NavigationFieldSpace1.FieldArray[i, j];
                        NavigationGridCell NavigationGridCell1 = NavigationFieldSpace1.NavigFieldArray[i, j];

                        if (NavigationGridCell1.isAim)
                        {
                            aimDestination = new Rectangle(xDrawCoord + i * dist, yDrawCoord + j * dist, 10, 10);
                            aim.Draw(aimTexture, aimDestination, aimSource, bckgrColor, 0, new Vector2(50, 50), SpriteEffects.None, 0);

                            continue;
                        }

                        if (NavigationGridCell1.isObstacle)
                        {
                            obstacleDestination = new Rectangle(xDrawCoord + i * dist, yDrawCoord + j * dist, 10, 10);
                            obstacle.Draw(obstacleTexture, obstacleDestination, obstacleSource, bckgrColor, 0, new Vector2(50, 50), SpriteEffects.None, 0);

                            continue;
                        }
                        //var v = (int)Math.Round(20.0 * GridCell1.amplitude / NavigationFieldSpace1.preferredVelocity);
                        arrowDestination = new Rectangle(xDrawCoord + i * dist, yDrawCoord + j * dist, 15 + 0 * (int)Math.Round(15 * NavigationGridCell1.amplitude / NavigationFieldSpace1.preferredVelocity), 10);
                        arrow.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor, (float)NavigationGridCell1.angle, new Vector2(35, 13), SpriteEffects.None, 0);

                    
                    }
                
        
                    /*
                        for (int x = 0; x < NavigationFieldSpace1.xSize; x++)
                            for (int y = 0; y < NavigationFieldSpace1.ySize; y++)
                            {
                                arrowDestination = new Rectangle(xDrawCoord + x * dist, yDrawCoord + y * dist,
                                    (int)NavigationFieldSpace1.NavigFieldArray[x, y].amplitude * 10,
                                    (int)NavigationFieldSpace1.NavigFieldArray[x, y].amplitude);
                                arrow.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor,
                                    (float)NavigationFieldSpace1.NavigFieldArray[x, y].angle,
                                    new Vector2(132, 13), SpriteEffects.None, 0);

                                if (!NavigationFieldSpace1.NavigFieldArray[x, y].isObstacle)
                                    arrow.DrawString(SpriteFont1,
                                    NavigationFieldSpace1.NavigFieldArray[x,y].pathCost.ToString(),
                                    //NavigationFieldSpace1.NavigFieldArray[x, y].xPredecessor.ToString() + " " +
                                    //NavigationFieldSpace1.NavigFieldArray[x, y].yPredecessor.ToString(),
                                    new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);
                                else
                                    arrow.DrawString(SpriteFont1, "inf",
                                    new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);
                            }
                        */
                    arrow.End();
                    aim.End();
                    obstacle.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);                     
            
        }

    }
}
