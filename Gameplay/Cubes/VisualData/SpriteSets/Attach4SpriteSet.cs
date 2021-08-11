using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class Attach4SpriteSet
    {
        public string sprite0;
        public string sprite1;
        public string sprite2_1;
        public string sprite2_2;
        public string sprite3;
        public string sprite4;

        public Attach4SpriteSet(string spriteBaseName)
        {
             sprite0 = spriteBaseName + "0";
             sprite1 = spriteBaseName + "1";
             sprite2_1 = spriteBaseName + "2_1";
             sprite2_2 = spriteBaseName + "2_2";
             sprite3 = spriteBaseName + "3";
             sprite4 = spriteBaseName + "4";
        }

        public (string,bool flipHoriz,bool flipVert,bool rotate) GetSpriteFromNeighbourFlags(bool[] flags)
        {
            if(flags[0])
            {
                if(flags[2])
                {
                    if(flags[4])
                    {
                        if(flags[6])
                        {
                            //NESW-
                            return (sprite4, false, false, false);
                        }
                        else
                        {
                            //NES-
                            return (sprite3, false, false, false);
                        }
                    }
                    else
                    {
                        if (flags[6])
                        {
                            //NEW-
                            return (sprite3, true, false, true);

                        }
                        else
                        {
                            //NE-
                            return (sprite2_1, false, false, false);
                        }

                    }
                }
                else
                {
                    if (flags[4])
                    {
                        if (flags[6])
                        {
                            //NSW-
                            return (sprite3, true, false, false);

                        }
                        else
                        {
                            //NS
                            return (sprite2_2, false, false, false);

                        }
                    }
                    else
                    {
                        if (flags[6])
                        {
                            //NW
                            return (sprite2_1, true, true, true);

                        }
                        else
                        {
                            //N
                            return (sprite1, false, false, false);

                        }
                    }
                }
            }
            else
            {
                if (flags[2])
                {
                    if (flags[4])
                    {
                        if (flags[6])
                        {
                            //ESW-
                            return (sprite3, false, false, true);

                        }
                        else
                        {
                            //ES-
                            return (sprite2_1, false, false, true);

                        }
                    }
                    else
                    {
                        if (flags[6])
                        {
                            //EW-
                            return (sprite2_2, false, false, true);

                        }
                        else
                        {
                            //E-
                            return (sprite1, false, false, true);

                        }

                    }
                }
                else
                {
                    if (flags[4])
                    {
                        if (flags[6])
                        {
                            //SW-
                            return (sprite2_1, true, true, false);

                        }
                        else
                        {
                            //S-
                            return (sprite1, false, true, false);

                        }
                    }
                    else
                    {
                        if (flags[6])
                        {
                            //W-
                            return (sprite1, true, true, true);

                        }
                        else
                        {
                            //-
                            return (sprite0, false, false, false);
                        }
                    }
                }
            }
        }

        /*
        North           =0,
        NorthEast       =1,
        East            =2,
        SouthEast       =3,
        South           =4,
        SouthWest       =5,
        West            =6,
        NorthWest       =7*/

    }

}
