using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public class ChessBoardManage
    {
        //Vùng thuộc tính của Panel cần sử dụng
        #region Properties
        private Panel chessboard;
        public Panel ChessBoard
        {
            get { return chessboard; }
            set { chessboard = value; }
        }
        #endregion
        //Vùng phương thức khởi tạo
        #region Initialize
        public ChessBoardManage(Panel chessboard)
        {
            this.ChessBoard = chessboard;
        }
        #endregion
        //Vùng phương thức vẽ bàn cờ
        #region Methods
        public void Draw_ChessBoard()
        {
            Button oldbutton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Const.ChessBoard_H; i++)
            {
                for (int j = 0; j < Const.ChessBoard_W; j++)
                {
                    Button button = new Button()
                    {
                        Width = Const.Chess_W,
                        Height = Const.Chess_H,
                        Location = new Point(oldbutton.Location.X + oldbutton.Width, oldbutton.Location.Y)
                    };

                    ChessBoard.Controls.Add(button);
                    oldbutton = button;
                }
                oldbutton.Location = new Point(0, oldbutton.Location.Y + Const.Chess_H);
                oldbutton.Width = 0;
                oldbutton.Height = 0;

            }
        }
        #endregion

    }
}
