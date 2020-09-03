using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Shoot_Em_Up
{
    public class SoundManager
    {
        
        public Song playerShootSound;
        //source : https://freesound.org/people/fins/sounds/191594/
        //The audio file was edited to make it shorter

        public Song explodeSound;
        //source: https://www.zapsplat.com/wp-content/uploads/2015/cc0/cc0_explosion_large_gas_001.mp3
        //The audio file was edited to make it shorter

        public Song bgMusic;
        //https://opengameart.org/content/space-shooter-music (SkyFireTitleScreen)

        public Song collisionsound;
        //Collision (playership and asteroid & enemyship)
        //source: https://www.zapsplat.com/page/4/?s=crash (9th one from the list)
        //The audio file was edited to make it shorter

        public Song gameoversound;
        //Game over sound (when it goes to game over state)
        //source: https://www.zapsplat.com/page/4/?s=crash (First one on the list)
        //The audio file was edited to make it shorter


        public SoundEffect test;


        //Constructor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;
            collisionsound = null;
            gameoversound = null;
        }

        public void LoadContent(ContentManager Content)
        {
            //MP3= song not sound effect
            playerShootSound = Content.Load<Song>("Shootingmp3");
            explodeSound = Content.Load<Song>("explosmp3");
            bgMusic = Content.Load<Song>("TitleScreen");
            collisionsound = Content.Load<Song>("Collisionmp3");
            gameoversound = Content.Load<Song>("GameOvermp3");
       

        }



    }
}
