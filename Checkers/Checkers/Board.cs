using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Checkers
{
    class Board
    {
        private int S_LENGTH = 75;

        private Texture2D Sprites;
        private BoardTile[,] boardRecs;

        //private List<Piece> gamePieces = new List<Piece>();

        private List<BoardTile> moves = new List<BoardTile>();
        private List<BoardTile> caps = new List<BoardTile>();

        private BoardTile selected;
        PColor turn = PColor.black;


        public Board(Texture2D spriteSheet)
        {
            Sprites = spriteSheet;

            boardRecs = new BoardTile[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardRecs[i, j] = new BoardTile(i, j, new Rectangle(i * S_LENGTH, j * S_LENGTH, S_LENGTH, S_LENGTH));
                }
            }



        }

        public void Setup()
        {
            int sb = 0;
            int r;
            for (int b = 0; b < 24; b++)
            {
                r = 64 - b;
                if (b % 2 == sb)
                {
                    boardRecs[b % 8, b / 8].setPiece(new Piece(PColor.red));
                    boardRecs[7 - (b % 8), 7 - (b / 8)].setPiece(new Piece(PColor.black));

                }

                if ((b + 1) % 8 == 0)
                {
                    sb = 1 - sb;
                }


            }

        }

        private BoardTile getClicked(MouseState m)
        {
            int x_loc = -1;
            int y_loc = -1;

            for (int x = 0; x < 8; x++)
            {
                if (boardRecs[x, 0].inTile(m.X, 40))
                {
                    x_loc = x;
                    break;
                }
            }

            for (int y = 0; y < 8; y++)
            {
                if (boardRecs[0, y].inTile(40, m.Y))
                {
                    y_loc = y;
                    break;
                }
            }



            if (y_loc == -1 || x_loc == -1)
            {
                return null;
            }

            return boardRecs[x_loc, y_loc];

        }

        public void onClick(MouseState m)
        {
            BoardTile sel = getClicked(m);

            if (sel == null) return;

            //See if sel is in moves

            foreach (BoardTile bt in moves)
            {
                if (sel.Equals(bt))
                {
                    doMove(false, selected, sel);
                    return;
                }
            }

            foreach (BoardTile bt in caps)
            {
                if (sel.Equals(bt))
                {
                    doMove(true, selected, sel);
                    return;
                }
            }

            selected = sel;

            //Set move options

            moves = new List<BoardTile>();
            caps = new List<BoardTile>();

            if (!selected.hasPiece()) return; //No piece, return

            Piece p = selected.getPiece();

            if (p.getColor() != turn) return; //Not correct color, return

            BoardTile t_move;
            //Black

            if (p.getColor() == PColor.black)
            {
                ////////////////////// Upper - Left

                if (selected.x > 0 && selected.y > 0)
                {
                    t_move = boardRecs[selected.x - 1, selected.y - 1];

                    if (t_move.hasPiece())
                    {
                        //Test for capture
                        if (t_move.getPiece().getColor() != turn)
                        {
                            if (selected.x > 1 && selected.y > 1)
                            {
                                if (!boardRecs[selected.x - 2, selected.y - 2].hasPiece())
                                {
                                    //Can capture
                                    caps.Add(boardRecs[selected.x - 2, selected.y - 2]);

                                }

                            }
                        }
                    }
                    else
                    {
                        //Empty
                        moves.Add(t_move);
                    }


                }

                ///////////////////////////// Upper - Right

                if (selected.x < 7 && selected.y > 0)
                {
                    t_move = boardRecs[selected.x + 1, selected.y - 1];

                    if (t_move.hasPiece())
                    {
                        //Test for capture
                        if (t_move.getPiece().getColor() != turn)
                        {
                            if (selected.x < 6 && selected.y > 1)
                            {
                                if (!boardRecs[selected.x + 2, selected.y - 2].hasPiece())
                                {
                                    //Can capture
                                    caps.Add(boardRecs[selected.x + 2, selected.y - 2]);
                                }

                            }
                        }
                    }
                    else
                    {
                        //Empty
                        moves.Add(t_move);
                    }


                }

                ////////////////////////// Kings Only
                if (p.isKing())
                {
                    //////////////////////// Lower - Left

                    if (selected.x > 0 && selected.y < 7)
                    {
                        t_move = boardRecs[selected.x - 1, selected.y + 1];

                        if (t_move.hasPiece())
                        {
                            //Test for capture
                            if (t_move.getPiece().getColor() != turn)
                            {
                                if (selected.x > 1 && selected.y < 6)
                                {
                                    if (!boardRecs[selected.x - 2, selected.y + 2].hasPiece())
                                    {
                                        //Can capture
                                        caps.Add(boardRecs[selected.x - 2, selected.y + 2]);
                                    }

                                }
                            }
                        }
                        else
                        {
                            //Empty
                            moves.Add(t_move);
                        }


                    }



                    /////////////////////// Lower - Right

                    if (selected.x < 7 && selected.y < 7)
                    {
                        t_move = boardRecs[selected.x + 1, selected.y + 1];

                        if (t_move.hasPiece())
                        {
                            //Test for capture
                            if (t_move.getPiece().getColor() != turn)
                            {
                                if (selected.x < 6 && selected.y < 6)
                                {
                                    if (!boardRecs[selected.x + 2, selected.y + 2].hasPiece())
                                    {
                                        //Can capture
                                        caps.Add(boardRecs[selected.x + 2, selected.y + 2]);
                                    }

                                }
                            }
                        }
                        else
                        {
                            //Empty
                            moves.Add(t_move);
                        }


                    }

                } // End Kings

            } // End Black
            else
            {
                ///////////// Red

                //////////////////////// Lower - Left

                if (selected.x > 0 && selected.y < 7)
                {
                    t_move = boardRecs[selected.x - 1, selected.y + 1];

                    if (t_move.hasPiece())
                    {
                        //Test for capture
                        if (t_move.getPiece().getColor() != turn)
                        {
                            if (selected.x > 1 && selected.y < 6)
                            {
                                if (!boardRecs[selected.x - 2, selected.y + 2].hasPiece())
                                {
                                    //Can capture
                                    caps.Add(boardRecs[selected.x - 2, selected.y + 2]);
                                }

                            }
                        }
                    }
                    else
                    {
                        //Empty
                        moves.Add(t_move);
                    }


                }



                /////////////////////// Lower - Right

                if (selected.x < 7 && selected.y < 7)
                {
                    t_move = boardRecs[selected.x + 1, selected.y + 1];

                    if (t_move.hasPiece())
                    {
                        //Test for capture
                        if (t_move.getPiece().getColor() != turn)
                        {
                            if (selected.x < 6 && selected.y < 6)
                            {
                                if (!boardRecs[selected.x + 2, selected.y + 2].hasPiece())
                                {
                                    //Can capture
                                    caps.Add(boardRecs[selected.x + 2, selected.y + 2]);
                                }

                            }
                        }
                    }
                    else
                    {
                        //Empty
                        moves.Add(t_move);
                    }


                }

               
                ////////////////////////// Kings Only
                if (p.isKing())
                {
                    ////////////////////// Upper - Left

                    if (selected.x > 0 && selected.y > 0)
                    {
                        t_move = boardRecs[selected.x - 1, selected.y - 1];

                        if (t_move.hasPiece())
                        {
                            //Test for capture
                            if (t_move.getPiece().getColor() != turn)
                            {
                                if (selected.x > 1 && selected.y > 1)
                                {
                                    if (!boardRecs[selected.x - 2, selected.y - 2].hasPiece())
                                    {
                                        //Can capture
                                        caps.Add(boardRecs[selected.x - 2, selected.y - 2]);
                                    }

                                }
                            }
                        }
                        else
                        {
                            //Empty
                            moves.Add(t_move);
                        }


                    }

                    ///////////////////////////// Upper - Right

                    if (selected.x < 7 && selected.y > 0)
                    {
                        t_move = boardRecs[selected.x + 1, selected.y - 1];

                        if (t_move.hasPiece())
                        {
                            //Test for capture
                            if (t_move.getPiece().getColor() != turn)
                            {
                                if (selected.x < 6 && selected.y > 1)
                                {
                                    if (!boardRecs[selected.x + 2, selected.y - 2].hasPiece())
                                    {
                                        //Can capture
                                        caps.Add(boardRecs[selected.x + 2, selected.y - 2]);
                                    }

                                }
                            }
                        }
                        else
                        {
                            //Empty
                            moves.Add(t_move);
                        }


                    }


                } // End Kings



            } //End Red




        }
        public void doMove(bool cap, BoardTile from, BoardTile to)
        {
            to.setPiece(from.getPiece());

            if (to.y == 0 || to.y == 7)
            {
                to.getPiece().setKing(true);
            }

            from.remPiece();

            if (!cap)
            {
                turn = 1 - turn; //Switch player
            }
            else
            {
                if (to.x > from.x)
                {
                    if (to.y > from.y)
                    {
                        //LR
                        boardRecs[from.x + 1, from.y + 1].remPiece();

                    }
                    else
                    {
                        //UR
                        boardRecs[from.x + 1, from.y - 1].remPiece();

                    }

                }
                else
                {
                    if (to.y > from.y)
                    {
                        //LL
                        boardRecs[from.x - 1, from.y + 1].remPiece();

                    }
                    else
                    {
                        //UL
                        boardRecs[from.x - 1, from.y - 1].remPiece();


                    }

                }
            }
            moves = new List<BoardTile>();
            caps = new List<BoardTile>();
        }

        public void drawBoard(SpriteBatch g)
        {
            Rectangle black = new Rectangle(0, 0, 80, 80);
            Rectangle red = new Rectangle(80, 0, 80, 80);

            Rectangle Rmove = new Rectangle(0, 240, 80, 80);
            Rectangle Rcap = new Rectangle(80, 240, 80, 80);

            Rectangle blackP = new Rectangle(0, 80, 80, 80);
            Rectangle redP = new Rectangle(80, 80, 80, 80);

            Rectangle blackPK = new Rectangle(0, 160, 80, 80);
            Rectangle redPK = new Rectangle(80, 160, 80, 80);

            int sn = 0;
            int sb = 1;

            g.Begin();

            foreach (BoardTile r in boardRecs)
            {

                if (sn % 2 == sb)
                {
                    g.Draw(Sprites, r.getTile(), black, Color.White);
                }
                else
                {
                    g.Draw(Sprites, r.getTile(), red, Color.White);
                }

                Color clr = Color.White;

                if (r == selected)
                {
                    clr = Color.Yellow;
                }

                if (r.hasPiece())
                {
                    Piece p = r.getPiece();
                    if (p.isKing())
                    {
                        if (p.getColor() == PColor.red)
                        {
                            g.Draw(Sprites, r.getTile(), redPK, clr);
                        }
                        else
                        {
                            g.Draw(Sprites, r.getTile(), blackPK, clr);
                        }
                    }
                    else
                    {
                        if (p.getColor() == PColor.red)
                        {
                            g.Draw(Sprites, r.getTile(), redP, clr);
                        }
                        else
                        {
                            g.Draw(Sprites, r.getTile(), blackP, clr);
                        }
                    }

                }

                sn++;

                if (sn == 8)
                {
                    sn = 0;
                    sb = 1 - sb;
                }



            }
            foreach (BoardTile m in moves)
            {

                g.Draw(Sprites, m.getTile(), Rmove, Color.White);

            }

            foreach (BoardTile m in caps)
            {

                g.Draw(Sprites, m.getTile(), Rcap , Color.White);

                /*
                Piece p = m.getPiece();
                if (p.getColor() == PColor.red)
                {
                    g.Draw(Sprites, m.getTile(), redP, Color.White);
                }
                else
                {
                    g.Draw(Sprites, m.getTile(), blackP, Color.White);
                }
                 * */

            }



            g.End();
        }


    }

    class BoardTile
    {
        public int x;
        public int y;

        private Rectangle t;
        private Piece p;

        public BoardTile(int x_loc, int y_loc, Rectangle tile)
        {
            x = x_loc;
            y = y_loc;
            t = tile;
        }

        public Rectangle getTile()
        {
            return t;
        }

        public bool inTile(int x, int y)
        {
            return t.Contains(x, y);
        }

        public bool hasPiece()
        {
            return (p != null);
        }

        public Piece getPiece()
        {
            return p;
        }

        public void setPiece(Piece game_piece)
        {
            p = game_piece;
        }

        public void remPiece()
        {
            p = null;
        }

    }
}
