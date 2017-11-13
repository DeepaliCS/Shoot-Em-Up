using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Shoot_Em_Up
{
    //Main
    public class Player
    {

        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition;
        public int speed, health;
        public float bulletDelay;
        public bool isColliding;
        public Rectangle boundingBox, healthRectangle;
        public List<Bullet> bulletList;
        SoundManager sm = new SoundManager();


        // Constructor
        public Player()
        {
            bulletList = new List<Bullet>();
            texture = null;
            //Check this to restart players position after gameover
            position = new Vector2(300, 300);
            bulletDelay = 20;
            speed = 10;
            isColliding = false;
            health = 200;
            healthBarPosition = new Vector2(50, 50);


        }

        //Load Content
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("rocket");
            bulletTexture = content.Load<Texture2D>("bullet");
            healthTexture = content.Load<Texture2D>("health");
            //sm.LoadContent(content);
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        //Update
        public void Update(GameTime gametime)
        {

            //Getting Keyboard state
            KeyboardState keyState = Keyboard.GetState();

            //Bounding box for playership
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Set Rectangle for healthbar-check the numbers or change image
            healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 25);
            //Fire Bullets
            if (keyState.IsKeyDown(Keys.Space))
            {
                Shoot();
            }

            UpdateBullets();

            //Ship Controls
            if (keyState.IsKeyDown(Keys.Up))
                position.Y = position.Y - speed;
            if (keyState.IsKeyDown(Keys.Left))
                position.X = position.X - speed;
            if (keyState.IsKeyDown(Keys.Down))
                position.Y = position.Y + speed;
            if (keyState.IsKeyDown(Keys.Right))
                position.X = position.X + speed;

            //Keep Player Ship in Screen Bounds
            //Check This.
            if (position.X <= 0)
                position.X = 0;


            // Look at this - dimension to make sure rocket doesn't go off screen
            if (position.X >= 800 - texture.Width)
                position.X = 800 - texture.Width;

            if (position.Y <= 0)
                position.Y = 0;

            if (position.Y >= 950 - texture.Height)
                position.Y = 950 - texture.Height;
        }

        // Shoot Method (Used to set starting position of bullet)

        public void Shoot()
        {
            //Shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            //If bullet delay is at 0, create new bullet at player position, make it visible on screen
            //, then add that bullet to the list
            if (bulletDelay <= 0)
            {
                //sm.playerShootSound.Play();

                //passing bullet texture into bullet class
                Bullet newBullet = new Bullet(bulletTexture);
                //Centre of spaceship //tut 8
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2,
                    position.Y + 30);

                //set it to visible
                newBullet.isVisible = true;

                //only 20 bullets on screen at the time
                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            //reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 20;
        }

        //Update bullet function after its shooting
        public void UpdateBullets()
        {
            // for each bullet in our bulletList, update the movement and if the bullet hits the top of the screen,
            //remove it from the list
            foreach (Bullet b in bulletList)
            {
                // Bounding box for our bullets, for every bullet in our bullet list
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                //bullet going up the screen, movement
                b.position.Y = b.position.Y - b.speed;

                //take it off sceen if it hits top of screen
                if (b.position.Y <= 0)
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
    }
}
