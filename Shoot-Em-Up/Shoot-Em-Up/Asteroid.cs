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
            speed = 8;
            randX = random.Next(0, 550);
            randY = random.Next(-500, -50);
            isVisible = true;

        }
        
        //Update
        public void Update(GameTime gameTime)
        {
            // Set bounding box for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Update Movemement
            position.Y = position.Y + speed;
            if (position.Y >= 950)
                position.Y = -50;
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //Repeat if asteroid isnt destroyed
            if (isVisible)
                spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
