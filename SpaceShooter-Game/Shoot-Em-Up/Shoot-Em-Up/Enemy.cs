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
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        //Constuctor
        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newbulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newbulletTexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            //to change or edit this look at tut 16??
            bulletDelay = 40;
            speed = 5;
            isVisible = true;


        }

        //Update
        public void Update(GameTime gametime)
        {
            //Update Collision rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update enemy movement
            position.Y += speed;

            //Move enemy back to top of screen is he flys off bottom
            if (position.Y >= 900)
                position.Y = -20;

            EnemyShoot();
            UpdateBullets();

        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy ship
            spriteBatch.Draw(texture, position, Color.White);

            //Draw enemy bullets
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        //Update
        public void UpdateBullets()
        {
            // for each bullet in our bulletList, update the movement and if the bullet hits the top of the screen,
            //remove it from the list
            foreach (Bullet b in bulletList)
            {
                // Bounding box for our bullets, for every bullet in our bullet list
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                //bullet going up the screen, movement
                b.position.Y = b.position.Y + b.speed;

                //take it off sceen if it hits bottom of screen
                if (b.position.Y >= 800)
                    b.isVisible = false;

            }

            //Going through bullet list, if its not visible then remove from list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Enemy Shoot Function
        public void EnemyShoot()
        {
            //Shoot only if bulletdelay resets,
            //start counting down is bulletdelay is bigger than 0
            if (bulletDelay >= 0)
                bulletDelay--;

            if (bulletDelay <= 0)
            {
                //Create new bullet and position it front and centre of enemy ship
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;
                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);

            }

            //reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 40;
        }

    }


}
