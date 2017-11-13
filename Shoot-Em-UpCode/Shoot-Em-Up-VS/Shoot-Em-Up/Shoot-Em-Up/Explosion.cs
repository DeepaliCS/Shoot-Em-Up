using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shoot_Em_Up
{
    public class Explosion
    {

        public Texture2D texture;
        public Vector2 position;
        public float timer;
        public float interval;
        public Vector2 origin;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;

        // Constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            //intervals about how fast or slow the animation happens
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisible = true;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {

        }

        // Update
        public void Update(GameTime gameTime)
        {
            //Increase timer by the number of milliseconds since update was last called
            //Timer += time passed by
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check the time is more than the chosen interval
            if (timer > interval)
            {
                //Show next frame
                currentFrame++;

                // Reset Timer
                timer = 0f;

            }

            //if were on the last frame, make the explosion invisible and reset currentFrame
            //to beginning to spritesheet

            //8 because there are 8 images 
            if (currentFrame == 8)
            {
                isVisible = false;
                currentFrame = 0;
            }


            //tut 23
            //...

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        // Draw 
        public void Draw(SpriteBatch spriteBatch)
        {
            // if visible ==true then draw
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);

            }
        }
    }
}
