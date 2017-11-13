using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

//Website for sound and images - opengameart

//Fix ship going out of screen FIXED!!
//Fix enemy ship and bullets FIXED!!
//Add sprite font with HUD 
//Explosions not working tut 24
//Find files for sound and get it working, player and soundmanager class
//If you want sound to play in the menu, then go to tut 27-9:02
//Create menu and game over image
//Check game over and restarting game thing FIXED!!

namespace Shoot_Em_Up
{
    // Main
    public class Game1 : Game
    {

        // State Enum
        public enum State
        {
            Menu,
            Playing,
            Gameover
        }

        // Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;
        public Texture2D menuImage;
        public Texture2D gameoverImage;


        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();


        // Objects
        Player p = new Player();
        Starfield sf = new Starfield();
        HUD hud = new HUD();
        SoundManager sm = new SoundManager();

        public object MediaPLayer { get; private set; }

        // Set first state-Menu
        State gameState = State.Menu;


        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = "Space Shooter Game";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 10;
            menuImage = null;
            gameoverImage = null;
            IsMouseVisible = true;
        }

        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //hud.LoadContent(Content);
            p.LoadContent(Content);
            sf.LoadContent(Content);
            //sm.LoadContent(Content);
            //menuImage = Content.Load<Texture2D>("");
            //gameoverImage = Content.< Texture2D > ("");
        }

        // Update
        protected override void Update(GameTime gameTime)
        {

            //UPDATING PLAYING STATE
            switch (gameState)
            {
                case State.Playing:
                    {

                        // Setting starfield speed back to 5- faster than menu
                        sf.speed = 5;

                        // Updating enemies and checking collision of enemyship to playership
                        foreach (Enemy e in enemyList)
                        {
                            // Check if enemyship is colliding with player
                            if (e.boundingBox.Intersects(p.boundingBox))
                            {
                                // Reducing health to 40 (more than bullets) and taking off screen
                                p.health -= 40;
                                e.isVisible = false;
                            }

                            // Check if Enemy bullet collides with playership
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    // Make playership invisible if health reaches 0 or 10(check enemybulletDamage variable)
                                    p.health -= enemyBulletDamage;
                                    e.bulletList[i].isVisible = false;
                                }
                            }

                            // Check player bullet collision to enemy ship
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (p.bulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    //sm.explodeSound.Play();
                                    //explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.position.X, e.position.Y)));
                                    //hud.playerScore += 20;
                                    p.bulletList[i].isVisible = false;
                                    e.isVisible = false;
                                }
                            }
                            e.Update(gameTime);
                        }

                        // Update explosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        // For each asteroid in our asteroid list, update it and check for collisions
                        foreach (Asteroid a in asteroidList)
                        {
                            // If asteroids hit the ship then take them off screen
                            if (a.boundingBox.Intersects(p.boundingBox))
                            {
                                p.health -= 20;
                                a.isVisible = false;
                            }

                            // Iterate through bulletList,if any asteroids come in contact with bullets,
                            // Set visibilies to false (bullet and asteroid).
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(p.bulletList[i].boundingBox))
                                {
                                    //sm.explodeSound.Play();
                                    //explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(a.position.X, a.position.Y)));
                                    //hud.playerScore += 5;
                                    a.isVisible = false;
                                    p.bulletList.ElementAt(i).isVisible = false;
                                }
                            }
                            a.Update(gameTime);
                        }

                        //hud.Update(gameTime);

                        // If player health hits 0, then go to gameover state
                        //check this
                        if (p.health <= 0)
                            gameState = State.Gameover;

                        p.Update(gameTime);
                        sf.Update(gameTime);
                        ManageExplosions();
                        LoadAsteroids();
                        LoadEnemies();
                        break;
                    }

                //UPDATING MENU STATE
                case State.Menu:
                    {
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Enter))
                        {

                            gameState = State.Playing;
                            //This doesnt work
                            //Specifically for mp3 files
                            //MediaPLayer.Play(sm.bgMusic);
                        }
                        sf.Update(gameTime);
                        sf.speed = 1;
                        break;
                    }

                //UPDATING GAEMOVER STATE
                case State.Gameover:
                    {
                        //Get keyboard state
                        KeyboardState KeyState = Keyboard.GetState();

                        //If in gameover screen, and user presses esc, return to menu
                        if (KeyState.IsKeyDown(Keys.Escape))
                        {
                            //tut 29
                            p.position = new Vector2(400, 900);

                            //Clearing lists, starting enemies and ship to start
                            enemyList.Clear();
                            asteroidList.Clear();

                            // Reseting so player can play again from menu
                            p.health = 200;
                            //hud.playerScore = 0;

                            // Going back to menu to play again 
                            gameState = State.Menu;
                        }

                        // Stop Music
                        //MediaPLayer.Stop();
                        break;
                    }


            }

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (gameState)
            {
                //DRAWING PLAYING STATE
                case State.Playing:
                    {
                        // Drawing starfield (background) and player
                        sf.Draw(spriteBatch);
                        p.Draw(spriteBatch);

                        // Displaying each explosion
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        // Displaying each asteroid
                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        // Displaying each enemy
                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }
                        //hud.Draw(spriteBatch);
                        break;
                    }

                // DRAWING MENU STATE
                case State.Menu:
                    {
                        sf.Draw(spriteBatch);
                        //spriteBatch.Draw(menuImage, new Vector(0,0), Color.White);
                        break;
                    }

                //DRAWING GAMEOVER STATE
                case State.Gameover:
                    {
                        //spriteBatch.Draw(gameoverImage, new Vector2(0, 0), Color.White);
                        //spriteBatch.DrawString(hud.playerScoreFont, "Your Final Score was - " + hud.playerScore.ToString(), new Vector2(235,100), Color.Red);
                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Load Asteroids, setting random position and adding to list
        public void LoadAsteroids()
        {
            //values from tut 11

            //Creating random variables for asteroids
            int randX = random.Next(0, 550);
            int randY = random.Next(-500, -50);

            // If there are less than 5 asteroids on screen, then create more until there is 5 again
            if (asteroidList.Count < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randX, randY)));
            }

            //If any asteroids are not visible then remove from list
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Enemies

        public void LoadEnemies()
        {
            //values from tut 19

            //Creating random variables for enemies
            int randX = random.Next(0, 550);
            int randY = random.Next(-500, -50);

            // If there are less than 3 enemies on screen, then create more until there is 3 again
            if (enemyList.Count < 3)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyship"), new Vector2(randX, randY),
                    Content.Load<Texture2D>("enemybullet")));
            }

            //If any enemies are not visible then remove from list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Manage Explosions
        public void ManageExplosions()
        {

            for (int i = 0; i < explosionList.Count; i++)
            {
                if (explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;

                }
            }

        }

    }
}
