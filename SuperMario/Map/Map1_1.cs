using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace SuperMario
{
    class Map1_1:Map
    {
        
        public override void loadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("maps/map1_1");
            Mario.loadContent(content);
            Brick.loadContent(content);
            ItemBlock.loadContent(content);
            FireBall.loadContent(content);
            Item.loadContent(content);
            Enemy.loadContent(content);
        }
        public Map1_1()
        {
            mapRectangle = new Rectangle(0, 0, 3392, 224);
            mario = new Mario(50,183);
            sprites = new List<Sprite>();
            sprites.Add(mario);
            initSprites();
            end = 3270;
        }
        public void initSprites()
        {
            #region staticSprite
            //ground
            sprites.Add(new Sprite(new Rectangle(0, 201, 1103, 24)));
            sprites.Add(new Sprite(new Rectangle(1136, 201, 239, 24)));
            sprites.Add(new Sprite(new Rectangle(1424, 201, 1023, 24)));
            sprites.Add(new Sprite(new Rectangle(2480, 201, 912, 24)));
            //pipe
            sprites.Add(new Sprite(new Rectangle(448, 168, 32, 32)));
            sprites.Add(new Sprite(new Rectangle(608, 152, 32, 48)));
            sprites.Add(new Sprite(new Rectangle(736, 136, 32, 64)));
            sprites.Add(new Sprite(new Rectangle(912, 136, 32, 64)));
            sprites.Add(new Sprite(new Rectangle(2608, 168, 32, 32)));
            sprites.Add(new Sprite(new Rectangle(2864, 168, 32, 32)));
            //stair
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                sprites.Add(new Sprite(new Rectangle(2144 + 16 * i, 184 - 16 * i, 16, 16 * (i + 1))));
            }

            sprites.Add(new Sprite(new Rectangle(2240, 136, 16, 64)));
            sprites.Add(new Sprite(new Rectangle(2256, 152, 16, 48)));
            sprites.Add(new Sprite(new Rectangle(2272, 168, 16, 32)));
            sprites.Add(new Sprite(new Rectangle(2288, 184, 16, 16)));

            sprites.Add(new Sprite(new Rectangle(2368, 184, 16, 16)));
            sprites.Add(new Sprite(new Rectangle(2384, 168, 16, 32)));
            sprites.Add(new Sprite(new Rectangle(2400, 152, 16, 48)));
            sprites.Add(new Sprite(new Rectangle(2416, 136, 32, 64)));

            sprites.Add(new Sprite(new Rectangle(2480, 136, 16, 64)));
            sprites.Add(new Sprite(new Rectangle(2496, 152, 16, 48)));
            sprites.Add(new Sprite(new Rectangle(2512, 168, 16, 32)));
            sprites.Add(new Sprite(new Rectangle(2528, 184, 16, 16)));

            for (i = 0; i < 7; i++)
            {
                sprites.Add(new Sprite(new Rectangle(2896 + 16 * i, 184 - 16 * i, 16, 16 * (i + 1))));
            }
            sprites.Add(new Sprite(new Rectangle(2896 + 16 * i, 184 - 16 * i, 32, 16 * (i + 1))));

            sprites.Add(new Sprite(new Rectangle(3168, 184, 16, 16)));
            //Item
            Item bigMushoom1 = new BigMushoom(335, 119);
            sprites.Add(bigMushoom1);
            Item flower1 = new Flower(351, 55),flower2= new Flower(1247,119),flower3=new Flower(1743,55);
            sprites.Add(flower1);
            sprites.Add(flower2);
            sprites.Add(flower3);
            Item star1 = new Star(1615, 119);
            sprites.Add(star1);
            Item coin1 = new Coin(255, 119), coin2 = new Coin(367, 119),coin3=new Coin(1503,55);
            Item coin4 = new Coin(1695, 119), coin5 = new Coin(1743, 119), coin6 = new Coin(1791, 119);
            Item coin7 = new Coin(2063, 55), coin8 = new Coin(2079, 55), coin9 = new Coin(2719, 119);
            sprites.Add(coin3);
            sprites.Add(coin2);
            sprites.Add(coin1);
            sprites.Add(coin4);
            sprites.Add(coin5);
            sprites.Add(coin6);
            //brick
            sprites.Add(new Brick(319, 135));
            sprites.Add(new Brick(351, 135));
            sprites.Add(new Brick(383, 135));
            sprites.Add(new Brick(1231, 135));
            sprites.Add(new Brick(1263, 135));
            sprites.Add(new Brick(1279, 71));
            sprites.Add(new Brick(1296, 71));
            sprites.Add(new Brick(1312, 71));
            sprites.Add(new Brick(1328, 71));
            sprites.Add(new Brick(1344, 71));
            sprites.Add(new Brick(1360, 71));
            sprites.Add(new Brick(1376, 71));
            sprites.Add(new Brick(1392, 71));
            sprites.Add(new Brick(1455, 71));
            sprites.Add(new Brick(1471, 71));
            sprites.Add(new Brick(1487, 71));
            sprites.Add(new Brick(1503, 135));
            sprites.Add(new Brick(1599, 135));
            sprites.Add(new Brick(1615, 135,star1));
            sprites.Add(new Brick(1887, 135));
            sprites.Add(new Brick(1935, 71));
            sprites.Add(new Brick(1951, 71));
            sprites.Add(new Brick(1967, 71));
            sprites.Add(new Brick(2047, 71));
            sprites.Add(new Brick(2095, 71));
            sprites.Add(new Brick(2063, 135));
            sprites.Add(new Brick(2079, 135));
            sprites.Add(new Brick(2687, 135));
            sprites.Add(new Brick(2703, 135));
            sprites.Add(new Brick(2735, 135));
            //ItemBlock
            sprites.Add(new ItemBlock(255, 135,coin1));
            sprites.Add(new ItemBlock(351, 71,flower1));
            sprites.Add(new ItemBlock(335, 135,bigMushoom1));
            sprites.Add(new ItemBlock(367, 135,coin2));
            sprites.Add(new ItemBlock(1247, 135,flower2));
            sprites.Add(new ItemBlock(1503, 71,coin3));
            sprites.Add(new ItemBlock(1695, 135,coin4));
            sprites.Add(new ItemBlock(1743, 135,coin5));
            sprites.Add(new ItemBlock(1791, 135,coin6));
            sprites.Add(new ItemBlock(1743, 71,flower3));
            sprites.Add(new ItemBlock(2063, 71,coin7));
            sprites.Add(new ItemBlock(2079, 71,coin8));
            sprites.Add(new ItemBlock(2719, 135,coin9));
            //Enemies
            enemies = new List<Enemy>();
            //Goomba
            enemies.Add(new Goomba(430, 170, false));
            enemies.Add(new Goomba(690, 170, false));
            enemies.Add(new Goomba(870, 183));
            enemies.Add(new Goomba(800, 183, false));
            enemies.Add(new Goomba(1367, 0, false));
            enemies.Add(new Goomba(1399, 0, false));
            enemies.Add(new Goomba(1850, 183, false));
            enemies.Add(new Goomba(1720, 183, false));
            enemies.Add(new Goomba(1930, 183, false));
            enemies.Add(new Goomba(1950, 183, false));
            enemies.Add(new Goomba(2000, 183, false));
            enemies.Add(new Goomba(2040, 183, false));
            enemies.Add(new Goomba(2100, 183, false));
            enemies.Add(new Goomba(2120, 183, false));
            enemies.Add(new Goomba(2780, 183, false));
            enemies.Add(new Goomba(2810, 183, false));
            enemies.Add(new Turtle(1600, 170, false));
            //Flag
            Flag flag = new Flag(3159, 35);
            Pole pole = new Pole(flag, 35);
            sprites.Add(flag);
            sprites.Add(pole);
            castleFlag = new CastleFlag(3270, 120);
            sprites.Add(castleFlag);
            #endregion
        }
    }
}
