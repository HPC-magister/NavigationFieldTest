using System;
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
                
        GuidanseFieldSpace GuidanseFieldSpace1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = "Navigation Field";
            this.IsMouseVisible = true;

            arrowSource = new Rectangle(0, 0, 265, 27);
            arrowDestination = new Rectangle(200, 200, 100, 10);

            bckgrColor = Color.Gray;
            double PI = Math.PI;
            
            
            GuidanseFieldSpace1 = new GuidanseFieldSpace(10, 5);
            GuidanseFieldSpace1.SetField(true, 0, 0, PI / 4, 0, 0,0);
            GuidanseFieldSpace1.SetField(true, 0, 1, -PI / 4, 0, 0,0);
            GuidanseFieldSpace1.SetField(true, 1, 0, PI / 2, 0, 0, 0);
            GuidanseFieldSpace1.SetField(true, 1, 1, PI * 3 / 4, 0, 0, 0);
            GuidanseFieldSpace1.SetField(true, 1, 2, PI, 0, 0, 0);
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            arrow = new SpriteBatch(GraphicsDevice);            
            arrowTexture = Content.Load<Texture2D>("arrow");
            SpriteFont1 = Content.Load<SpriteFont>("SpriteFont1");
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(bckgrColor);
            /*
            row.Begin();
                //spriteBatch.Draw(arrowTexture, Vector2.Zero, Color.Gray);
                angle += 0.01;
                row.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor, (float)(angle), new Vector2(132, 13), SpriteEffects.None, 0);
                arrowDestination.X += 100;
                row.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor, -(float)(angle), new Vector2(132, 13), SpriteEffects.None, 0);
                arrowDestination.X -= 100;
            row.End();

            System.Threading.Thread.Sleep(10);
            */


            int x;
            int y;

            int dist = 100;

            arrow.Begin();

                //Field[,] ArrayOfFields = GuidanseFieldSpace1.GetArayOfFields();

                foreach(Field e in GuidanseFieldSpace1.GetArayOfFields()) {
                    if (e.IsActive()) {
                        arrowDestination = new Rectangle(200 + e.GetXPos() * dist, 200 + e.GetYPos() * dist, 100, 10);

                        arrow.Draw(arrowTexture, arrowDestination, arrowSource, bckgrColor, (float)e.GetAngle(), new Vector2(132, 13), SpriteEffects.None, 0);
                    }
                    
                }
                arrow.DrawString(SpriteFont1, 
                    "Count of fields:   " + GuidanseFieldSpace1.GetCountOfFields().ToString() + 
                    "\nCount of active fields:   " + GuidanseFieldSpace1.GetCountOfActiveFields(), new Vector2(0,0), Color.Black);

            arrow.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);                     
            
        }
    }
}
