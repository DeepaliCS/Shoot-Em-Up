using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace Shoot_Em_Up
{
    // Main
    public class GameRoot : Game
    {

        // State Enum
        public enum State
        {
            Menu,
            GamePlay,
            Level2Settings,
            BossLvSettings,
            Gameover
        }

        // Set first state-Menu
        State gameState = State.Menu;

        // Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;
        public Texture2D menuImage;
        public Texture2D gameoverImage;
        public int currentsfSpeed;


        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();


        // Objects
        Player p = new Player();
        Starfield sf = new Starfield();
        HUD hud = new HUD();
        SoundManager smgr = new SoundManager();

        //Used to transition to level 3
        public bool lev3Traversal;

        //Used to say its already passed level 2
        public bool lev2Traversal;

        // Constructor
        public GameRoot()
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
            lev3Traversal = false;
            lev2Traversal = false;
            currentsfSpeed = 6;

        }


        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            hud.LoadContent(Content);
            p.LoadContent(Content);
            sf.LoadContent(Content);
            smgr.LoadContent(Content);

            // Play bg music
            MediaPlayer.Play(smgr.bgMusic);

            // Loading gameover and menu screens
            // (All were made from scratch)
            menuImage = Content.Load<Texture2D>("start");
            gameoverImage = Content.Load<Texture2D>("Gameover");     
        }


        // Update
        protected override void Update(GameTime gameTime)
        {
     
            //UPDATING PLAYING STATE
            switch (gameState)
            {
                //UPDATING MENU STATE
                case State.Menu:
                    {

                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.GamePlay;
                            // Stop bg music
                            MediaPlayer.Stop();
                        }

                        // Setting the level number visible for the player
                        hud.levelNo = 1;

                        // background speed and visibility
                        sf.Update(gameTime);
                        sf.speed = 1;

                        // End of menu statement
                        break;
                    }

                case State.GamePlay:
                    {

                        //background speed
                        sf.speed = currentsfSpeed;

                        // Level 3 spawns and settings
                        if (lev3Traversal == true)
                        {

                            // Updating enemies and checking collision of enemyship to playership
                            foreach (Enemy e in enemyList)
                            {
                                // Check if enemyship is colliding with player
                                if (e.boundingBox.Intersects(p.boundingBox))
                                {
                                    // Play collision sound if player hits enemy ship
                                    MediaPlayer.Play(smgr.collisionsound);

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

                                        // play explosion
                                        MediaPlayer.Play(smgr.explodeSound);

                                        // Add 20 points to score and make bullets invisible
                                        hud.playerScore += 20;
                                        p.bulletList[i].isVisible = false;
                                        e.isVisible = false;
                                    }
                                }
                                e.Update(gameTime);
                            }
                        }

                        // For each asteroid in our asteroid list, update it and check for collisions
                        foreach (Asteroid a in asteroidList)
                        {
                            // If asteroids hit the ship then take them off screen
                            if (a.boundingBox.Intersects(p.boundingBox))
                            {
                                // Play collision sound if player hits asteroid
                                MediaPlayer.Play(smgr.collisionsound);

                                p.health -= 20;
                                a.isVisible = false;
                            }

                            // Iterate through bulletList,if any asteroids come in contact with bullets,
                            // Set visibilies to false (bullet and asteroid).
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(p.bulletList[i].boundingBox))
                                {
                                    // play explosion
                                    MediaPlayer.Play(smgr.explodeSound);
                                    hud.playerScore += 5;
                                    a.isVisible = false;
                                    p.bulletList.ElementAt(i).isVisible = false;
                                }
                            }
                            a.Update(gameTime);
                        }

                        // Running constant updates
                        hud.Update(gameTime);
                        p.Update(gameTime);
                        sf.Update(gameTime);
                        LoadAsteroids();
                        LoadEnemies();

                        // If player health hits 0, then go to gameover state, Do game over state
                        if (p.health <= 0)
                        {
                            gameState = State.Gameover;
                            // Play gloomy game over sound
                            MediaPlayer.Play(smgr.gameoversound);
                        }

                        // If the player reaches a certain number of points then go to level 2, display on spritefont
                        if (hud.playerScore == 60 && lev2Traversal == false) //original = 60
                        {
                            gameState = State.Level2Settings;
                            lev2Traversal = true;
                        }
                        else if (hud.playerScore == 80) //original 80
                        {
                            gameState = State.BossLvSettings;
                        }


                        // End of gameplay statement
                        break;
                    }

                case State.Level2Settings:
                    {
                        // Setting starfield speed, faster than menu
                        currentsfSpeed = 7;

                        // Setting the level number visible for the player
                        hud.levelNo = 2;

                        // Making the speed for each asteroid faster than level 1
                        foreach (Asteroid a in asteroidList)
                        {
                            a.speed = 10;
                        }

                        // Reseting score
                        hud.playerScore = 0;

                        // Going back to gameplay mode after level 2 settings
                        gameState = State.GamePlay;

                        // End of level2 case statement
                        break;
                    }
                case State.BossLvSettings:
                    {
                        // Setting starfield speed, faster than menu
                        currentsfSpeed = 10;

                        // Setting the level number visible for the player
                        hud.levelNo = 3;

                        // Reseting score
                        hud.playerScore = 0;

                        // Going back to game play mode after level 3 settings
                        lev3Traversal = true;
                        gameState = State.GamePlay;

                        // End of boss level case statement
                        break;
                    }

                //UPDATING GAEMOVER STATE
                case State.Gameover:
                    {

                        //Get keyboard state
                        KeyboardState KeyState = Keyboard.GetState();

                        //If in gameover screen, and user presses enter, return to menu
                        if (KeyState.IsKeyDown(Keys.Enter))
                        {
                            // return player to bottom center to 'fair play'
                            p.position = new Vector2(400, 900);

                            //Clearing lists, starting enemies and ship to start
                            enemyList.Clear();
                            asteroidList.Clear();

                            // Reseting so player can play again in full health
                            p.health = 200;

                            // Reseting score
                            hud.playerScore = 0;

                            // Set level 2 & 3 traversal to false to start again
                            lev2Traversal = false;
                            lev3Traversal = false;

                            //Keeping the speed back to level 1 speed
                            foreach (Asteroid a in asteroidList)
                            {
                                a.speed = 8;
                            }

                            // Setting background image speed back to default (level1)
                            currentsfSpeed = 6;

                            // Going back to menu to play again 
                            gameState = State.Menu;
                        }


                        // End of gameover case statement
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
                //DRAWING PLAYING STATE (level 1,2 & 3)
                case State.GamePlay:
                    {
                        // Drawing starfield (background) and player
                        sf.Draw(spriteBatch);
                        p.Draw(spriteBatch);

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
                        hud.Draw(spriteBatch);
                        break;
                    }

                // DRAWING MENU STATE
                case State.Menu:
                    {
                        sf.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0,0), Color.White);
                        break;
                    }
                //DRAWING GAMEOVER STATE
                case State.Gameover:
                    {
                        spriteBatch.Draw(gameoverImage, new Vector2(0, 0), Color.White);

                        // Showing level score in Gameover state
                        spriteBatch.DrawString(hud.playerScoreFont, "Level: " +
                            hud.levelNo.ToString(), new Vector2(320, 150), Color.Red);

                        // Showing final score in Gameover state
                        spriteBatch.DrawString(hud.playerScoreFont, "Final Score: " + 
                            hud.playerScore.ToString(), new Vector2(320, 100), Color.Red);
                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Load Asteroids, setting random position and adding to list
        public void LoadAsteroids()
        {
            // Creating random variables for asteroids
            int randX = random.Next(0, 550);
            int randY = random.Next(-500, -50);

            // If there are less than 10 asteroids on screen, then create more until there is 10 again
            if (asteroidList.Count < 10)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randX, randY)));
            }

            // If any asteroids are not visible then remove from list
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
            // Creating random variables for enemies
            int randX = random.Next(0, 550);
            int randY = random.Next(-500, -50);

            // If there are less than 3 enemies on screen, then create more until there is 3 again
            if (enemyList.Count < 5)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyship"), new Vector2(randX, randY),
                    Content.Load<Texture2D>("bullet")));
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
    }
}
