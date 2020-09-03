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
    public class Starfield
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        //Constructor
        public Starfield()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -800);
            speed = 5;

        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Background");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Setting speed from background scrolling
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            //Scroll background (repeating)
            if (bgPos1.Y >= 800)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -800;
            }
        }

    }
}
