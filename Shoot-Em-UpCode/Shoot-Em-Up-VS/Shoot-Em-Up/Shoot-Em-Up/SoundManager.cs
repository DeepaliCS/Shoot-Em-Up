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

        public SoundEffect playerShootSound;
        public SoundEffect explodeSound;
        public Song bgMusic;

        //Constructor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;

        }

        public void LoadContent(ContentManager Content)
        {

            playerShootSound = Content.Load<SoundEffect>("");
            explodeSound = Content.Load<SoundEffect>("");

            //MP3= song not sound effect
            bgMusic = Content.Load<Song>("");

        }



    }
}
