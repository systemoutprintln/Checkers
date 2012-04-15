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

namespace Checkers
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texSprites;

        Board b;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {


            base.Initialize();

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();




            this.IsMouseVisible = true;
        }


        protected override void LoadContent()
        {
 
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texSprites = Content.Load<Texture2D>(@"SpriteS");

            b = new Board(texSprites);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here

            if (gameTime.TotalGameTime.Ticks == 0)
            {

                b.Setup();

            }

            MouseState m = Mouse.GetState();

            if (m.LeftButton == ButtonState.Pressed)
            {
                b.onClick(m);
            }


            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            b.drawBoard(spriteBatch);


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
