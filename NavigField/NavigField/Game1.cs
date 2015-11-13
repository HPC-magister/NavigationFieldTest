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
                        
            GuidanseFieldSpace1 = new FieldSpace(3, 3);

            Random random = new Random(); 

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    GuidanseFieldSpace1.UpdateCell(false, i, j, random.Next(), 5);
            
            NavigationFieldSpace1 = new NavigationFieldSpace(10, 9);

            System.Threading.Thread t = new System.Threading.Thread(thr);
            t.Start();

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

        public void thr()
        {
            NavigationFieldSpace1.CalculateFieldForAim(7, 7);
        }

        protected override void Draw(GameTime gameTime)
        {           
            GraphicsDevice.Clear(bckgrColor);

            arrow.Begin();                       

                arrow.DrawString(SpriteFont1, 
                "Count of fields:   " + GuidanseFieldSpace1.GetCountOfFields().ToString() + 
                "\nCount of active fields:   " + GuidanseFieldSpace1.GetCountOfActiveCells(), new Vector2(0,0), Color.Black);

                

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
                            NavigationFieldSpace1.FieldArray[x, y].calcIterationsPassed.ToString(),
                            new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);
                        else
                            arrow.DrawString(SpriteFont1, "inf",
                            new Vector2(xDrawCoord + x * dist + 15, yDrawCoord + y * dist + 15), Color.Black);
                    }

            arrow.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);                     
            
        }

    }
}
