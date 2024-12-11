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

            NewGame();

        }

        //Hàm kết thúc Game
        void EndGame()
        {
            tmCooldown.Stop();
            pnlChessBoard.Enabled = false;
        }

        //Hàm bắt đầu tính thời gian của từng lượt 
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

        //Hàm bắt đầu Game mới
        void NewGame()
        {
            prgbarTime.Value = 0;
            tmCooldown.Stop();
            ChessBoard.Draw_ChessBoard();
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        //Hàm thoát Game 
        void QuitGame() 
        {
            Application.Exit();
        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitGame();
        }
        private void Tic_Tac_Toe_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát Game không?", "THÔNG BÁO", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true; //Thay đổi event
        }


    }

  
    
}
