using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Checkers
{
    enum PColor { red = 0, black }

    class Piece
    {
        private bool king;
        private PColor clr;

        public Piece( PColor piece_color)
        {
            
            clr = piece_color;
        }

        public PColor getColor()
        {
            return clr;
        }

        public bool isKing()
        {
            return king;
        }

        public void setKing(bool k)
        {
            king = k;
        }



    }

}
