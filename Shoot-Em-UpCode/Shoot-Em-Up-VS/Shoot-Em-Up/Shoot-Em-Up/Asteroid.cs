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
    public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;

        public bool isVisible;
        Random random = new Random();
        public float randX, randY;


        //Constructor 
        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;
            randX = random.Next(0, 550);
            randY = random.Next(-500, -50);
            isVisible = true;


        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {

        }

        //Update
        public void Update(GameTime gameTime)
        {
            // Set bounding box for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Move it to load content if you want it to move sideways
            //Finding the centre of the asteroid
            //origin.X = texture.Width / 2;
            //origin.Y = texture.Height / 2;

            // Update Movemement
            position.Y = position.Y + speed;
            if (position.Y >= 800)
                position.Y = -50;

            ////Rotating Asteroid


            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //rotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;


        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //Repeat if asteroid isnt destroyed
            if (isVisible)
                spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(texture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
