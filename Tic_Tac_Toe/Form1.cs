using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Tic_Tac_Toe : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        #endregion
        public Tic_Tac_Toe()
        {
            InitializeComponent();
            ChessBoard = new ChessBoardManager(pnlChessBoard, txtbPlayerName, pictbMark);
            //Ủy thác event kết thúc game và đổi lượt 
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            //Cài đặt thuộc tính cho Progress bar 
            prgbarTime.Step = Const.CoolDown_Step;
            prgbarTime.Maximum = Const.CoolDown_Time;
            prgbarTime.Value = 0;

            //Cài đặt thuộc tính cho Timer
            tmCooldown.Interval = Const.CoolDown_Interval;

            ChessBoard.Draw_ChessBoard();

            //tmCooldown.Start();
        }

        void EndGame()
        {
            tmCooldown.Stop();
            pnlChessBoard.Enabled = false;
            //MessageBox.Show("Kết thúc Game!");
        }
        void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {
            tmCooldown.Start();
            prgbarTime.Value = 0;
        }

        void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void tmCooldown_Tick(object sender, EventArgs e)
        {
            prgbarTime.PerformStep(); //Cứ mỗi 100ms (1/10s), hiển thị 1 step

            //Progress bar chạy hết mà vẫn chưa đánh, hiển thị hộp thoại thông báo kết thúc game
            if (prgbarTime.Value >= prgbarTime.Maximum)
            {
                EndGame();
                MessageBox.Show("Mất lượt! Kết thúc Game!");
            } 
                
        }
    }

  
    
}
