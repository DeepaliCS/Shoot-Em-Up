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
    public class HUD
    {

        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;

        // Constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 800;
            screenWidth = 600;

            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth / 2, 50);

        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            //ADD FONT
            playerScoreFont = Content.Load<SpriteFont>("georgia");

        }

        // Update
        public void Update(GameTime gameTime)
        {
            //Get Keyboard state 
            KeyboardState keyState = Keyboard.GetState();

        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //If we are showing our HUD(if showHud ==true) then display our HUD
            if (showHud)
                spriteBatch.DrawString(playerScoreFont, "Score - " + playerScore, playerScorePos, Color.Red);

        }

    }
}
